using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterAnimation))]
[RequireComponent(typeof(CharacterInput))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterHit))]
[RequireComponent(typeof(CharacterAttack))]
[RequireComponent(typeof(CharacterMovement))]
[RequireComponent(typeof(SpriteRenderer))]
public class  CharacterIdentity : EnhancedMonoBehaviour
{
	#region Private members
	#endregion

	#region Public members
	public CharacterStats CharacterStats = null;
	public CharacterMovement CharacterMovement = null;
	public SpriteRenderer SpriteRenderer = null;
	public CharacterAnimation CharacterAnimation = null;
	public CharacterInput CharacterInput = null;
	public CharacterHit CharacterHit = null;
	public CharacterAttack CharacterAttack = null;
	#endregion

	#region Properties
	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls
	#endregion

	#region Private methods
	#endregion

	#region Public methods


	public void DestroyThis()
	{
		
	}
	#endregion



}
