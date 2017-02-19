using UnityEngine;
using System.Collections;

public class EnemyInput : CharacterInput
{
	#region Private members
	#endregion

	#region Public members
	#endregion

	#region Properties
	public bool GoUp
	{
		get; set;
	}

	public bool GoDown
	{
		get; set;
	}

	public bool GoLeft
	{
		get; set;
	}

	public bool GoRight
	{
		get; set;
	}

	public bool Attack
	{
		get; set;
	}

	public bool Special1
	{
		get; set;
	}

	public bool Special2
	{
		get; set;
	}

	#endregion

	#region Events
	#endregion

	#region Monobehavior calls
	// Update is called once per frame
	protected override void Update () {
		
		if(LockInput)
			return;
		
		KeyUpPressed = GoUp;
		KeyDownPressed = GoDown;
		KeyLeftPressed = GoLeft;
		KeyRightPressed = GoRight;
		KeyAttackPressed = Attack;
		KeySpecialPressed1 = Special1;
		KeySpecialPressed2 = Special2;
	}
	#endregion

	#region Private methods
	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		GoUp = false;
		GoDown = false;
		GoLeft = false;
		GoRight = false;
		Attack = false;
		Special1 = false;
		Special2 = false;
	}
	#endregion

	#region Public methods
	#endregion



}

