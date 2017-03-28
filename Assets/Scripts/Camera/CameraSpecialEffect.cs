using UnityEngine;
using System.Collections;

public class CameraSpecialEffect : MonoBehaviour
{
	#region Private members
	private const float MAX_WAVE_INCR = Mathf.PI / 2.0f;

	[Header("Camera Shake params")]
	[SerializeField]private float _cameraShakeRatio = 0.1f;
	[SerializeField]private float _shakingTime = 0.3f;

	[Header("Full Screen Shaders")]
	[SerializeField]private Material _fadeInMaterial = null;
	[SerializeField]private Material _wavedMaterial = null;

	private Material _inUseMaterial = null;

	private Vector3 _preShakeCameraPos;

	#endregion

	#region Public members



	#endregion

	#region Properties

	public bool IsShaking
	{
		get;
		set;
	}

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake()
	{
		_inUseMaterial = _fadeInMaterial;
	}

	void OnRenderImage(RenderTexture srcTexture, RenderTexture destTexture)
	{		
		Graphics.Blit (srcTexture, destTexture, _inUseMaterial);
	}

	#endregion

	#region Private methods

	private IEnumerator SpecialEffectShakeCoroutine(Transform cameraTrans, float shakeTime)
	{		
		IsShaking = true;
		float goalTime = Time.realtimeSinceStartup + shakeTime;

		while (Time.realtimeSinceStartup < goalTime)
		{
			cameraTrans.position = _preShakeCameraPos + new Vector3 (Random.Range (-_cameraShakeRatio, _cameraShakeRatio),
				Random.Range (-_cameraShakeRatio, _cameraShakeRatio), Random.Range (-_cameraShakeRatio, _cameraShakeRatio));
			yield return new WaitForEndOfFrame ();
		}

		cameraTrans.transform.position = _preShakeCameraPos;
		IsShaking = false;
	}

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
				AudioListener.volume += alphaIncr;
				alpha += alphaIncr;
				_fadeInMaterial.SetFloat("_Alpha",alpha);
				yield return waitForSeconds;
			} while(alpha < 1.0f);
		}
		else
		{
			do
			{
				if(muteSound)					
					AudioListener.volume -= alphaIncr;
				alpha -= alphaIncr;
				_fadeInMaterial.SetFloat("_Alpha",alpha);				
				yield return waitForSeconds;
			} while(alpha > 0.0f);
			this.enabled = false;
			if(CharacterManager.Singleton != null)
				CharacterManager.Singleton.LockPlayersInput (false);
		}
	}

	private IEnumerator WaveCoroutine()
	{
		float waveIncrease = 0.01f;
		WaitForSeconds waitForSeconds = new WaitForSeconds (waveIncrease);
		float waveIndex = 0.0f;
		do
		{	
			_wavedMaterial.SetFloat("_WaveIndex",waveIndex);
			waveIndex += waveIncrease;
			yield return waitForSeconds;
		} while(waveIndex < MAX_WAVE_INCR);

		this.enabled = false;
		_wavedMaterial.SetFloat("_WaveIndex",0.0f);
	}

	#endregion

	#region Public methods

	public void SpecialEffectShake(Transform cameraTrans,float shakeTime = 0.3f)
	{
		_preShakeCameraPos = cameraTrans.position;
		StartCoroutine (SpecialEffectShakeCoroutine (cameraTrans, _shakingTime));
	}

	public void SpecialEffectFadeScreen(bool fadeIn, float fadeTime = 0.3f, bool muteSound = true)
	{		
		_inUseMaterial = _fadeInMaterial;
		this.enabled = true;
		StartCoroutine (FadeCoroutine(fadeIn, fadeTime, muteSound));
	}

	public void SpecialEffectWave(float xScreenPos)
	{
		_inUseMaterial = _wavedMaterial;
		_inUseMaterial.SetFloat ("_XPosOrigin", xScreenPos);
		this.enabled = true;
		StartCoroutine (WaveCoroutine ());
	}
	 
	#endregion

}
