using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
	public enum PauseScreenOptions
	{
		RESUME = 0,
		QUIT = 1
	}

	#region Private members

	private static UIManager _instance = null;

	[SerializeField] private Transform _player1InfoPos = null;
	[SerializeField] private Transform _player2InfoPos = null;
	[SerializeField] private Transform _bossInfoPos = null;

	[Header("GameOver screen elements")]
	[SerializeField]
	private GameObject _gameOverScreen = null;

	[Header("GoText elements")]
	[SerializeField] private AudioSource _audioSource = null;
	[SerializeField] private GameObject _goText = null;
	[SerializeField] private AudioClip _goTextSound = null;

	[Header("Pause screen elements")]
	[SerializeField] private GameObject _pauseScreen = null;
	[SerializeField] private AudioClip _swichPauseOptionSound = null;

	private Outline[] _pauseScreenOptions = null;
	private int _currentPauseScreenOption = 0;

	private bool _gamePaused = false;

	#endregion

	#region Public members

	[HideInInspector] public UICharacterInfo player1Info = null;
	[HideInInspector] public UICharacterInfo player2Info = null;
	[HideInInspector] public UICharacterInfo bossInfo = null;

	#endregion

	#region Properties

	public static UIManager Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	void Awake()
	{
		_instance = this;
		_pauseScreenOptions = _pauseScreen.transform.GetComponentsInChildren<Outline> ();
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
			PauseGame ();		

		if(_gamePaused)
		{
			if(Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) )
			{
				PrevPauseOption ();
			}
			if(Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) )
			{
				NextPauseOption ();
			}
			if(Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) ||Input.GetKeyDown(KeyCode.Return) )
			{
				SelectCurrentPauseOption ();
			}
		}

	}
	#endregion

	#region Private methods

	private IEnumerator GoTextCoroutine()
	{
		int timesShowing = 3;
		while (timesShowing > 0)
		{
			_goText.SetActive (true);
			_audioSource.PlayOneShot (_goTextSound);
			yield return new WaitForSeconds (0.5f);
			_goText.SetActive (false);
			yield return new WaitForSeconds (0.75f);
			timesShowing--;
		}
	}

	private void PauseGame()
	{
		if (!_gamePaused)
		{
			_gamePaused = true;
			TogglePlayerInfo (false);
			CharacterManager.Singleton.LockEveryoneInput (true);
			Time.timeScale = 0.0f;
			_pauseScreen.SetActive (true);
			_pauseScreenOptions [_currentPauseScreenOption].enabled = true;
		}
		else
		{
			_pauseScreenOptions [_currentPauseScreenOption].enabled = false;
			_currentPauseScreenOption = 0;
			_pauseScreen.SetActive (false);
			TogglePlayerInfo (true);
			CharacterManager.Singleton.LockEveryoneInput (false);
			Time.timeScale = 1.0f;
			_gamePaused = false;
		}
	}

	private void NextPauseOption()
	{
		_audioSource.PlayOneShot (_swichPauseOptionSound);
		_pauseScreenOptions [_currentPauseScreenOption].enabled = false;
		_currentPauseScreenOption++;
		if (_currentPauseScreenOption >= _pauseScreenOptions.Length)
			_currentPauseScreenOption = 0;
		_pauseScreenOptions [_currentPauseScreenOption].enabled = true;
	}

	private void PrevPauseOption()
	{
		_audioSource.PlayOneShot (_swichPauseOptionSound);
		_pauseScreenOptions [_currentPauseScreenOption].enabled = false;
		_currentPauseScreenOption--;
		if (_currentPauseScreenOption < 0)
			_currentPauseScreenOption = _pauseScreenOptions.Length - 1;
		_pauseScreenOptions [_currentPauseScreenOption].enabled = true;
	}

	private void SelectCurrentPauseOption()
	{
		switch ((PauseScreenOptions)_currentPauseScreenOption)
		{
			case PauseScreenOptions.RESUME:
				PauseGame ();
			break;
			case PauseScreenOptions.QUIT:				
				Time.timeScale = 1.0f;
				CameraManager.Singleton.CameraSpecialEffect.FadeFinishedEvent += delegate {
					GameManager.Singleton.ResetGame();
				};
				CameraManager.Singleton.SpecialEffectFadeScreen (true, 0.5f);				
			break;
		}
	}

	private void TogglePlayerInfo(bool activate)
	{
		player1Info.gameObject.SetActive (activate);
		if(player2Info != null)
			player2Info.gameObject.SetActive (activate);
		bossInfo.gameObject.SetActive (activate);
	}
	#endregion

	#region Public methods

	public void AddPlayerInfo(int player, GameObject prefab, int initialLives)
	{
		if (player == Constants.PLAYER_ONE && player1Info == null)
		{
			player1Info = (Instantiate (prefab, Constants.Vector3.zero, Constants.Quaternion.identity) as GameObject).GetComponent<UICharacterInfo> ();
			player1Info.transform.parent = _player1InfoPos.transform.parent;
			player1Info.transform.localPosition	= _player1InfoPos.localPosition;
			player1Info.transform.localScale = Constants.Vector3.one;
			player1Info.livesText.text = "X " + initialLives;
		}
		else if (player == Constants.PLAYER_TWO && player2Info == null)
		{
			player2Info = (Instantiate (prefab, Constants.Vector3.zero, Constants.Quaternion.identity) as GameObject).GetComponent<UICharacterInfo> ();
			player2Info.transform.parent = _player2InfoPos.transform.parent;
			player2Info.transform.localPosition	= _player2InfoPos.localPosition;
			player2Info.transform.localScale = Constants.Vector3.one;
			player2Info.livesText.text = "X " + initialLives;
		}
	}

	public void AddBossInfo(GameObject prefab)
	{
		bossInfo = (Instantiate (prefab, Constants.Vector3.zero, Constants.Quaternion.identity) as GameObject).GetComponent<UICharacterInfo> ();
		bossInfo.transform.parent = _bossInfoPos.transform.parent;
		bossInfo.transform.localPosition = _bossInfoPos.localPosition;
		bossInfo.transform.localScale = Constants.Vector3.one;
	}

	public void RemoveBossInfo()
	{
		if(bossInfo != null)
			Destroy (bossInfo.gameObject);
	}

	public void RaiseGoText()
	{
		StartCoroutine (GoTextCoroutine ());
	}

	public void RaiseGameOverScreen()
	{
		TogglePlayerInfo (false);
		CharacterManager.Singleton.LockEveryoneInput (true);
		_gameOverScreen.SetActive (true);
	}

	#endregion



	
}
