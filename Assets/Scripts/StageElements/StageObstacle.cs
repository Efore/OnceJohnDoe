using UnityEngine;
using System.Collections;

public class StageObstacle : EnhancedMonoBehaviour
{
	private enum PathBlocked {NONE, TOP, BOTTOM, LEFT, RIGHT};

	#region Private members
	[SerializeField]private BoxCollider _collider;
	private PathBlocked _pathBlocked = PathBlocked.NONE;
	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls
	void OnCollisionEnter(Collision other)
	{
		CharacterMovement otherMovement = other.collider.transform.root.GetComponent<CharacterMovement> ();

		if (otherMovement == null)
			return;
		Vector3 otherPosition = otherMovement.TransformRef.position;
		Vector3 contactPoint = other.contacts[other.contacts.Length/2].point;

		if (Mathf.Abs(contactPoint.x - TransformRef.position.x) < (_collider.bounds.size.x / 2))
		{
			if (otherPosition.y > TransformRef.position.y || otherPosition.z > TransformRef.position.z)
			{
				_pathBlocked = PathBlocked.BOTTOM;
				otherMovement.BottomBlocked = true;
			}
			else if (otherPosition.y < TransformRef.position.y || otherPosition.z < TransformRef.position.z)
			{
				_pathBlocked = PathBlocked.TOP;
				otherMovement.TopBlocked = true;	
			}
		}
		else
		{
			if (otherPosition.x > TransformRef.position.x)
			{
				_pathBlocked = PathBlocked.LEFT;
				otherMovement.LeftBlocked = true;
			}
			else if(otherPosition.x < TransformRef.position.x)
			{
				_pathBlocked = PathBlocked.RIGHT;
				otherMovement.RightBlocked = true;	
			}
		}

	}

	void OnCollisionExit(Collision other)
	{
		CharacterMovement otherMovement = other.collider.transform.root.GetComponent<CharacterMovement> ();

		if (otherMovement == null)
			return;

		switch(_pathBlocked)
		{
			case PathBlocked.TOP:
				otherMovement.TopBlocked = false;
				break;
			case PathBlocked.BOTTOM:
				otherMovement.BottomBlocked = false;
				break;
			case PathBlocked.LEFT:
				otherMovement.LeftBlocked = false;
				break;
			case PathBlocked.RIGHT:
				otherMovement.RightBlocked = false;
				break;
		}
		_pathBlocked = PathBlocked.NONE;
	}
	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}
