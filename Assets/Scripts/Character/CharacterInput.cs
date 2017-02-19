using UnityEngine;
using System.Collections;

//PRIORITY 1
public class CharacterInput : CharacterComponent {

	public delegate void StartRunning();
	public event StartRunning StartRunningEvent;

	private bool _lockInput = false;

	#region Properties

	public bool LockInput
	{
		get { return _lockInput; }
		set 
		{ 
			_lockInput = value;
			KeyUpPressed = false;
			KeyDownPressed = false;
			KeyLeftPressed = false;
			KeyLeftReleased = true;
			KeyRightPressed = false;
			KeyRightReleased = false;
			KeyAttackPressed = false;
			KeySpecialPressed1 = false;
			KeySpecialPressed2 = false;
		}
	}

	public bool KeyUpPressed
	{
		get; set;
	}

	public bool KeyDownPressed
	{
		get; set;
	}

	public bool KeyLeftPressed
	{
		get; set;
	}

	public bool KeyLeftReleased
	{
		get; set;
	}

	public bool KeyRightPressed
	{
		get; set;
	}

	public bool KeyRightReleased
	{
		get; set;
	} 

	public bool KeyAttackPressed
	{
		get; set;
	}

	public bool KeySpecialPressed1
	{
		get; set;
	}

	public bool KeySpecialPressed2
	{
		get; set;
	}

	#endregion

	#region Public methods

	public void RaiseStartRunningEvent()
	{
		StartRunningEvent();
	}

	#endregion

	#region Monobehavior calls
	protected override void Awake ()
	{
		base.Awake ();
		LockInput = false;
	}
	#endregion
		
}
