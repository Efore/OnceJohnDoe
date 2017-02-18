using UnityEngine;
using System.Collections;

public class ChargingAttack : EnhancedMonoBehaviour
{
	#region Private members
	[SerializeField]
	private CharacterIdentity _characterIdentity;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls
	void OnTriggerEnter(Collider other)
	{
		CharacterIdentity otherIdentity = other.transform.parent.GetComponent<CharacterIdentity>();
		if(otherIdentity != null)
		{
			otherIdentity.CharacterHit.GetHit(_characterIdentity,_characterIdentity.CharacterAttack.NumMaxAttacks, 
				_characterIdentity.CharacterMovement.HeadingDirection, _characterIdentity.CharacterStats.AttackDamage);
		}
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}
