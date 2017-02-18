using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossPlayerDetector : EnhancedMonoBehaviour 
{
	#region Private members
	[SerializeField]
	protected BossIdentity _bossIdentity;

	protected PlayerIdentity _detectedPlayer = null;
	protected EnemyAI _bossAIController;

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
		_bossAIController = _bossIdentity.BossAI;
	}

	protected virtual void OnTriggerEnter(Collider other)
	{	
		if (_detectedPlayer != null)
			return;

		if (!_playersDetected.Contains (other)) {
			_playersDetected.Add (other);
			other.gameObject.GetComponent < ColliderDisabledAnnouncer> ().ColliderDisabledEvent += OnTriggerExit;
		}

		_detectedPlayer = other.transform.parent.GetComponent<PlayerIdentity> ();
		_bossAIController.SetTargetInRangeOfAttack (_detectedPlayer);
	}

	protected virtual void OnTriggerExit(Collider other)
	{		
		if (_detectedPlayer == null)
			return;

		if (_playersDetected.Contains (other)) {
			other.gameObject.GetComponent < ColliderDisabledAnnouncer> ().ColliderDisabledEvent -= OnTriggerExit;
			_playersDetected.Remove (other);
		}

		_detectedPlayer = null;
		_bossAIController.SetTargetInRangeOfAttack(null);
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();
		_detectedPlayer = null;
		_bossAIController.SetTargetInRangeOfAttack(null);

		HashSet<Collider>.Enumerator iterator = _playersDetected.GetEnumerator ();
		while (iterator.MoveNext ()) 
		{
			iterator.Current.gameObject.GetComponent<ColliderDisabledAnnouncer> ().ColliderDisabledEvent -= OnTriggerExit;
		}
	}

	#endregion

	#region Private methods
	#endregion

	#region Public methods
	#endregion
}
