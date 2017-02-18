using UnityEngine;
using System.Collections;

public class EnemyTameable : MonoBehaviour
{
	#region Private members

	[SerializeField]
	private EnemyIdentity _enemyIdentity = null;

	[SerializeField]
	private GameObject[] _enemyAttackColliderContainers = null;

	[SerializeField]
	private GameObject _enemyHitColliderContainer = null;

	[SerializeField]
	private Material _tamedEnemyMaterialReference = null;

	#endregion

	#region Public members

	public EnemyIdentity EnemyIdentity
	{
		get { return _enemyIdentity; }
	}

	#endregion

	#region Properties

	#endregion

	#region Events

	public delegate void EnemyTamed ();
	public event EnemyTamed EnemyTamedEvent;

	#endregion

	#region MonoBehaviour calls

	#endregion

	#region Private methods

	private void SwitchCollidersLayers()
	{
		foreach (GameObject go in _enemyAttackColliderContainers)
		{
			go.layer = LayerMask.NameToLayer(Tags.LAYER_PLAYER_ATTACK_COLLIDER);
		}

		_enemyHitColliderContainer.layer = LayerMask.NameToLayer(Tags.LAYER_PLAYER_HIT_COLLIDER);
	}

	private void ModifyEnemyAIElements(Transform positionToFollow)
	{		
		_enemyIdentity.EnemyAIController.SetTargetInRangeOfAttack (null);
		_enemyIdentity.EnemyAIController.TransformToFollow = positionToFollow;
	}

	private void ModifyEnemyStats(CharacterStats tamerStats)
	{
		_enemyIdentity.CharacterStats.MovementSpeed = tamerStats.MovementSpeed;
		_enemyIdentity.CharacterStats.AttackDamage = tamerStats.AttackDamage;
	}

	#endregion

	#region Public methods

	public void TameEnemy(Transform positionToFollow, CharacterStats tamerStats)
	{
		SwitchCollidersLayers ();
		ModifyEnemyAIElements (positionToFollow);
		_enemyIdentity.SpriteRenderer.sharedMaterial = _tamedEnemyMaterialReference;

		if (EnemyTamedEvent != null)
			EnemyTamedEvent ();
	}

	public void AttackTarget(EnemyIdentity target)
	{
		_enemyIdentity.EnemyAIController.Target = target;
	}

	#endregion

}
