using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;

public class AnimationEventDictionary : Dictionary<string,UnityEvent> {

	#region Private members

	private Dictionary<string,UnityEvent> _events = new Dictionary<string, UnityEvent>();

	#endregion

	#region Public methods

	public void AddListener(string eventName, UnityAction listener)
	{
		UnityEvent thisEvent = null;
		if(_events.TryGetValue(eventName,out thisEvent))
			thisEvent.AddListener(listener);
		else
		{
			thisEvent = new UnityEvent();
			thisEvent.AddListener(listener);
			_events.Add(eventName,thisEvent);
		}
	}

	public void RemoveListener(string eventName, UnityAction listener)
	{
		if(_events.Count == 0)
			return;
		UnityEvent thisEvent = null;
		if(_events.TryGetValue(eventName,out thisEvent))
			thisEvent.RemoveListener(listener);
	}

	public void TriggerAnimationEvent(string eventName)
	{
		UnityEvent thisEvent = null;
		if(_events.TryGetValue(eventName,out thisEvent))
			thisEvent.Invoke();
	}

	#endregion
}
