using UnityEngine;
using System.Collections;

public class PlayerInput : CharacterInput
{
	#region Private members

	[SerializeField]
	private float _maxTimeForCheckIfRuns = 0.5f;

	private bool _checkingRunning = false;

	#endregion

	#region Public members
	[Range (1,2)]
	public int player = Constants.PLAYER_ONE;
	#endregion

	#region Properties
	#endregion

	#region Events
	#endregion

	#region Monobehavior calls

	// Update is called once per frame
	protected override void Update () {
		
		if(LockInput)
			return;
		if (player == Constants.PLAYER_ONE)
		{
			KeyUpPressed = Input.GetKey (KeyCode.W);
			KeyDownPressed = Input.GetKey (KeyCode.S);
			KeyLeftPressed = Input.GetKey (KeyCode.A);
			KeyLeftReleased = Input.GetKeyUp (KeyCode.A);
			KeyRightPressed = Input.GetKey (KeyCode.D);
			KeyRightReleased = Input.GetKeyUp (KeyCode.D);
			KeyAttackPressed = Input.GetKeyDown (KeyCode.G);
			KeySpecialPressed1 = Input.GetKeyDown (KeyCode.H);
			KeySpecialPressed2 = Input.GetKeyDown (KeyCode.J);
		}
		else
		{
			KeyUpPressed = Input.GetKey (KeyCode.UpArrow);
			KeyDownPressed = Input.GetKey (KeyCode.DownArrow);
			KeyLeftPressed = Input.GetKey (KeyCode.LeftArrow);
			KeyLeftReleased = Input.GetKeyUp (KeyCode.LeftArrow);
			KeyRightPressed = Input.GetKey (KeyCode.RightArrow);
			KeyRightReleased = Input.GetKeyUp (KeyCode.RightArrow);
			KeyAttackPressed = Input.GetKeyDown (KeyCode.Return);
			KeySpecialPressed1 = Input.GetKeyDown (KeyCode.RightShift);
			KeySpecialPressed2 = Input.GetKeyDown (KeyCode.RightControl);
		}

		if(KeyLeftPressed && !_checkingRunning)
			StartCoroutine(CheckRunCoroutine(false));
		else if(KeyRightPressed && !_checkingRunning)
			StartCoroutine(CheckRunCoroutine(true));
	}
	#endregion

	#region Private methods

	protected IEnumerator CheckRunCoroutine(bool right)
	{
		_checkingRunning = true; 
		float timeAcum = 0.0f;
		bool keyUp = false;
		bool mustRun = false;

		if(right)
		{
			do
			{
				timeAcum += Time.deltaTime;
				if(KeyRightReleased)
					keyUp = true;

				if(keyUp && KeyRightPressed)
					mustRun = true;

				yield return new WaitForEndOfFrame();
			}while(timeAcum < _maxTimeForCheckIfRuns && !mustRun);
		}
		else
		{
			do
			{
				timeAcum += Time.deltaTime;

				if(KeyLeftReleased)
					keyUp = true;

				if(keyUp &&  KeyLeftPressed)
					mustRun = true;

				yield return new WaitForEndOfFrame();
			}while(timeAcum < _maxTimeForCheckIfRuns && !mustRun);
		}

		_checkingRunning = false;
		if(mustRun)
			RaiseStartRunningEvent();
	}

	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_checkingRunning = false;
	}

	#endregion

	#region Public methods
	#endregion



}
