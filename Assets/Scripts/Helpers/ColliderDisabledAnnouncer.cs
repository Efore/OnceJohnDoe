using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class ColliderDisabledAnnouncer : MonoBehaviour {

	private Collider _collider;

	public delegate void ColliderDisabled(Collider collider);
	public event ColliderDisabled ColliderDisabledEvent;

	void Awake()
	{
		_collider = GetComponent<Collider> ();
	}

	void OnDisable()
	{
		if(ColliderDisabledEvent != null)
			ColliderDisabledEvent (_collider);
	}
}
