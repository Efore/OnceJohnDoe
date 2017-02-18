using UnityEngine;
using System.Collections;

public class CameraFadeInEffect : MonoBehaviour
{
	#region Private members

	[SerializeField]private Material _fadeInMaterial = null;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void OnRenderImage(RenderTexture srcTexture, RenderTexture destTexture)
	{
		Graphics.Blit (srcTexture, destTexture, _fadeInMaterial);
	}

	#endregion

	#region Private methods

	private IEnumerator FadeCoroutine(bool fadeIn, float fadeTime, bool muteSound)
	{
		float alpha = _fadeInMaterial.GetFloat ("_Alpha");
		float alphaIncr = 0.1f;
		WaitForSeconds waitForSeconds = new WaitForSeconds (fadeTime * alphaIncr);

		if (fadeIn)
		{	
			if(CharacterManager.Singleton != null)
				CharacterManager.Singleton.LockPlayersInput (true);
			do
			{
				alpha += alphaIncr;
				_fadeInMaterial.SetFloat("_Alpha",alpha);
				yield return waitForSeconds;
			} while(alpha < 1.0f);
		}
		else
		{
			do
			{
				alpha -= alphaIncr;
				_fadeInMaterial.SetFloat("_Alpha",alpha);				
				yield return waitForSeconds;
			} while(alpha > 0.0f);
			this.enabled = false;
			if(CharacterManager.Singleton != null)
				CharacterManager.Singleton.LockPlayersInput (false);
		}
	}

	#endregion

	#region Public methods

	public void FadeScreen(bool fadeIn, float fadeTime = 0.3f, bool muteSound = false)
	{
		StartCoroutine (FadeCoroutine(fadeIn, fadeTime, muteSound));
	}

	#endregion

}
