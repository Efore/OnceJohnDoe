using UnityEngine;
using System.Collections;

public class DaniSpecialAttack : PlayerSpecialAttack
{
	#region Private members

	[SerializeField]
	private Transform[] _tamedEnemyPositions = null;

	private EnemyTameable[] _tamedEnemies = null;

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
		_tamedEnemies = new EnemyTameable[_tamedEnemyPositions.Length];
	}

	protected override void Update()
	{
		if(Input.GetKeyDown(KeyCode.J))
		{
			TameEnemies ();
		}

		if(Input.GetKeyDown(KeyCode.K))
		{
			CommandTamedEnemies ();
		}
	}

	#endregion

	#region Private methods

	private void TameEnemies()
	{
		int bodyGuardPositionIndex = 0;
		foreach (EnemyIdentity enemy in CharacterManager.Singleton.Enemies)
		{
			EnemyTameable enemyTameable = enemy.GetComponent<EnemyTameable> ();

			if (enemyTameable != null)
			{
				_tamedEnemies [bodyGuardPositionIndex] = enemyTameable;
				enemyTameable.TameEnemy (_tamedEnemyPositions [bodyGuardPositionIndex],_characterIdentity.CharacterStats);
				bodyGuardPositionIndex++;
			}

			if (bodyGuardPositionIndex == _tamedEnemyPositions.Length)
				break;
		}

		for (int i = 0; i < _tamedEnemies.Length; ++i)
			CharacterManager.Singleton.Enemies.Remove (_tamedEnemies [i].EnemyIdentity);

	}

	private void CommandTamedEnemies()
	{
		for (int i = 0; i < _tamedEnemies.Length; ++i)
			_tamedEnemies [i].AttackTarget (CharacterManager.Singleton.GetRandomEnemy ());
	}

	#endregion

	#region Public methods

	#endregion

}
