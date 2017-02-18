using UnityEngine;
using System.Collections;

public class KnifeCaniAI : CaniAI
{
	#region Private members
	[SerializeField]
	protected GameObject _chargingAttackCollider;

	private bool _isCharging = false;

	#endregion

	#region Public members
	#endregion

	#region Properties
	public bool IsCharging
	{
		get { return _isCharging; }
		set {
			_isCharging = value;
			_characterIdentity.CharacterMovement.IsRunning = value;
			_chargingAttackCollider.SetActive (value);
		}
	}

	public Vector2 ChargingDirection
	{
		get;
		set;
	}	

	public float XCoordLimit
	{
		get;
		set;
	}

	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_isCharging = false;
		_chargingAttackCollider.SetActive (false);
	}

	protected override void Update ()
	{
		if (!IsCharging)
			base.Update ();
		else
		{
			_enemyInput.GoUp = false;
			_enemyInput.GoDown = false;
			if (ChargingDirection ==Constants.Vector2.right)
			{
				_enemyInput.GoRight = true;
				_enemyInput.GoLeft = false;
				if (_characterIdentity.TransformRef.position.x > XCoordLimit)
					IsCharging = false;
			}
			else
			{
				_enemyInput.GoLeft = true;
				_enemyInput.GoRight = false;
				if (_characterIdentity.TransformRef.position.x < XCoordLimit)
					IsCharging = false;
			}
		}
	}
	#endregion

	#region Private methods
	#endregion

	#region Public methods
	#endregion

}
