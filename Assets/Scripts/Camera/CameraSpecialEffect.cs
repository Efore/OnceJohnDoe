using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraSpecialEffect : MonoBehaviour
{	
	#region Private members
	private const float MAX_WAVE_INCR = Mathf.PI / 2.0f;
	private const float SMALL_HIT_WAVE_SCALE = 2f;
	private const float BIG_HIT_WAVE_SCALE = 3f;

	[Header("Camera Shake params")]
	[SerializeField]private float _cameraShakeRatio = 0.1f;
	[SerializeField]private float _shakingTime = 0.3f;

	[Header("Wave at Hit params")]
	[SerializeField]
	private GameObject _hitWaveRingPrefab = null;
	[SerializeField]
	private Camera _waveAtHitCamera = null;

	[Header("Full Screen Shaders")]
	[SerializeField]private bool _useLoadingScreen = false;
	[SerializeField]private bool _startInBlack = false;
	[SerializeField]private Material _fadeInMaterial = null;
	[SerializeField]private Material _wavedScreenMaterial = null;
	[SerializeField]private Material _waveAtHitMaterial = null;
	[SerializeField]private Material _loadingMat = null;

	private Vector3 _preShakeCameraPos;

	private List<HitWaveBehaviour> _hitWaveRings = new List<HitWaveBehaviour>();

	private Material _inUseMaterial = null;
	private Material _fadeInMaterialCopy = null;
	private Material _wavedScreenMaterialCopy = null;
	private Material _waveAtHitMaterialCopy = null;
	private Material _loadingMatCopy = null;

	private bool _useOnRenderImage = false;
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

	public delegate void FadeFinished();
	public event FadeFinished FadeFinishedEvent;

	#endregion

	#region MonoBehaviour calls

	void Awake()
	{	
		_fadeInMaterialCopy = _fadeInMaterial != null ? new Material (_fadeInMaterial) : null;
		_waveAtHitMaterialCopy = _waveAtHitMaterial != null ? new Material (_waveAtHitMaterial) : null;
		_wavedScreenMaterialCopy = _wavedScreenMaterial != null ? new Material (_wavedScreenMaterial) : null;
		_loadingMatCopy = _loadingMat != null ? new Material (_loadingMat) : null;

		if (_startInBlack)
		{			
			_useOnRenderImage = true;
			_fadeInMaterialCopy.SetFloat ("_Alpha", 1.0f);
			AudioListener.volume = 0.0f;
			_inUseMaterial = _fadeInMaterialCopy;
		}
		else
		{
			AudioListener.volume = 1.0f;
		}

		if (_useLoadingScreen)
		{			
			_useOnRenderImage = true;
			_inUseMaterial = _loadingMatCopy;
			Texture loadingScene = GameManager.Singleton.player1CharacterPrefab.GetComponent<PlayerIdentity> ().CharacterUiInfo.GetComponent<UICharacterInfo> ().LoadingScene;
			_inUseMaterial.SetTexture ("_BlendingTex", loadingScene);
		}
	}

	void OnRenderImage(RenderTexture srcTexture, RenderTexture destTexture)
	{	
		if(_useOnRenderImage)	
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
		float alpha = _inUseMaterial.GetFloat ("_Alpha");
		float alphaIncr = Time.deltaTime / fadeTime;
		WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();

		if (fadeIn)
		{	
			_useOnRenderImage = true;
			do
			{				
				AudioListener.volume -= alphaIncr;
				if(AudioListener.volume < 0.0f)
					AudioListener.volume = 0.0f;
				alpha += alphaIncr;
				if(alpha > 1.0f)
					alpha = 1.0f;
				_inUseMaterial.SetFloat("_Alpha",alpha);
				yield return waitForFrame;
			} while(alpha < 1.0f);
		}
		else
		{
			do
			{
				if(muteSound)					
				{
					AudioListener.volume += alphaIncr;
					if(AudioListener.volume > 1.0f)
						AudioListener.volume = 1.0f;
				}
				alpha -= alphaIncr;
				if(alpha < 0.0f)
					alpha = 0.0f;
				_inUseMaterial.SetFloat("_Alpha",alpha);				
				yield return waitForFrame;
			} while(alpha > 0.0f);
			_useOnRenderImage = false;
		}

		if (FadeFinishedEvent != null)
			FadeFinishedEvent ();
	}

	private IEnumerator WaveCoroutine()
	{
		float waveIncrease = 0.01f;
		WaitForSeconds waitForSeconds = new WaitForSeconds (waveIncrease);
		float waveIndex = 0.0f;
		_useOnRenderImage = true;

		do
		{	
			_inUseMaterial.SetFloat("_WaveIndex",waveIndex);
			waveIndex += waveIncrease;
			yield return waitForSeconds;
		} while(waveIndex < MAX_WAVE_INCR);

		_inUseMaterial.SetFloat("_WaveIndex",0.0f);
		_useOnRenderImage = false;
	}

	#endregion

	#region Public methods

	public void FadeInEffect(float fadeTime)
	{
		SpecialEffectFadeScreen (true, fadeTime);
	}

	public void SpecialEffectShake(Transform cameraTrans,float shakeTime = 0.3f)
	{
		_preShakeCameraPos = cameraTrans.position;
		StartCoroutine (SpecialEffectShakeCoroutine (cameraTrans, _shakingTime));
	}

	public void SpecialEffectFadeScreen(bool fadeIn, float fadeTime = 1.0f, bool muteSound = true)
	{		
		_inUseMaterial = _fadeInMaterialCopy;
		StartCoroutine (FadeCoroutine(fadeIn, fadeTime, muteSound));
	}

	public void SpecialEffectWave(float xScreenPos)
	{
		_inUseMaterial = _wavedScreenMaterialCopy;
		_inUseMaterial.SetFloat ("_XPosOrigin", xScreenPos);
		StartCoroutine (WaveCoroutine ());
	}

	public void SpecialEffectHitWave(Vector3 position, bool bigHit)
	{
		_useOnRenderImage = true;
		if(!_waveAtHitCamera.enabled)
		{
			_waveAtHitCamera.enabled = true;
			_waveAtHitCamera.Render ();				
		}

		HitWaveBehaviour newHitWave = ObjectPoolManager.Singleton.Instantiate (_hitWaveRingPrefab, position, Constants.Quaternion.identity).GetComponent<HitWaveBehaviour> ();
		newHitWave.WaveFinishedEvent += delegate {
			_hitWaveRings.Remove(newHitWave);
			newHitWave.gameObject.SetActive(false);
			if(_hitWaveRings.Count == 0)
			{
				_useOnRenderImage = false;
				_waveAtHitCamera.enabled = false;
			}
		};

		newHitWave.MaxScale = bigHit ? BIG_HIT_WAVE_SCALE : SMALL_HIT_WAVE_SCALE;
		_hitWaveRings.Add (newHitWave);
		_inUseMaterial = _waveAtHitMaterialCopy;
	}
	 
	#endregion

}
