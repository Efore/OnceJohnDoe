using UnityEngine;
using System.Collections;

[RequireComponent(typeof(EnemyAI))]
public class BossIdentity : CharacterIdentity
{
	#region Private members

	[SerializeField]private GameObject _healthBarPrefab = null;

	#endregion

	#region Public members
	public BossAI BossAI = null;
	public BossPlayerDetector BossPlayerDetectorController = null;
	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	protected override void OnEnable ()
	{
		base.OnEnable ();

	}

	protected override void Start ()
	{
		
	}

	protected override void OnDisable ()
	{		
		base.OnDisable ();
		UIManager.Singleton.RemoveBossInfo ();
		CharacterManager.Singleton.UnregisterBoss();
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	public void ActivateBoss()
	{
		UIManager.Singleton.AddBossInfo (_healthBarPrefab);
		CharacterManager.Singleton.RegisterBoss (this);
	}

	#endregion

}
