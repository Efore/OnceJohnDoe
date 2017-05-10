using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	#region Private members

	[SerializeField]
	private SpriteRenderer _objectLeftRight = null;

	[SerializeField]
	private SpriteRenderer _objectRightLeft = null;

	[SerializeField]
	private Sprite[] _randomSprites = null;

	[SerializeField]
	private float _minTimeForRandomObjectSpawn = 1.0f;

	[SerializeField]
	private float _maxTimeForRandomObjectSpawn = 3.0f;

	[SerializeField]
	private AudioSource _audioSource = null;

	[SerializeField]
	private AudioClip _music = null;

	[SerializeField]
	private AudioClip _lightSfx = null;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	private void Start()
	{
		StartCoroutine (LeftRightCoroutine ());
		StartCoroutine (RightLeftCoroutine ());
	}

	private void Update()
	{
		if (Input.anyKey)
			StartCoroutine (GoToCharSelectionMenu ());
	}

	#endregion

	#region Private methods

	private IEnumerator GoToCharSelectionMenu()
	{		
		CameraSpecialEffect fadeInEffect = Camera.main.GetComponent<CameraSpecialEffect> ();
		fadeInEffect.SpecialEffectFadeScreen (true);
		yield return new WaitForSeconds (1.0f);
		GameManager.Singleton.LoadScene ("char_selection");
	}

	private IEnumerator LeftRightCoroutine()
	{
		do
		{			
			yield return new WaitForSeconds(Random.Range(_minTimeForRandomObjectSpawn,_maxTimeForRandomObjectSpawn));
			if(!_objectLeftRight.gameObject.activeInHierarchy)
			{
				_objectLeftRight.sprite = _randomSprites[Random.Range(0,_randomSprites.Length)];
				_objectLeftRight.gameObject.SetActive(true);
			}
		} while(true);
	}

	private IEnumerator RightLeftCoroutine()
	{
		do
		{
			yield return new WaitForSeconds(Random.Range(_minTimeForRandomObjectSpawn,_maxTimeForRandomObjectSpawn));
			if(!_objectRightLeft.gameObject.activeInHierarchy)
			{
				_objectRightLeft.sprite = _randomSprites[Random.Range(0,_randomSprites.Length)];
				_objectRightLeft.gameObject.SetActive(true);
			}
		} while(true);
	}

	private void PlayLightSfx()
	{
		_audioSource.PlayOneShot (_lightSfx);
	}

	private void PlayMusic()
	{
		_audioSource.clip = _music;
		_audioSource.Play ();
		_audioSource.loop = true;
	}

	#endregion

	#region Public methods

	#endregion

}
