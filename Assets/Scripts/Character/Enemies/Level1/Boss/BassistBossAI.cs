using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BassistBossAI : BossAI
{
	private enum Side
	{
		LEFT, RIGHT
	}

	private enum SpecialAttack
	{
		PLUCK = 1, CHORD = 2
	}

	#region Private members

	private const float OFFSET_FROM_SCENE_LIMITS = 2.0f;
	private const float MAX_LERP_PARAM = 1.0f;
	private const float DISTANCE_TO_CHECK_IF_FALLING_DEST = 0.1f;

	[SerializeField] private DialogUnit _dialogPreBattle = null;

	[Header ("Params for Special Attacks")]
	[SerializeField]
	private int _maxNumOfSoundwavesPlucking = 15;
	[SerializeField]
	private float _maxTimeBetweenSpecials = 2.0f;

	[Header ("GameObject references")]
	[SerializeField]
	private GameObject _spikesParticle = null;
	[SerializeField]
	private GameObject _shield = null;
	[SerializeField]
	private GameObject _soundWaveProjectilePrefab = null;

	[Header ("Speakers data")]
	[SerializeField]
	private Transform _rightSpeakersPosition = null;
	[SerializeField]
	private Transform _leftSpeakersPosition = null;
	[SerializeField]
	private GameObject _speakersPrefab = null;

	private GameObject _rightSpeakers = null;
	private GameObject _leftSpeakers = null;

	private SpecialAttack _currentSpecialAttack = SpecialAttack.CHORD;
	private List<Transform> _originsToUse = new List<Transform> ();

	private Side _currentSide = Side.RIGHT;
	private Vector3 _posAfterRoll = Constants.Vector3.zero;

	private float _initialY;
	private float _lastRelativeHealth = 1.0f;
	private float _currentLerping = 0.02f;

	private bool _isRolling = false;
	private bool _isPlaying = false;

	private Coroutine _behaviourCoroutine = null;

	private List<Transform> _rightSpeakersOrigins = new List<Transform> ();
	private List<Transform> _leftSpeakersOrigins = new List<Transform>();

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();

		_characterIdentity.CharacterHit.CharacterGetsHitEvent += delegate {
			PlayVictoryScene();
		};
		_characterIdentity.CharacterHit.HealthChangeEvent += ChangeMaxTimeBetweenSpecials;
		DialogManager.Singleton.CurrentDialogEndedEvent += DialogEndedCallback;
	}

	protected override void Start ()
	{
		base.Start ();
		_characterIdentity.CharacterMovement.HeadingDirection = Constants.Vector2.left;
	}

	protected override void Update ()
	{
		if (_isRolling)
		{
			_currentLerping += Time.deltaTime * 0.1f;
			if (_currentLerping >= MAX_LERP_PARAM)
				_currentLerping = MAX_LERP_PARAM;
			float lerp = _currentLerping / MAX_LERP_PARAM;

			TransformRef.position = Vector3.Lerp (TransformRef.position, _posAfterRoll, lerp);
			if (Vector3.Distance (TransformRef.position, _posAfterRoll) < DISTANCE_TO_CHECK_IF_FALLING_DEST)
			{					
				_characterIdentity.CharacterAnimation.SetAnimationBool ("Roll", false);
				_isRolling = false;
				_behaviourCoroutine = StartCoroutine (BassistBehaviorCoroutine());
				_currentLerping = 0.0f;
			}
		}
		else if(_targetInRangeOfAttack != null)
		{
			if (!_readyToAttack)
			{
				StartCoroutine (StartAttackCoroutine ());
			}
		}
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();
		DialogManager.Singleton.CurrentDialogEndedEvent -= DialogEndedCallback;
	}


	#endregion

	#region Private methods

	private IEnumerator BassistBehaviorCoroutine()
	{
		do
		{			
			yield return new WaitForSeconds(Random.Range(_maxTimeBetweenSpecials * 0.5f,_maxTimeBetweenSpecials));
			if(CanPerformSpecial())
			{	
				int special = Random.Range(1,3);
				_currentSpecialAttack = (SpecialAttack) special;
				_characterIdentity.CharacterAnimation.SetAnimationTrigger("SpecialAttack" + special);
				FillOriginsList();
			}		
		} while(true);
	}

	protected override IEnumerator StartAttackCoroutine ()
	{
		_readyToAttack = true;
		_characterIdentity.CharacterAttack.AttackCounter = 3;
		yield return new WaitForSeconds (1.0f);
		_readyToAttack = false;
	}

	private void DialogEndedCallback (DialogUnit dialog)
	{
		if (dialog == _dialogPreBattle)
			StartBehaviour ();		
	}

	private void FillOriginsList()
	{
		_isPlaying = true;

		if (_lastRelativeHealth > 0.2f)
		{
			_shield.gameObject.SetActive (true);
			_spikesParticle.gameObject.SetActive (true);
		}

		List<Transform> projectileOrigins = _currentSide == Side.LEFT ? _leftSpeakersOrigins : _rightSpeakersOrigins;

		int num = 0;
		switch (_currentSpecialAttack)
		{
			case SpecialAttack.PLUCK:
				num = Random.Range (_maxNumOfSoundwavesPlucking / 2, _maxNumOfSoundwavesPlucking);
				for (int i = 0; i < num; ++i)
				{
					_originsToUse.Add (projectileOrigins [Random.Range(0,4)]);
				}

				break;
			case SpecialAttack.CHORD:
				num = Random.Range (0, 4);
				for (int i = 0; i < 4; ++i)
				{
					if (i != num)
						_originsToUse.Add (projectileOrigins [i]);
				}
				break;
		}	
	}

	protected bool CanPerformSpecial()
	{
		return!(_characterIdentity.CharacterAttack.IsAttacking || _characterIdentity.CharacterHit.IsBeingHit || _isRolling || _isPlaying);
	}

	protected void AnimationEndRollingEndsCallback()
	{
		if (_currentSide == Side.RIGHT)
		{
			_currentSide = Side.LEFT;
			_characterIdentity.CharacterMovement.HeadingDirection = Constants.Vector2.right;
		}
		else
		{
			_currentSide = Side.RIGHT;
			_characterIdentity.CharacterMovement.HeadingDirection = Constants.Vector2.left;
		}
	}

	protected void AnimationStandUpEndsCallbackAI()
	{
		_originsToUse.Clear ();
		StopCoroutine (_behaviourCoroutine);
		_characterIdentity.CharacterAnimation.SetAnimationBool("Roll",true);
		_readyToAttack = false;
		_targetInRangeOfAttack = null;
		float nextLimitXpos;
		_isRolling = true;
		if (_currentSide == Side.RIGHT)
		{
			nextLimitXpos = StageManager.Singleton.LeftLimit.position.x + OFFSET_FROM_SCENE_LIMITS;
		}
		else
		{
			nextLimitXpos = StageManager.Singleton.RightLimit.position.x - OFFSET_FROM_SCENE_LIMITS;
		}
		_posAfterRoll = StageManager.Singleton.Get3DPosition (new Vector3 (nextLimitXpos, _initialY, 0.0f));
	} 

	protected void ThrowProjectile()
	{
		switch (_currentSpecialAttack)
		{
			case SpecialAttack.PLUCK:
				CreateProjectile (_originsToUse [0]);
				_originsToUse.RemoveAt (0);
				if(_originsToUse.Count == 0)
					_characterIdentity.CharacterAnimation.SetAnimationTrigger("SpecialAttack1");		
				break;

			case SpecialAttack.CHORD:
				for (int i = 0; i < _originsToUse.Count; ++i)
				{
					CreateProjectile (_originsToUse [i]);
				}
				_originsToUse.Clear ();
				break;
		}
	}

	protected void CreateProjectile(Transform origin)
	{
		Vector3 adjust = new Vector3 (0.0f, 1.0f, 0.0f);

		Projectile projectile = ObjectPoolManager.Singleton.Instantiate (_soundWaveProjectilePrefab, origin.position + adjust, 
			Quaternion.identity).GetComponent<Projectile>();
		projectile.transform.right = -_characterIdentity.CharacterMovement.HeadingDirection;
		projectile.ProjectileDirection = _characterIdentity.CharacterMovement.HeadingDirection;
		projectile.owner = _characterIdentity;
		projectile.gameObject.SetActive (true);
	}

	private void StopPlaying()
	{
		_isPlaying = false;
		_shield.gameObject.SetActive (false);
		_spikesParticle.gameObject.SetActive (false);
	}

	private void ChangeMaxTimeBetweenSpecials (float relativeHealth)
	{
		_lastRelativeHealth = relativeHealth;
		float max = _maxTimeBetweenSpecials / relativeHealth;
		_maxTimeBetweenSpecials = max * relativeHealth;
	}


	private void CreateSpeakers()
	{
		_rightSpeakers = GameObject.Instantiate (_speakersPrefab, _rightSpeakersPosition.position, Quaternion.identity) as GameObject;

		_leftSpeakers = GameObject.Instantiate (_speakersPrefab, _leftSpeakersPosition.position, Quaternion.Euler(0.0f,180.0f,0.0f)) as GameObject;

		_initialY = TransformRef.position.y;
		_rightSpeakers.GetComponentsInChildren<Transform> (_rightSpeakersOrigins);
		_rightSpeakersOrigins.RemoveAt (0);
		_leftSpeakers.GetComponentsInChildren<Transform> (_leftSpeakersOrigins);
		_leftSpeakersOrigins.RemoveAt (0);
	}

	#endregion

	#region Public methods

	public void StartBehaviour()
	{
		((BossIdentity)_characterIdentity).ActivateBoss ();
		CreateSpeakers ();
		_behaviourCoroutine = StartCoroutine (BassistBehaviorCoroutine ());
	}

	public void PlayVictoryScene()
	{
		GameManager.Singleton.LoadScene (UIManager.Singleton.player1Info.VictorySceneName);
	}

	#endregion

}
