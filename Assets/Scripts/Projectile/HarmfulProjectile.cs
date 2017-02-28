using UnityEngine;
using System.Collections;

public class HarmfulProjectile : Projectile 
{
	#region Private members
	#endregion

	#region Public members
	#endregion

	#region Properties
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	#endregion

	#region Private methods
	protected override void ExecuteAtCollision (Collider other)
	{
		CharacterIdentity victim = other.transform.root.GetComponent<CharacterIdentity>();

		if(victim == null)
			return;
		
		victim.CharacterHit.GetHit(owner,1,ProjectileDirection,_damage);
	}
	#endregion

	#region Public methods
	#endregion

}
