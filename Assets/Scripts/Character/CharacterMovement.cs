using UnityEngine;
using System.Collections;

public class CharacterMovement : CharacterComponent {

	#region Private members 
	private bool _isRunning = false;
	private Vector2 _headingDirection = Constants.Vector2.right;
	#endregion

	#region Properties

	public Vector2 HeadingDirection 
	{
		get { return _headingDirection;	}
		set {
			if (_headingDirection != value)
			{
				_headingDirection = value;
				TransformRef.Rotate (Constants.Vector3.up, 180.0f);
			}
		}
	}

	public bool IsRunning
	{
		get{ return _isRunning; }
		set
		{
			_isRunning = value;
			_characterIdentity.CharacterAnimation.SetAnimationBool("Run",_isRunning);
		}
	}

	public Vector2 Movement
	{
		get; set;
	}

	public bool TopBlocked
	{
		get;
		set;
	}

	public bool BottomBlocked
	{
		get;
		set;
	}

	public bool LeftBlocked
	{
		get;
		set;
	}

	public bool RightBlocked
	{
		get;
		set;
	}

	public float RelativeSpeed
	{
		get { return Movement.magnitude / (_characterIdentity.CharacterStats.MovementSpeed * 2.0f * Time.deltaTime) ; }
	}

	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();
		_characterIdentity.CharacterInput.StartRunningEvent += SetIsRunning;
		TopBlocked = false;
		BottomBlocked = false;
		LeftBlocked = false;
		RightBlocked = false;
	}

	protected override void Start ()
	{
		base.Start ();
	}

	// Update is called once per frame
	protected override void Update () {
		base.Update();

		if(!CheckIfPossible())
			return;
		
		Movement = Constants.Vector2.zero;
		SetMovement();

		Movement *= IsRunning ? (_characterIdentity.CharacterStats.MovementSpeed * 2.0f * Time.deltaTime) 
			: (_characterIdentity.CharacterStats.MovementSpeed * Time.deltaTime);
		
		_characterIdentity.CharacterAnimation.SetAnimationFloat("Speed",Movement.magnitude);
		TransformRef.position = StageManager.Singleton.Get3DMovement (TransformRef.position, Movement);
	}

	#endregion

	#region Private methods

	private void SetIsRunning()
	{
		if(!IsRunning)
			IsRunning = true; 
	}

	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_isRunning = false;
	}

	protected override bool CheckIfPossible ()
	{
		return (!_characterIdentity.CharacterAttack.IsAttacking && !_characterIdentity.CharacterHit.IsBeingHit);
	}

	private void SetMovement()
	{
		
		if (_characterIdentity.CharacterInput.KeyUpPressed && !TopBlocked)
			Movement += Constants.Vector2.up;
		else if (_characterIdentity.CharacterInput.KeyDownPressed && !BottomBlocked)
			Movement += Constants.Vector2.down;
		


		if(_characterIdentity.CharacterInput.KeyLeftPressed)
		{
			if(!LeftBlocked)
				Movement += Constants.Vector2.left;
			
			if(HeadingDirection != Constants.Vector2.left)
				HeadingDirection = Constants.Vector2.left;
		}
		else if(_characterIdentity.CharacterInput.KeyRightPressed)
		{
			if(!RightBlocked)
				Movement += Constants.Vector2.right;
			
			if(HeadingDirection != Constants.Vector2.right)
				HeadingDirection = Constants.Vector2.right;			
		}			
		
		if(Movement.x != 0 && Movement.y != 0)
			Movement *= 0.5f;
		else if(Movement == Constants.Vector2.zero && IsRunning && gameObject.CompareTag(Tags.PLAYER))
		{
			IsRunning = false;
		}
	}

	#endregion

	#region Public methods

	public bool IsSomeDirectionBlocked()
	{
		return (TopBlocked || BottomBlocked || RightBlocked || LeftBlocked);
	}

	public void BlockMovement(bool blocked)
	{
		TopBlocked = BottomBlocked = LeftBlocked = RightBlocked = blocked;
	}
	#endregion
}
