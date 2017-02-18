using UnityEngine;
using System.Collections;

public class EnemyAI : CharacterComponent
{
	#region Private members

	private const float OFFSET_TO_ALIGN = 0.2f;

	protected CharacterIdentity _target = null;
	protected CharacterIdentity _targetInRangeOfAttack = null; 
	protected EnemyInput _enemyInput = null;

	protected Transform _transformToFollow = null;

	protected bool _readyToAttack = false;
	protected bool _lockMovement = false;



	#endregion

	#region Public members
	#endregion

	#region Properties

	public bool ReadyToAttack
	{
		get { return _readyToAttack; }
		set 
		{
			_readyToAttack = value;
			if (!_readyToAttack) 
			{
				_enemyInput.Attack = false;
			}
		}

	}

	public CharacterIdentity Target
	{
		get { return _target; }
		set { 
			if (_target != null)
				_target.CharacterHit.CharacterDefeatedEvent -= ChangePlayerTarget;				
			_target = value;
			_targetInRangeOfAttack = null;
			if (_target != null)
				_target.CharacterHit.CharacterDefeatedEvent += ChangePlayerTarget;
		}
	}

	public Transform TransformToFollow
	{
		get { return _transformToFollow; }
		set { _transformToFollow = value; }
	}

	#endregion

	#region Events

	public delegate void ChangingPlayerTarget ();
	public event ChangingPlayerTarget ChangingPlayerTargetEvent;

	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();
		Target = null;
	}

	protected override void Start ()
	{
		base.Start ();
		_enemyInput = (EnemyInput)_characterIdentity.CharacterInput;
		ChangePlayerTarget ();
	}

	protected override void Update ()
	{
		base.Update ();

		if (Target == null)
		{			
			if (_transformToFollow == null)
				return;
		
			AlignWithGameObjectToFollow ();
		}
		else
		{
			if (!_lockMovement)
				AlignWithTarget ();
			else if (_targetInRangeOfAttack != null)
			{
				if (!_readyToAttack)
					StartCoroutine (StartAttackCoroutine ());
			}
		}
		SetCanMove(_targetInRangeOfAttack == null);
	}

	#endregion

	#region Private methods

	protected virtual IEnumerator StartAttackCoroutine()
	{
		yield break;
	}

	protected void PlayerRegisteredCallback (PlayerIdentity player)
	{
		if (Target == null)
		{
			Target = player;
		}
	}

	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		Target = null;
		_readyToAttack = false;
		_targetInRangeOfAttack = null;
		_enemyInput = null;
		_lockMovement = false;
	}

	protected void ChangePlayerTarget()
	{		
		Target = CharacterManager.Singleton.GetRandomPlayer();	

		if (ChangingPlayerTargetEvent != null)
			ChangingPlayerTargetEvent ();
	}

	protected void AlignWithTarget()
	{		
		if(_target.TransformRef.position.x - this.TransformRef.position.x > OFFSET_TO_ALIGN)
			_enemyInput.GoRight = true;
		else
			_enemyInput.GoRight = false;

		if(_target.TransformRef.position.x - this.TransformRef.position.x < -OFFSET_TO_ALIGN)
			_enemyInput.GoLeft = true;
		else
			_enemyInput.GoLeft = false;

		if(_target.TransformRef.position.z - this.TransformRef.position.z > OFFSET_TO_ALIGN)
			_enemyInput.GoUp = true;
		else
			_enemyInput.GoUp = false;

		if(_target.TransformRef.position.z - this.TransformRef.position.z < -OFFSET_TO_ALIGN)
			_enemyInput.GoDown = true;
		else
			_enemyInput.GoDown = false;
	}

	protected void AlignWithGameObjectToFollow()
	{
		if(_transformToFollow.position.x - this.TransformRef.position.x > OFFSET_TO_ALIGN)
			_enemyInput.GoRight = true;
		else
			_enemyInput.GoRight = false;

		if(_transformToFollow.position.x - this.TransformRef.position.x < -OFFSET_TO_ALIGN)
			_enemyInput.GoLeft = true;
		else
			_enemyInput.GoLeft = false;

		if(_transformToFollow.position.z - this.TransformRef.position.z > OFFSET_TO_ALIGN)
			_enemyInput.GoUp = true;
		else
			_enemyInput.GoUp = false;

		if(_transformToFollow.position.z - this.TransformRef.position.z < -OFFSET_TO_ALIGN)
			_enemyInput.GoDown = true;
		else
			_enemyInput.GoDown = false;
	}


	#endregion

	#region Public methods

	public CharacterIdentity GetTargetInRangeOfAttack()
	{
		return _targetInRangeOfAttack;
	}

	public virtual void SetTargetInRangeOfAttack(CharacterIdentity targetInRangeOfAttack)
	{		
		if(targetInRangeOfAttack != null)
			Target = targetInRangeOfAttack;
		_targetInRangeOfAttack = targetInRangeOfAttack;
	}

	public void SetCanMove(bool canMove)
	{
		if(!canMove)
		{
			_enemyInput.GoUp = false;
			_enemyInput.GoDown = false;
			_enemyInput.GoLeft = false;
			_enemyInput.GoRight = false;
		}
		_lockMovement = !canMove;
	}

	#endregion



}
