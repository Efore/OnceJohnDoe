
using UnityEngine;
using System.Collections;

public class EnemyLongRangePlayerDetector : EnhancedMonoBehaviour
{
	#region Private members
	[SerializeField]protected EnemyIdentity _enemyIdentity;
	[SerializeField]private BoxCollider _boxCollider;

	protected EnemyAI _enemyAIController;
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
		_enemyAIController = _enemyIdentity.EnemyAIController;
	}


	protected void OnTriggerStay(Collider other)
	{
		if (((KnifeCaniAI)_enemyAIController).IsCharging)
			return;

		if (_enemyAIController.GetTargetInRangeOfAttack() != null)
			return;
	
		if(Mathf.Abs(other.transform.position.x - _enemyIdentity.TransformRef.position.x) < (_boxCollider.bounds.size.x / 2))
			return;
				
		StartRunning ();
	}

	#endregion

	#region Private methods
	private void StartRunning()
	{
		float limitX = _enemyIdentity.CharacterMovement.HeadingDirection ==Constants.Vector2.right ? _boxCollider.bounds.size.x : -_boxCollider.bounds.size.x;
		limitX += _enemyIdentity.TransformRef.position.x;

		((KnifeCaniAI)_enemyAIController).XCoordLimit = limitX;
		((KnifeCaniAI)_enemyAIController).IsCharging = true;

		if(limitX > _enemyIdentity.TransformRef.position.x)
			((KnifeCaniAI)_enemyAIController).ChargingDirection =Constants.Vector2.right;
		else
			((KnifeCaniAI)_enemyAIController).ChargingDirection =Constants.Vector2.left;
		
	}
	#endregion

	#region Public methods

	#endregion

}
