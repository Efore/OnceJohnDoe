#define TEMPORAL

using UnityEngine;
using System.Collections;

public class CharacterHit : CharacterComponent {

	#region Private members 
	[SerializeField]
	private Transform _fxHitParticlePosition = null;

	private const float MIN_FALLING_DISTANCE = 1.0f;
	private const float MAX_FALLING_DISTANCE = 5.0f;
	private const float DISTANCE_PER_DAMAGE_POINT = 0.15f;

	private const float DISTANCE_TO_CHECK_IF_FALLING_DEST = 0.05f;
	private const float LERPING_PARAM = 0.1f;

	private string[] _animationHitNamesToCheck = new string[]{"Hit1Front","Hit2Front","Hit1Back","Hit2Back"};
	private string[] _animationStandUpToCheck = new string[]{"StandUpFront","StandUpBack"};
	private float _initialHealth;
	private float _currentHealth;

	#if TEMPORAL
	private bool _isFalling = false;
	private Vector3 _destFallingPosition = Vector3.zero;

	#endif
	#endregion

	public delegate void CharacterGetsHit(CharacterIdentity attacker, bool bigHit);
	public event CharacterGetsHit CharacterGetsHitEvent;
	public delegate void CharacterDies();
	public event CharacterDies CharacterDiesEvent;
	public delegate void CharacterDissapears();
	public event CharacterDissapears CharacterDefeatedEvent;
	public delegate void HealthChange(float relativeHealth);
	public event HealthChange HealthChangeEvent;

	#region Properties

	public bool IsBeingHit
	{
		get; set;
	}

	public bool IsInvulnerable
	{
		get; set;
	}

	public bool StandWith1hp
	{
		get;
		set;
	}

	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();	

		for(int i = 0; i < _animationHitNamesToCheck.Length; ++i)
			_characterIdentity.CharacterAnimation.AnimationEndsEvents.AddListener(_animationHitNamesToCheck[i],
				new UnityEngine.Events.UnityAction(AnimationHitEndsCallback));

		for(int i = 0; i < _animationStandUpToCheck.Length; ++i)
			_characterIdentity.CharacterAnimation.AnimationEndsEvents.AddListener(_animationStandUpToCheck[i],
				new UnityEngine.Events.UnityAction(AnimationStandUpEndsCallback));
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		IsBeingHit = false;
		IsInvulnerable = false;
		StandWith1hp = false;
		_currentHealth = _characterIdentity.CharacterStats.HealthPoints;
		_initialHealth = _currentHealth;
	}

	protected override void Update ()
	{
		base.Update ();
	
		#if TEMPORAL
		if(_isFalling)
		{			
			TransformRef.position = StageManager.Singleton.Get3DPosition(Vector3.Lerp(TransformRef.position, _destFallingPosition,LERPING_PARAM));

			if(_characterIdentity.CharacterMovement.RightBlocked || _characterIdentity.CharacterMovement.LeftBlocked)
				_isFalling = false;			
			else if(Vector3.Distance(TransformRef.position,_destFallingPosition) < DISTANCE_TO_CHECK_IF_FALLING_DEST)				
				_isFalling = false;			
		}
			
		#endif
	}

	#endregion

	#region Private methods

	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_isFalling = false;
		_destFallingPosition = Vector3.zero;
	}

	protected override bool CheckIfPossible ()
	{
		return (!IsBeingHit && !IsInvulnerable);
	}

	protected void AnimationHitEndsCallback()
	{		
		_characterIdentity.CharacterAnimation.SetAnimationInt("HitBack",0);
		_characterIdentity.CharacterAnimation.SetAnimationInt("HitFront",0);
		IsBeingHit = false;
	}

	protected void AnimationStandUpEndsCallback()
	{
		_characterIdentity.CharacterAnimation.SetAnimationInt("HitBack",0);
		_characterIdentity.CharacterAnimation.SetAnimationInt("HitFront",0);
		IsBeingHit = false;
	} 

	private void CreateFxHitParticle(bool bigHit)
	{
		GameObject original = bigHit ? FxManager.Singleton.fxPrefabHitCharacterText : FxManager.Singleton.fxPrefabHitCharacter;
		ObjectPoolManager.Singleton.Instantiate(original,_fxHitParticlePosition.position,Constants.Quaternion.identity);
	}
	#endregion

	#region Public methods

	public void GetHit(CharacterIdentity attacker, int hitNum, Vector2 attackerHeadingDirection, float damage)
	{
		if(!CheckIfPossible() || hitNum == 0)
			return;
		
		IsBeingHit = true;

		bool front = attackerHeadingDirection != _characterIdentity.CharacterMovement.HeadingDirection;
		bool bigHit = false;
		_currentHealth -= damage;

		if(HealthChangeEvent != null)
			HealthChangeEvent (_currentHealth/_initialHealth);		

		if(_characterIdentity.CharacterAttack.IsAttacking)
			_characterIdentity.CharacterAttack.StopAttack();

		if(_characterIdentity.CharacterMovement.IsRunning)
		{
			_characterIdentity.CharacterMovement.IsRunning = false;
		}

		if(_currentHealth <= 0.0f)
		{
			if (StandWith1hp)
			{
				_currentHealth = 1.0f;
			}
			else
			{
				hitNum = 3;

				if (CharacterDiesEvent != null)
					CharacterDiesEvent ();

				_characterIdentity.CharacterAnimation.SetAnimationBool ("Die", true);
			}
		}

		#if TEMPORAL
		if(hitNum == 3)
		{
			bigHit = true;
			float destY = 0.0f;

			destY = TransformRef.position.y;
			
			float _destX = 0.0f; 

			float horizontalDistance = Mathf.Clamp(damage * DISTANCE_PER_DAMAGE_POINT,MIN_FALLING_DISTANCE,MAX_FALLING_DISTANCE);

			if(front)
				_destX = _characterIdentity.CharacterMovement.HeadingDirection == Constants.Vector2.left ? TransformRef.position.x + 1 * horizontalDistance 
					: TransformRef.position.x + -1 * horizontalDistance;
			else
				_destX = _characterIdentity.CharacterMovement.HeadingDirection == Constants.Vector2.left ? TransformRef.position.x - 1 * horizontalDistance 
					: TransformRef.position.x + 1 * horizontalDistance;

			_destFallingPosition = new Vector3(_destX,destY,TransformRef.position.z);


			_isFalling = true;
		}

		if(front)
			_characterIdentity.CharacterAnimation.SetAnimationInt("HitFront",hitNum);
		else
			_characterIdentity.CharacterAnimation.SetAnimationInt("HitBack",hitNum);

		CreateFxHitParticle(bigHit);

		#endif
		if(CharacterGetsHitEvent != null)
			CharacterGetsHitEvent(attacker,bigHit);
	}

	public void RaiseCharacterDefeatedEvent()
	{
		if (CharacterDefeatedEvent != null)
			CharacterDefeatedEvent ();
	}

	public void RespawnCharacter()
	{
		_currentHealth = _initialHealth;
		_characterIdentity.CharacterAnimation.SetAnimationBool("Die",false);
		HealthChangeEvent (1.0f);
	}
	#endregion
}
