
using UnityEngine;
using System.Collections;

public class Projectile : EnhancedMonoBehaviour
{
	#region Private members
	[SerializeField]
	protected bool _destroyAtContact = false;

	[SerializeField]
	protected float _damage = 10.0f;

	[SerializeField]
	protected float _timeToDestroy = 5.0f;

	[SerializeField]
	protected float _movementSpeed = 6.0f;


	public CharacterIdentity owner = null;

	#endregion


	#region Properties
	public Vector2 ProjectileDirection
	{
		get; set;
	}
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	protected override void Awake ()
	{
		base.Awake ();
		ProjectileDirection = Constants.Vector2.left;
	}

	protected override void OnEnable ()
	{
		base.OnEnable ();
		StartCoroutine(DestroyCoroutine());
	}

	protected override void Update ()
	{
		base.Update ();

		Vector3 movement = ProjectileDirection * (_movementSpeed * Time.deltaTime);

		TransformRef.position += Utils.Singleton.Vector2ToVector3(movement);
	}

	protected void OnTriggerEnter(Collider other)
	{
		ExecuteAtCollision(other);
	}

	#endregion

	#region Private methods
	protected virtual IEnumerator DestroyCoroutine()
	{
		yield return new WaitForSeconds(_timeToDestroy);
		gameObject.SetActive(false);
	}

	protected virtual void ExecuteAtCollision(Collider other)
	{}
	#endregion

	#region Public methods
	#endregion



}
