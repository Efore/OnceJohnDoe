using UnityEngine;
using System.Collections;
 
public class AnimatedHarmfulProjectile : HarmfulProjectile {


	#region Public members
	public Animator Animator;
	#endregion

	[SerializeField]
	protected bool _hasBreakingAnimation = false;

	protected override void ExecuteAtCollision (Collider other)
	{
		base.ExecuteAtCollision (other);
		if(_destroyAtContact)
		{
			if(_hasBreakingAnimation)
				gameObject.GetComponent<Animator>().SetBool("Breaking",true);
			else
				gameObject.SetActive(false);
		}
	}
}
