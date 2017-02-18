using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterIdentity))]
public class  CharacterComponent : EnhancedMonoBehaviour
{
	#region Private members
	protected CharacterIdentity _characterIdentity;
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
		_characterIdentity = transform.root.GetComponent<CharacterIdentity>();
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();
		RestartComponent();
	}
	#endregion

	#region Private methods

	protected virtual bool CheckIfPossible()
	{
		return true;
	}

	protected virtual void RestartComponent()
	{}
	#endregion

	#region Public methods
	#endregion



}
