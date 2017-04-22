
using UnityEngine;
using System.Collections;

public class EnemyTrafficCollider : EnhancedMonoBehaviour
{
	#region Private members
	[SerializeField]
	private CharacterMovement _characterMovement = null;

	private bool _hasToStop = false;

	private const float TIME_FOR_TRAFFIC = 2.0f;

	#endregion

	#region Public members

	#endregion

	#region Properties

	public CharacterMovement CharacterMovement
	{
		get { return _characterMovement; }
	}

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void OnTriggerStay(Collider other)
	{
		if (_hasToStop || _characterMovement.IsRunning)
			return;
		
		if (_characterMovement.gameObject.tag != other.gameObject.tag ||
			_characterMovement.HeadingDirection.x != other.GetComponent<EnemyTrafficCollider> ().CharacterMovement.HeadingDirection.x)
			return;

		if((_characterMovement.HeadingDirection.x < 0 && TransformRef.position.x > other.transform.position.x) ||
			(_characterMovement.HeadingDirection.x > 0 && TransformRef.position.x < other.transform.position.x) )
			StartCoroutine (StopForAWhile ());		
	}

	#endregion

	#region Private methods

	private IEnumerator StopForAWhile()
	{
		Debug.Log ("stopYtal");
		_hasToStop = true;
		_characterMovement.HasToWait = true;
		yield return new WaitForSeconds (TIME_FOR_TRAFFIC);
		_characterMovement.HasToWait = false;
		_hasToStop = false;
	}

	#endregion

	#region Public methods

	#endregion

}
