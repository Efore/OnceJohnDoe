using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : CharacterComponent {

	#region Private members

	private Animator _animator;
	private string _currentAnimationName = "Idle";
	private AnimationEventDictionary _animationStartingEvents = new AnimationEventDictionary();
	private AnimationEventDictionary _animationEndingEvents = new AnimationEventDictionary();
	private bool _animationStartingTriggered = false;
	private bool _animationEndingTriggered = false;

	private string[] _animationNamesToCheck = new string[]{"Idle","Walk","Run","Attack1","Attack2",
		"Attack3","RunAttack","Hit1Front","Hit2Front","Hit3Front", 
		"Hit1Back","Hit2Back","Hit3Back", "StandUpBack", "StandUpFront","EndRolling"};

	#endregion

	#region Properties

	public AnimationEventDictionary AnimationStartEvents
	{
		get { return _animationStartingEvents; }
	}

	public AnimationEventDictionary AnimationEndsEvents
	{
		get { return _animationEndingEvents; }
	}

	#endregion

	#region MonoBehaviour calls 

	protected override void Awake ()
	{
		base.Awake ();
		_animator = GetComponent<Animator>();
	}

	protected override void Update ()
	{
		base.Update ();

		if(!CurrentAnimationIs(_currentAnimationName))
		{
			_animationEndingTriggered = false;
			_animationStartingTriggered = false;
			_currentAnimationName = "";
		}

		for(int i = 0; i < _animationNamesToCheck.Length; ++i)
		{
			if(CurrentAnimationIs(_animationNamesToCheck[i]))
			{
				if(_currentAnimationName != _animationNamesToCheck[i])
				{
					_currentAnimationName = _animationNamesToCheck[i];
				}	
				break;
			}
		}

		if(CurrentAnimationProgress() < 0.15 && !_animationStartingTriggered)
		{
			_animationStartingEvents.TriggerAnimationEvent(_currentAnimationName);
			_animationStartingTriggered = true;
		}
		else if(CurrentAnimationProgress() > 0.85 && !_animationEndingTriggered)
		{
			_animationEndingEvents.TriggerAnimationEvent(_currentAnimationName);	
			_animationEndingTriggered = true;
		}
	}
	#endregion

	#region Private methods 

	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_animator.Rebind ();
		_currentAnimationName = "Idle";
		_animationStartingEvents = new AnimationEventDictionary();
		_animationEndingEvents = new AnimationEventDictionary();
		_animationStartingTriggered = false;
		_animationEndingTriggered = false;
	}

	#endregion

	#region Public methods

	public void SetAnimationFloat(string paramName, float value)
	{
		_animator.SetFloat(paramName,value);
	}

	public void SetAnimationInt(string paramName, int value)
	{
		_animator.SetInteger(paramName,value);
	}
		
	public void SetAnimationBool(string paramName, bool value)
	{
		_animator.SetBool(paramName,value);
	}

	public void SetAnimationTrigger(string paramName)
	{
		_animator.SetTrigger (paramName);
	}

	public bool CurrentAnimationIs(string animation)
	{
		return _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer." + animation);
	}

	public float CurrentAnimationProgress()
	{		
		return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
	}

	public void StartAtAnimation(string animation)
	{
		_animator.Play (animation);
	}



	#endregion

}
