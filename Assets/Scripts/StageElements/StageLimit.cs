using UnityEngine;
using System.Collections;

public class StageLimit : EnhancedMonoBehaviour
{
	public enum LimitType { TOP, BOTTOM, LEFT, RIGHT}

	#region Private members
	[SerializeField] private BoxCollider _collider;
	[SerializeField] private LimitType _type;
	#endregion

	#region Public members

	#endregion

	#region Properties

	public LimitType Type
	{
		get {
			return _type; 
		}
	}

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls
	void OnCollisionEnter(Collision other)
	{
		CharacterMovement otherMovement = other.collider.transform.root.GetComponent<CharacterMovement> ();
		switch(_type)
		{
			case LimitType.RIGHT:
				otherMovement.RightBlocked = true;
				break;
			case LimitType.LEFT:
				otherMovement.LeftBlocked = true;
				break;
			case LimitType.TOP:
				otherMovement.TopBlocked = true;
				break;
			case LimitType.BOTTOM:
				otherMovement.BottomBlocked = true;
				break;
		}
	}

	void OnCollisionExit(Collision other)
	{
		CharacterMovement otherMovement = other.collider.transform.root.GetComponent<CharacterMovement> ();

		if (otherMovement == null)
			return;

		switch(_type)
		{
			case LimitType.RIGHT:
				otherMovement.RightBlocked = false;
				break;
			case LimitType.LEFT:
				otherMovement.LeftBlocked = false;
				break;
			case LimitType.TOP:
				otherMovement.TopBlocked = false;
				break;
			case LimitType.BOTTOM:
				otherMovement.BottomBlocked = false;
				break;
		}
	}
	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}
