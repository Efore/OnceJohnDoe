using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DaniSpecialAttack : PlayerSpecialAttack
{
	#region Private members

	[Header("Dani params")]
	[SerializeField]
	private Transform[] _tamedEnemyPositions = null;

	[SerializeField]
	private int _tamingDuration = 30;

	[SerializeField]
	private float _maxMetalRecoveryPerSecond = 10;

	private List<EnemyTameable> _tamedEnemies = new List<EnemyTameable> ();

	private float _currentTamingDuration = 0.0f;

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
	}

	protected override void Start()
	{
		base.Start ();
		StartCoroutine (MetalRecoveryCoroutine ());
		_currentTamingDuration = _tamingDuration;
	}

	protected override void Update ()
	{
		base.Update ();
		if (_currentTamingDuration < _tamingDuration)
		{
			_currentTamingDuration -= Time.deltaTime;

			if (_currentTamingDuration < 10.0f)
				for (int i = 0; i < _tamedEnemies.Count; ++i)
				{
					_tamedEnemies [i].TamingCountDown.gameObject.SetActive (true);
					_tamedEnemies [i].TamingCountDown.fillAmount = _currentTamingDuration / 10.0f;
				}

			if (_currentTamingDuration <= 0.0f)
			{
				_currentTamingDuration = _tamingDuration;
				KillTamedEnemies ();
			}
		}

	}

	#endregion

	#region Private methods

	protected IEnumerator MetalRecoveryCoroutine()
	{
		WaitForSeconds waitForSeconds = new WaitForSeconds (0.01f);
		do
		{
			if(_characterIdentity.CharacterHit.CurrentHealth > 0)
			{
				CurrentMetalPoints += CalculateMetalPointRecovery();
			}
			yield return waitForSeconds;
		} while(true);
	}

	protected override void PerformSpecialAttack1 ()
	{
		if (_tamedEnemies.Count > 0)
			return;
		
		base.PerformSpecialAttack1 ();
	}

	protected override void StartSpecialAttack1Effect ()
	{
		TameEnemies ();
		SpecialAttack1Ends();
	}

	protected override void FinishSpecialAttack1Effect ()
	{
		_characterIdentity.CharacterInput.LockInput = false;
		_characterIdentity.CharacterHit.IsInvulnerable = false;
		_characterIdentity.CharacterAnimation.SetAnimationBool("SpecialAttack1",false);
		foreach (EnemyIdentity enemy in CharacterManager.Singleton.Enemies)
			enemy.CharacterInput.LockInput = false;
		foreach (EnemyTameable enemyTameable in _tamedEnemies)
			enemyTameable.EnemyIdentity.CharacterInput.LockInput = false;
	}

	protected override void PerformSpecialAttack2 ()
	{
		if (_tamedEnemies.Count == 0)
			return;
		
		base.PerformSpecialAttack2 ();
	}

	protected override void StartSpecialAttack2Effect ()
	{
		SpecialAttack2Ends ();
	}

	protected override void FinishSpecialAttack2Effect ()
	{
		_characterIdentity.CharacterInput.LockInput = false;
		_characterIdentity.CharacterHit.IsInvulnerable = false;
		_characterIdentity.CharacterAnimation.SetAnimationBool("SpecialAttack2",false);
	}

	private void TameEnemies()
	{
		int bodyGuardPositionIndex = 0;
		foreach (EnemyIdentity enemy in CharacterManager.Singleton.Enemies)
		{
			EnemyTameable enemyTameable = enemy.GetComponent<EnemyTameable> ();

			if (bodyGuardPositionIndex == _tamedEnemyPositions.Length)
				continue;

			if (enemyTameable != null)
			{
				_tamedEnemies.Add(enemyTameable);
				enemyTameable.EnemyIdentity.CharacterHit.CharacterDefeatedEvent += 
					() => { 
						_tamedEnemies.Remove (enemyTameable);
						if (_tamedEnemies.Count == 0)
							_currentTamingDuration = _tamingDuration;
					};

				enemyTameable.TameEnemy (_tamedEnemyPositions [bodyGuardPositionIndex],_characterIdentity.CharacterStats);
				bodyGuardPositionIndex++;
			}
		}

		for (int i = 0; i < _tamedEnemies.Count; ++i)
			CharacterManager.Singleton.Enemies.Remove (_tamedEnemies [i].EnemyIdentity);

		if(_tamedEnemies.Count > 0)
			_currentTamingDuration -= 0.01f;
	}

	private void CommandTamedEnemies()
	{
		if (CharacterManager.Singleton.Enemies.Count == 0)
			return;

		List<EnemyIdentity> _enemiesInFront = new List<EnemyIdentity> ();

		for (int i = 0; i < CharacterManager.Singleton.Enemies.Count; ++i)
		{
			if (_characterIdentity.TransformRef.InverseTransformPoint(CharacterManager.Singleton.Enemies [i].TransformRef.position).x > 0)
				_enemiesInFront.Add (CharacterManager.Singleton.Enemies [i]);
		}

		if (_enemiesInFront.Count > 0)
		{
			for (int i = 0; i < _tamedEnemies.Count; ++i)
				_tamedEnemies [i].AttackTarget (_enemiesInFront [Random.Range (0, _enemiesInFront.Count)]);			
		}
	}

	private void KillTamedEnemies()
	{
		foreach (EnemyTameable enemy in _tamedEnemies)
		{
			Vector2 headingDirection = enemy.EnemyIdentity.TransformRef.position.x < TransformRef.position.x ? Constants.Vector2.left :
				Constants.Vector2.right;

			enemy.EnemyIdentity.CharacterHit.GetHit (_characterIdentity, 3, headingDirection, 1000.0f, false);
		}
	}

	private float CalculateMetalPointRecovery()
	{
		float maxRecovery = _maxMetalRecoveryPerSecond * 0.01f;
		maxRecovery -= maxRecovery * _characterIdentity.CharacterMovement.RelativeSpeed;
		return maxRecovery;
	}

	#endregion

	#region Public methods

	public void CreateWaves()
	{
		CameraManager.Singleton.SpecialEffectWaveScreen (TransformRef.position);
		foreach (EnemyIdentity enemy in CharacterManager.Singleton.Enemies)
		{
			if(enemy.GetComponent<EnemyTameable>() != null)
				enemy.CharacterInput.LockInput = true;
		}
	}

	#endregion

}
