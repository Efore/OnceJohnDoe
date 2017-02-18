using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyAI))]
public class  EnemyIdentity : CharacterIdentity
{
	#region Private members
	#endregion

	#region Public members

	#endregion

	#region Properties
	public EnemyAI EnemyAIController = null;
	public EnemyTargetDetector EnemyPlayerDetectorController = null;
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	protected override void OnEnable()
	{
		base.OnEnable();
		CharacterManager.Singleton.RegisterEnemy(this);
	}

	protected override void Start ()
	{
		base.Start ();
		CharacterHit.CharacterDiesEvent += UnregisterEnemy;
	}

	#endregion

	#region Private methods

	private void UnregisterEnemy ()
	{
		CharacterManager.Singleton.UnregisterEnemy(this);
	}

	#endregion

	#region Public methods
	#endregion



}
