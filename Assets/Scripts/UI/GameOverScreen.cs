using UnityEngine;
using System.Collections;

public class GameOverScreen : MonoBehaviour
{
	#region Private members

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
		CameraManager.Singleton.CameraSpecialEffect.FadeFinishedEvent += delegate {
			GameManager.Singleton.ResetGame();
		};
	}

	#endregion

	#region Private methods

	private void GameOverScreenLoaded()
	{
		CameraManager.Singleton.SpecialEffectFadeScreen (true, 1.0f);
	}

	#endregion

	#region Public methods

	#endregion

}
