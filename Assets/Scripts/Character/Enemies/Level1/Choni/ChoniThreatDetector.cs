
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChoniThreatDetector : EnhancedMonoBehaviour 
{
	private enum RunDirection {NONE, LEFT, RIGHT };

	#region Private members
	[SerializeField]
	private EnemyIdentity _enemyIdentity = null;

	private bool _isInDanger = false;
	private RunDirection _runDirection;
	private HashSet<Collider> _potentialThreats = new HashSet<Collider>();

	#endregion

	#region Public members
	#endregion

	#region Properties
	public bool IsInDanger
	{
		get { return _isInDanger; }
		set
		{
			_isInDanger = value;
			_enemyIdentity.CharacterMovement.IsRunning = IsInDanger;
		}
	}
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.transform.parent == this.transform.parent)
			return;
		
		if (other.transform.root.tag == Tags.PLAYER && !_potentialThreats.Contains (other))
		{
			_potentialThreats.Add (other);
			other.gameObject.GetComponent<ColliderDisabledAnnouncer> ().ColliderDisabledEvent += OnTriggerExit;

			if(!IsInDanger)
			{
				StartFleeing (other);
			}
		}
		
	}

	void OnTriggerExit(Collider other)
	{
		if(other.gameObject.transform.parent == this.transform.parent)
			return;
		
		if (other.transform.root.tag == Tags.PLAYER && _potentialThreats.Contains (other))
		{
			_potentialThreats.Remove (other);
			other.gameObject.GetComponent<ColliderDisabledAnnouncer> ().ColliderDisabledEvent -= OnTriggerExit;
			if (_potentialThreats.Count == 0)
			{
				StartCoroutine (StopFleeingCoroutine ());
			}
		}
	}

	protected override void OnDisable()
	{
		HashSet<Collider>.Enumerator iterator = _potentialThreats.GetEnumerator ();
		while (iterator.MoveNext ()) 
		{
			iterator.Current.gameObject.GetComponent<ColliderDisabledAnnouncer> ().ColliderDisabledEvent -= OnTriggerExit;
		}
	}

	#endregion

	#region Private methods

	private IEnumerator StopFleeingCoroutine()
	{	
		yield return new WaitForSeconds(1.5f);

		if (_potentialThreats.Count > 0)
			yield break; 
		
		CancelFleeing ();
	}

	private void StartFleeing(Collider other)
	{
		_enemyIdentity.EnemyPlayerDetectorController.enabled = false;
		_enemyIdentity.CharacterAttack.StopAttack();
		_enemyIdentity.CharacterAnimation.SetAnimationInt("ProjectileType",-1);
		_enemyIdentity.EnemyAIController.ReadyToAttack = false;
		_enemyIdentity.EnemyAIController.SetCanMove(false);
		_enemyIdentity.EnemyAIController.SetCanMove(true);

		if(other.transform.position.x > this.TransformRef.position.x)
			_runDirection = RunDirection.LEFT;
		else
			_runDirection = RunDirection.RIGHT;

		switch (_runDirection)
		{
		case RunDirection.LEFT:
			((EnemyInput)_enemyIdentity.CharacterInput).GoRight = false;
			((EnemyInput)_enemyIdentity.CharacterInput).GoLeft = true;
			break;
		case RunDirection.RIGHT:
			((EnemyInput)_enemyIdentity.CharacterInput).GoLeft = false;
			((EnemyInput)_enemyIdentity.CharacterInput).GoRight = true;
			break;
		}
		IsInDanger = true;
	}

	private void CancelFleeing()
	{
		IsInDanger = false;
		switch (_runDirection)
		{
		case RunDirection.LEFT:
			((EnemyInput)_enemyIdentity.CharacterInput).GoLeft = false;
			break;
		case RunDirection.RIGHT:
			((EnemyInput)_enemyIdentity.CharacterInput).GoRight = false;
			break;
		}
		_enemyIdentity.EnemyPlayerDetectorController.enabled = true;
		_runDirection = RunDirection.NONE;
	}
	#endregion

	#region Public methods
	#endregion



}
