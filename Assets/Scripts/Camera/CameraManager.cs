
using UnityEngine;
using System.Collections;

public class CameraManager : EnhancedMonoBehaviour
{
	#region Singleton

	private static CameraManager _instance = null;

	public static CameraManager Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Private members
	private PlayerIdentity _playerToFollow = null;
	private float _leftLimit;
	private float _rightLimit;
	private float _cameraWidth;
	private CameraBorder[] _cameraBorders;
	private Coroutine _shakeCoroutine = null;

	[Header("Shaking params")]
	[SerializeField]private float _shakeDuration = 0.5f;
	[SerializeField]private float _shakeMagnitude = 0.5f;

	[Header("Backgrounds to move with the camera")]
	[SerializeField] private GameObject [] _bgsBack;
	[SerializeField] private GameObject [] _bgsMiddle;

	[Header("Camera Effects")]
	[SerializeField] private CameraFadeInEffect _cameraFadeInEffect = null;

	#endregion

	#region Public members


	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();
		_instance = this;
		_cameraBorders = GetComponentsInChildren<CameraBorder>();

	}

	protected override void Start ()
	{
		base.Start ();
		_cameraWidth = _cameraBorders[0].TransformRef.position.x - _cameraBorders[1].TransformRef.position.x ;
		_playerToFollow = CharacterManager.Singleton.GetPlayer(Constants.PLAYER_ONE);
		_playerToFollow.CharacterHit.CharacterGetsHitEvent += RaiseShake;
		UpdateCameraLimits();
	}

	protected override void Update ()
	{
		base.Update ();
		float prevX = TransformRef.position.x;
		TransformRef.position = _playerToFollow.TransformRef.position + (Vector3.back) * 10;
		CorrectCameraPosition();

		float diffX = TransformRef.position.x - prevX;	

		for (int i = 0; i < _bgsBack.Length; ++i)
		{
			_bgsBack [i].transform.position += new Vector3 (diffX * 0.95f, 0.0f, 0.0f);
		}

		for (int i = 0; i < _bgsMiddle.Length; ++i)
		{
			_bgsMiddle [i].transform.position += new Vector3 (diffX * 0.9f, 0.0f, 0.0f);
		}
	}


	#endregion

	#region Private methods

	private	IEnumerator ShakeCoroutine() {

		float elapsed = 0.0f;

		Vector3 originalCamPos = TransformRef.position;

		while (elapsed < _shakeDuration) {

			elapsed += Time.deltaTime;          

			float percentComplete = elapsed / _shakeDuration;         
			float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

			// map value to [-1, 1]
			float x = Random.value * 2.0f - 1.0f;
			float y = Random.value * 2.0f - 1.0f;
			x *= _shakeMagnitude * damper;
			y *= _shakeMagnitude * damper;

			TransformRef.position += new Vector3(x, y, 0);

			yield return null;
		}

		TransformRef.position = originalCamPos;
	}

	private void RaiseShake(CharacterIdentity attacker, bool bigHit)
	{
		if(bigHit)
			return;
		
		if(_shakeCoroutine != null)
		{
			StopCoroutine(_shakeCoroutine);
		}
		_shakeCoroutine = StartCoroutine(ShakeCoroutine());
	}

	private void CorrectCameraPosition()
	{
		if (TransformRef.position.y > 0.6f)
			TransformRef.position = new Vector3 (TransformRef.position.x, 0.6f, TransformRef.position.z);
		if (TransformRef.position.y < -0.7f)
			TransformRef.position = new Vector3 (TransformRef.position.x, -0.7f, TransformRef.position.z);
		if (TransformRef.position.x <= _leftLimit)
			TransformRef.position = new Vector3 (_leftLimit, TransformRef.position.y, TransformRef.position.z);
		if (TransformRef.position.x >= _rightLimit)
			TransformRef.position = new Vector3 (_rightLimit, TransformRef.position.y, TransformRef.position.z);
	}

	#endregion

	#region Public methods

	public void UpdateCameraLimits()
	{
		_leftLimit = StageManager.Singleton.LeftLimit.position.x + _cameraWidth/2 - 2 ;
		_rightLimit = StageManager.Singleton.RightLimit.position.x - _cameraWidth/2 + 2;
	}

	public void FadeScreen(bool fadeIn, float fadeTime = 0.3f, bool muteSound = false)
	{
		_cameraFadeInEffect.enabled = true;
		_cameraFadeInEffect.FadeScreen (fadeIn, fadeTime, muteSound);
	}

	#endregion

}

