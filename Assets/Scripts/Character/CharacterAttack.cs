using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class CharacterAttack : CharacterComponent {

	public delegate void AttackFinished();
	public event AttackFinished AttackFinishedEvent;
	public delegate void ComboFinished();
	public event ComboFinished ComboFinishedEvent;

	#region Private members

	private bool _isAttackingAndRunning = false;
	private bool _pendingAttack = false;
	private int _attackCounter = 0;
	private Vector3 _posAfterAttackRunning = Vector3.zero;
	private string[] _animationNamesToCheck = new string[]{"Attack1","Attack2","Attack3","RunAttack"};

	#endregion

	#region Events

	public delegate void CharacterAttacksCharacter (CharacterIdentity victim);
	public event CharacterAttacksCharacter CharacterAttacksCharacterEvent; 

	#endregion

	#region Properties

	public int AttackCounter
	{
		get { return _attackCounter; }
		set
		{
			_attackCounter = value;
			_characterIdentity.CharacterAnimation.SetAnimationInt("Attack",_attackCounter);
		}
	}

	public int NumMaxAttacks
	{
		get; set;
	}

	public bool IsAttacking
	{
		get { return _attackCounter > 0; }
	}

	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();
		NumMaxAttacks = _characterIdentity.CharacterStats.MaxAttacks;	

		for(int i = 0; i < _animationNamesToCheck.Length; ++i)
		{
			_characterIdentity.CharacterAnimation.AnimationEndsEvents.AddListener(_animationNamesToCheck[i],new UnityAction(AnimationAttackEndsCallback));
		}
	}

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();
	}
	
	// Update is called once per frame
	protected override void Update ()
	{
		base.Update ();

		if(!CheckIfPossible())
			return;
		
		if(!_characterIdentity.CharacterMovement.IsRunning)
		{
			if(_isAttackingAndRunning)
				_isAttackingAndRunning = false;			
		}
		ManageAttack();
	}

	#endregion

	#region Private methods

	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_isAttackingAndRunning = false;
		_pendingAttack = false;
		_attackCounter = 0;
		_posAfterAttackRunning = Vector3.zero;
	}

	protected override bool CheckIfPossible ()
	{
		return (!_characterIdentity.CharacterHit.IsBeingHit);
	}

	private void AnimationAttackEndsCallback()	{

		if(AttackCounter == (NumMaxAttacks - 1) && ComboFinishedEvent != null)
			ComboFinishedEvent();

		if(!_characterIdentity.CharacterMovement.IsRunning)
			ManageKeepAttacking();
		else
		{
			_characterIdentity.CharacterMovement.IsRunning = false;
			AttackCounter = 0;
		}

		if(AttackFinishedEvent != null)
			AttackFinishedEvent();
	}

	private void ManageAttack()
	{		
		if(_characterIdentity.CharacterInput.KeyAttackPressed)
		{			
			if(_attackCounter < NumMaxAttacks && !_characterIdentity.CharacterMovement.IsRunning)
			{
				if(_attackCounter == 0)
				{
					AttackCounter++;
				}
				else
				{
					if(_characterIdentity.CharacterAnimation.CurrentAnimationProgress() > 0.3f)
					{
						_pendingAttack = true;
					}
				}	
			}
			else if(_characterIdentity.CharacterMovement.IsRunning && !_isAttackingAndRunning)
			{
				AttackCounter = 3;
				_isAttackingAndRunning = true;
				_posAfterAttackRunning = TransformRef.position + (Utils.Singleton.Vector2ToVector3(_characterIdentity.CharacterMovement.Movement.normalized) * 2.0f);
			}
		}
		if(_isAttackingAndRunning)
		{
			TransformRef.position = Vector3.Lerp(TransformRef.position,_posAfterAttackRunning, 0.1f);
		}
	}

	private void ManageKeepAttacking()
	{
		if(_attackCounter < NumMaxAttacks && _pendingAttack)
		{
			AttackCounter++;
			_pendingAttack = false;
		}
		else
		{				
			AttackCounter = 0;
		}
	}

	#endregion

	#region Public methods

	public void StopAttack()
	{
		AttackCounter = 0;
		_isAttackingAndRunning = false;
	}

	public void RaiseCharacterAttacksCharacterEvent(CharacterIdentity victim)
	{
		if (CharacterAttacksCharacterEvent != null)
			CharacterAttacksCharacterEvent (victim);
	}

	#endregion
}
