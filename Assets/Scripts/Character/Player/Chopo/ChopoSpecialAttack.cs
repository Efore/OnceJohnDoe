using UnityEngine;
using System.Collections;

public class ChopoSpecialAttack : PlayerSpecialAttack
{
	#region Private members

	[Header("Chopo params")]
	[SerializeField] protected AudioSource _audioSource = null;
	[SerializeField] protected float _metalPointRecoveringBase = 1.0f;
	[Header("Special Attack 1 params")]
	[SerializeField]private RuntimeAnimatorController _superChopoAnimatorController = null;
	[SerializeField]private GameObject _electricBallGameObject = null;
	[SerializeField]private AudioClip _superChopoScream = null;
	[SerializeField]private float _dmgMultiplicator = 2.0f;
	[SerializeField]private float _durationSpecial1 = 10.0f;

	[Header("Special Attack 2 params")]
	[SerializeField]private SpriteRenderer _effectsScreen = null;
	[SerializeField]private int _numberOfTargets = 5;
	[SerializeField]private float _secondsPerEnemy = 0.75f;
	[SerializeField]private ParticleSystem _electricBoltParticle = null;
	[SerializeField]private AudioClip _boltSfx = null;

	private RuntimeAnimatorController _chopoAnimatorController = null;
	private Animator _animator = null;
	private SpriteRenderer _spriteRenderer = null;
	private float _durationSpecial2 = 1.0f;


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
		_animator = GetComponent<Animator>();
		_chopoAnimatorController = _animator.runtimeAnimatorController;
		_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	protected override void Start ()
	{
		base.Start ();
		_characterIdentity.CharacterHit.CharacterGetsHitEvent += RecoverMPByGettingHit;
		_characterIdentity.CharacterAttack.CharacterAttacksCharacterEvent += RecoverMPByAttacking;
	}

	#endregion

	#region Private methods

	private IEnumerator SpecialAttack1Coroutine()
	{
		float elapsedTime = 0.0f;
		while(IsUsingSpecial1)
		{
			elapsedTime += Time.deltaTime;
			if(elapsedTime >= _durationSpecial1 && TryToEndSpecial())
			{
				SpecialAttack1Ends();
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator SpecialAttack2Coroutine()
	{
		float elapsedTime = 0.0f;
		int enemies = 1;
		while(IsUsingSpecial2)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime >= _secondsPerEnemy * enemies)
			{	
				Color c = Color.white;
				c.a = _effectsScreen.color.a;
				_effectsScreen.color = c;
				KillEnemy ();
				++enemies;
				yield return new WaitForSeconds(0.2f);
				c = Color.black;
				c.a = _effectsScreen.color.a;
				_effectsScreen.color = c;
			}
			if(elapsedTime >= _durationSpecial2 || enemies > _numberOfTargets)
			{
				SpecialAttack2Ends();
			}
			yield return new WaitForEndOfFrame();
		}
	}

	protected override void PerformSpecialAttack1 ()
	{
		base.PerformSpecialAttack1 ();
		_audioSource.PlayOneShot (_superChopoScream);
	}

	protected override void StartSpecialAttack1Effect ()
	{
		base.StartSpecialAttack1Effect ();
		_characterIdentity.CharacterInput.LockInput = false;
		_characterIdentity.CharacterHit.StandWith1hp = true;
		_characterIdentity.CharacterHit.IsInvulnerable = false;
		_characterIdentity.CharacterStats.AttackDamage *= _dmgMultiplicator;
		UIManager.Singleton.player1Info.ToogleAlternativeFace (true);
		_animator.runtimeAnimatorController = _superChopoAnimatorController;
		_animator.Rebind();
		StartCoroutine(SpecialAttack1Coroutine());
	}

	protected override void FinishSpecialAttack1Effect ()
	{
		base.FinishSpecialAttack1Effect ();

		TransformRef.localScale /= TransformRef.localScale.x / 1.5f; 
		_spriteRenderer.color = Color.white;
		_electricBallGameObject.SetActive(false);
		_characterIdentity.CharacterHit.StandWith1hp = false;
		_characterIdentity.CharacterStats.AttackDamage /= _dmgMultiplicator;
		UIManager.Singleton.player1Info.ToogleAlternativeFace (false);
		_animator.runtimeAnimatorController = _chopoAnimatorController;
		_animator.Rebind();

	}

	protected override void StartSpecialAttack2Effect ()
	{
		base.StartSpecialAttack2Effect ();
		_durationSpecial2 = Mathf.Max(1.0f, CharacterManager.Singleton.Enemies.Count * _secondsPerEnemy + _secondsPerEnemy);
		StartCoroutine(SpecialAttack2Coroutine());
	}

	protected override void FinishSpecialAttack2Effect ()
	{
		base.FinishSpecialAttack2Effect ();
		_characterIdentity.CharacterInput.LockInput = false;
		_characterIdentity.CharacterHit.IsInvulnerable = false;
		_characterIdentity.CharacterAnimation.SetAnimationBool("SpecialAttack2",false);
	}

	private bool TryToEndSpecial()
	{
		return (!_characterIdentity.CharacterHit.IsBeingHit && !_characterIdentity.CharacterAttack.IsAttacking
			&& !_characterIdentity.CharacterMovement.IsRunning );
	}

	private void KillEnemy()
	{
		if (CharacterManager.Singleton.Enemies.Count == 0)
			return;
		
		EnemyIdentity enemy = CharacterManager.Singleton.GetRandomEnemy ();

		_electricBoltParticle.transform.position = enemy.TransformRef.position;
		_electricBoltParticle.Play (true);
		_audioSource.PlayOneShot (_boltSfx);

		Vector2 headingDirection = enemy.TransformRef.position.x < TransformRef.position.x ? Constants.Vector2.left :
			Constants.Vector2.right;
		
		enemy.CharacterHit.GetHit (_characterIdentity, 3, headingDirection, 1000.0f);
	}

	private void RecoverMetalPoint(float multiplicator)
	{
		CurrentMetalPoints += multiplicator * _metalPointRecoveringBase;
	}

	private void RecoverMPByAttacking (CharacterIdentity victim)
	{
		RecoverMetalPoint (10.0f);
	}

	private void RecoverMPByGettingHit (CharacterIdentity attacker, int hitNum)
	{
		float multiplicator = hitNum == 3 ? 10.0f : 5.0f;
		RecoverMetalPoint (multiplicator);
	}

	#endregion

	#region Public methods


	#endregion



}
