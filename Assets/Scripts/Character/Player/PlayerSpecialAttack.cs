using UnityEngine;
using System.Collections;

public class PlayerSpecialAttack : CharacterComponent
{
	#region Private members
	protected float _initialMetalPoints;
	protected float _currentMetalPoints;

	[Header("General params")]
	[Range(0.0f,1.0f)]
	[SerializeField] protected float _specialAttack1requirement = 0.5f;
	[SerializeField] protected float _metalPointRecoveringFactor = 1.0f;
	[SerializeField]private float _aoeDistance = 3.0f;

	protected float _specialAttack2requirement = 1.0f;

	#endregion

	#region Public members
	#endregion

	#region Properties
	public bool IsUsingSpecial1
	{
		get; set;
	}

	public bool IsUsingSpecial2
	{
		get; set;
	}

	public float CurrentMetalPoints
	{
		get { return _currentMetalPoints; }
		set {
			_currentMetalPoints = value;
			if (MetalPointsChangedEvent != null)
				MetalPointsChangedEvent (_currentMetalPoints / _initialMetalPoints);
		}
	}

	#endregion

	#region Events

	public delegate void MetalPointsChanged(float relativeMP);
	public event MetalPointsChanged MetalPointsChangedEvent;

	#endregion

	#region MonoBehaviour calls
	protected override void Awake ()
	{
		base.Awake ();
		IsUsingSpecial1 = false;
		IsUsingSpecial2 = false;
		_initialMetalPoints = _characterIdentity.CharacterStats.MetalPoints;
		_currentMetalPoints = _initialMetalPoints;
	}

	protected override void Update ()
	{
		base.Update ();

		if(!CheckIfPossible())
			return;

		if(_characterIdentity.CharacterInput.KeySpecialPressed1)
		{
			float relativeMP = _currentMetalPoints / _initialMetalPoints;
			if (relativeMP >= _specialAttack1requirement)
				PerformSpecialAttack1();
		}
		if(_characterIdentity.CharacterInput.KeySpecialPressed2)
		{
			float relativeMP = _currentMetalPoints / _initialMetalPoints;
			if (relativeMP >= _specialAttack2requirement)
				PerformSpecialAttack2();
		}

	}
	#endregion

	#region Private methods

	protected override bool CheckIfPossible ()
	{
		return (!_characterIdentity.CharacterHit.IsBeingHit && !_characterIdentity.CharacterAttack.IsAttacking 
			&& !IsUsingSpecial1 && !IsUsingSpecial2);
	}

	protected virtual void PerformSpecialAttack1()
	{
		CurrentMetalPoints -= _initialMetalPoints * _specialAttack1requirement; 
		IsUsingSpecial1 = true;
		_characterIdentity.CharacterHit.IsInvulnerable = true;
		_characterIdentity.CharacterInput.LockInput = true;
		_characterIdentity.CharacterMovement.Movement = Constants.Vector2.zero;
		_characterIdentity.CharacterAnimation.SetAnimationBool("SpecialAttack1",true);
	}

	protected virtual void SpecialAttack1Ends()
	{
		IsUsingSpecial1 = false;
		FinishSpecialAttack1Effect ();
	}

	protected virtual void StartSpecialAttack1Effect()
	{
		
	}

	protected virtual void FinishSpecialAttack1Effect()
	{

	}

	protected virtual void PerformSpecialAttack2()
	{
		CurrentMetalPoints -=_initialMetalPoints * _specialAttack2requirement; 
		IsUsingSpecial2 = true;
		_characterIdentity.CharacterHit.IsInvulnerable = true;
		_characterIdentity.CharacterInput.LockInput = true;
		_characterIdentity.CharacterMovement.Movement = Constants.Vector2.zero;
		_characterIdentity.CharacterAnimation.SetAnimationBool("SpecialAttack2",true);
	}

	protected virtual void SpecialAttack2Ends()
	{
		IsUsingSpecial2 = false;
		FinishSpecialAttack2Effect();
	}

	protected virtual void StartSpecialAttack2Effect()
	{

	}

	protected virtual void FinishSpecialAttack2Effect()
	{

	}

	#endregion

	#region Public methods

	public void RecoverMetalPoint(float multiplicator)
	{
		CurrentMetalPoints += multiplicator * _metalPointRecoveringFactor;
		if (CurrentMetalPoints >= _initialMetalPoints)
			CurrentMetalPoints = _initialMetalPoints;
	}

	public void RespawnCharacter()
	{
		CurrentMetalPoints = _initialMetalPoints;
	}

	public void SpecialAttack1AnimationFinished()
	{
		StartSpecialAttack1Effect();
	}

	public void SpecialAttack2AnimationFinished()
	{		
		StartSpecialAttack2Effect();
	}


	public void PushAwayEnemies()
	{
		for(int i = CharacterManager.Singleton.Enemies.Count - 1; i >= 0; --i)
		{
			if(Vector3.Distance(CharacterManager.Singleton.Enemies[i].TransformRef.position, TransformRef.position) < _aoeDistance)
			{
				Vector2 headingDirection = CharacterManager.Singleton.Enemies[i].TransformRef.position.x < TransformRef.position.x ? Constants.Vector2.left :
					Constants.Vector2.right;
				CharacterManager.Singleton.Enemies[i].CharacterHit.GetHit(_characterIdentity,_characterIdentity.CharacterStats.MaxAttacks,headingDirection,
					_characterIdentity.CharacterStats.AttackDamage);
			}
		}
	}

	#endregion



}
