using UnityEngine;
using System.Collections;

public class VictoryScreen : MonoBehaviour
{
	#region Private members

	[SerializeField]
	private DialogUnit _dialogUnit = null;

	[SerializeField]
	private CameraSpecialEffect _cameraSpecialEffect = null;

	[SerializeField]
	private float _fadeInTime = 4.0f;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake()
	{
		DialogManager.Singleton.CurrentDialogEndedEvent += delegate(DialogUnit dialog) {
			if(dialog == _dialogUnit)
				_cameraSpecialEffect.FadeInEffect(_fadeInTime);
		};
		_cameraSpecialEffect.FadeFinishedEvent += delegate() {
			if(GameManager.Singleton != null)
				GameManager.Singleton.LoadScene(GameManager.SceneIndexes.Credits);
		};
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}
