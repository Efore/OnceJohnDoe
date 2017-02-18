using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DialogUnit : MonoBehaviour
{
	#region Private members

	private DialogText[] _texts;

	#endregion

	#region Public members

	public Sprite backgroundSprite = null;
	public Sprite speaker1Sprite = null;
	public Sprite speaker2Sprite = null;
	public float timeToShowText = 3.0f;

	#endregion

	#region Properties

	public DialogText[] Texts
	{
		get { return _texts; }
	}

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake()
	{
		_texts = transform.GetComponentsInChildren<DialogText> ();
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}	