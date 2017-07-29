
using UnityEngine;
using System.Collections;

public class DestroyableByBeingHit : EnhancedMonoBehaviour
{
	#region Private members
	[SerializeField] private Animator _animator = null;
	private Collider _collider = null;
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
		_collider = GetComponent<Collider> ();
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.transform.parent.GetComponent<CharacterHit> ()!= null && other.transform.parent.GetComponent<CharacterHit> ().IsBeingHit)
		{
			if (_animator != null)
			{
				_animator.enabled = true;
				_collider.enabled = false;
			}
		}
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}
