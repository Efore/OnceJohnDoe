
using UnityEngine;
using System.Collections;

public class CaniAI : EnemyAI
{
	#region Private members
	[SerializeField]
	protected float _maxDelayToAttack = 1.5f;
	[SerializeField]
	protected bool _comesFromCaniBiker = false;

	protected float _delayToAttack = 0.0f;
	protected float _attackWithComboRNG = 1.0f;
	protected bool _attackWithCombo = false;


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
		_characterIdentity.CharacterAttack.ComboFinishedEvent += HandleComboFinishedEvent;
		if (_comesFromCaniBiker)
		{
			if (TransformRef.rotation != Quaternion.identity)
			{
				TransformRef.rotation = Quaternion.identity;
				_characterIdentity.CharacterMovement.HeadingDirection =Constants.Vector2.left;
			}
			StartInTheGround ();
		}
	}


	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_attackWithComboRNG = 1.0f;
		_attackWithCombo = false;
	}

	#endregion

	#region Private methods

	protected override IEnumerator StartAttackCoroutine()
	{				
		_readyToAttack = true;
		_attackWithCombo = Random.Range(0.0f,1.0f) > _attackWithComboRNG;

		if(_attackWithCombo)
			_attackWithComboRNG = 1.0f;
		else
			_attackWithComboRNG -= 0.1f;

		do{			
			_delayToAttack = Random.Range(_maxDelayToAttack * 0.5f,_maxDelayToAttack);
			yield return new WaitForSeconds(_delayToAttack);
		}while(_targetInRangeOfAttack != null && _targetInRangeOfAttack.CharacterHit.IsBeingHit);

		if(_targetInRangeOfAttack == null)
		{
			_characterIdentity.CharacterAttack.StopAttack();
			ReadyToAttack = false;
			yield break;
		}

		_enemyInput.Attack = true;	
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();

		if(!_attackWithCombo)
		{
			ReadyToAttack = false;
		}
	}

	protected void HandleComboFinishedEvent()
	{
		_enemyInput.Attack = false;
		_readyToAttack = false;
	}

	private void StartInTheGround()
	{
		_characterIdentity.CharacterHit.IsBeingHit = true;
		_characterIdentity.CharacterAnimation.SetAnimationBool ("StartInGround", true);
	}

	#endregion

	#region Public methods



	#endregion



}
