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

	protected IEnumerator SpecialAttack1Coroutine()
	{
		yield return new WaitForSeconds (_tamingDuration);
		KillTamedEnemies ();
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
		StartCoroutine (SpecialAttack1Coroutine ());
		SpecialAttack1Ends();
	}

	protected override void FinishSpecialAttack1Effect ()
	{
		_characterIdentity.CharacterInput.LockInput = false;
		_characterIdentity.CharacterHit.IsInvulnerable = false;
		_characterIdentity.CharacterAnimation.SetAnimationBool("SpecialAttack1",false);
	}

	protected override void PerformSpecialAttack2 ()
	{
		if (_tamedEnemies.Count == 0)
			return;
		
		base.PerformSpecialAttack2 ();
	}

	protected override void StartSpecialAttack2Effect ()
	{
		CommandTamedEnemies ();
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

			if (enemyTameable != null)
			{
				_tamedEnemies.Add(enemyTameable);
				enemyTameable.EnemyIdentity.CharacterHit.CharacterDefeatedEvent += () => { _tamedEnemies.Remove(enemyTameable); };
				enemyTameable.TameEnemy (_tamedEnemyPositions [bodyGuardPositionIndex],_characterIdentity.CharacterStats);
				bodyGuardPositionIndex++;
			}

			if (bodyGuardPositionIndex == _tamedEnemyPositions.Length)
				break;
		}

		for (int i = 0; i < _tamedEnemies.Count; ++i)
			CharacterManager.Singleton.Enemies.Remove (_tamedEnemies [i].EnemyIdentity);

	}

	private void CommandTamedEnemies()
	{
		for (int i = 0; i < _tamedEnemies.Count; ++i)
			_tamedEnemies [i].AttackTarget (CharacterManager.Singleton.GetRandomEnemy ());
	}

	private void KillTamedEnemies()
	{
		foreach (EnemyTameable enemy in _tamedEnemies)
			enemy.EnemyIdentity.CharacterHit.KillCharacter ();
	}

	private float CalculateMetalPointRecovery()
	{
		float maxRecovery = _maxMetalRecoveryPerSecond * 0.01f;
		maxRecovery -= maxRecovery * _characterIdentity.CharacterMovement.RelativeSpeed;
		return maxRecovery;
	}

	#endregion

	#region Public methods

	#endregion

}
