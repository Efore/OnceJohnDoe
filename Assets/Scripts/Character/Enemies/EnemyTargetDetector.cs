using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyTargetDetector : EnhancedMonoBehaviour 
{
	#region Private members
	[SerializeField]
	protected EnemyIdentity _enemyIdentity;

	protected CharacterIdentity _detectedTarget = null;
	protected EnemyAI _enemyAIController;

	private HashSet<Collider> _playersDetected = new HashSet<Collider>();
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
		_enemyIdentity.GetComponent<CharacterHit>().CharacterGetsHitEvent += CharacterGetsHitCallback;
	}


	protected virtual void OnTriggerEnter(Collider other)
	{	
		if (_detectedTarget != null)
			return;

		_detectedTarget = other.transform.parent.GetComponent<CharacterIdentity> ();

		if (!_playersDetected.Contains (other)) {
			_playersDetected.Add (other);
			other.gameObject.GetComponent <ColliderDisabledAnnouncer> ().ColliderDisabledEvent += OnTriggerExit;
		}

		if (_enemyAIController.Target != _detectedTarget)
			_enemyAIController.Target = _detectedTarget;
		
		_enemyAIController.SetTargetInRangeOfAttack (_detectedTarget);
	}

	protected virtual void OnTriggerExit(Collider other)
	{	
		if (_playersDetected.Contains (other)) {
			other.gameObject.GetComponent < ColliderDisabledAnnouncer> ().ColliderDisabledEvent -= OnTriggerExit;
			_playersDetected.Remove (other);
		}

		if (_playersDetected.Count == 0)
		{
			_detectedTarget = null;
			_enemyAIController.SetTargetInRangeOfAttack (null);
		}
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();
		_detectedTarget = null;
		_enemyAIController.SetTargetInRangeOfAttack(null);

		HashSet<Collider>.Enumerator iterator = _playersDetected.GetEnumerator ();
		while (iterator.MoveNext ()) 
		{
			if(iterator.Current.gameObject.GetComponent<ColliderDisabledAnnouncer> () != null)
				iterator.Current.gameObject.GetComponent<ColliderDisabledAnnouncer> ().ColliderDisabledEvent -= OnTriggerExit;
		}
	}

	#endregion

	#region Private methods

	private void CharacterGetsHitCallback (CharacterIdentity attacker, int numHit)
	{
		if(numHit == 3)
		{
			_detectedTarget = null;
			_enemyAIController.SetTargetInRangeOfAttack(null);
		}
	}


	#endregion

	#region Public methods
	#endregion
}
