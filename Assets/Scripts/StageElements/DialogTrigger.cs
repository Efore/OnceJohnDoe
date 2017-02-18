using UnityEngine;
using System.Collections;

public class DialogTrigger : MonoBehaviour
{
	#region Private members

	[SerializeField]
	private DialogUnit[] _dialog;

	private int _currentDialog = 0;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void OnEnable()
	{
		DialogManager.Singleton.CurrentDialogEndedEvent += CurrentDialogEndedCallback;
	}

	void OnTriggerEnter()
	{
		DialogManager.Singleton.ShowDialog (_dialog[_currentDialog]);
	}

	void OnDisable()
	{
		DialogManager.Singleton.CurrentDialogEndedEvent -= CurrentDialogEndedCallback;
	}

	#endregion

	#region Private methods

	private void CurrentDialogEndedCallback (DialogUnit dialog)
	{
		_currentDialog++;
		if (_currentDialog < _dialog.Length)
			DialogManager.Singleton.ShowDialog (_dialog [_currentDialog]);
	}

	#endregion

	#region Public methods

	#endregion

}
