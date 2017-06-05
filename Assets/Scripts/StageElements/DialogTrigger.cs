using UnityEngine;
using System.Collections;

public class DialogTrigger : MonoBehaviour
{
	#region Private members

	[SerializeField]
	private DialogUnit _dialog;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls


	void OnTriggerEnter()
	{
		DialogManager.Singleton.ShowDialog (_dialog);
	}


	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}
