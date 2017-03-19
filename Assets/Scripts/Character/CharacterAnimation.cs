using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : CharacterComponent {

	#region Private members

	private Animator _animator;

	#endregion

	#region Properties

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
	}
	#endregion

	#region Private methods 

	protected override void RestartComponent ()
	{
		base.RestartComponent ();
		_animator.Rebind ();
	}

	#endregion

	#region Public methods

	public void SetAnimationFloat(string paramName, float value)
	{
		if(_animator != null)
			_animator.SetFloat(paramName,value);
	}

	public void SetAnimationInt(string paramName, int value)
	{
		if(_animator != null)
			_animator.SetInteger(paramName,value);
	}
		
	public void SetAnimationBool(string paramName, bool value)
	{
		if(_animator != null)
			_animator.SetBool(paramName,value);
	}

	public void SetAnimationTrigger(string paramName)
	{
		if(_animator != null)
			_animator.SetTrigger (paramName);
	}

	public bool CurrentAnimationIs(string animation)
	{
		if(_animator != null)
			return _animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer." + animation);
		return false;
	}

	public float CurrentAnimationProgress()
	{	
		if(_animator != null)	
			return _animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		return 0.0f;
	}

	public void StartAtAnimation(string animation)
	{
		if(_animator != null)
			_animator.Play (animation);
	}



	#endregion

}
