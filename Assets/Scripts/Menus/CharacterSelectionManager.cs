using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CharacterSelectionManager : MonoBehaviour
{
	#region Singleton

	private static CharacterSelectionManager _instance = null;

	public static CharacterSelectionManager Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Private members

	[SerializeField]
	private GameObject _loadingScreenGameObject = null;

	private CharacterSelectionMember[] _characterSelectionMembers;
	private CharacterSelectionMember _currentSelectedCharacter = null;
	private int _currentSelectedCharacterIndex = 0;
	private int _currentPlayerSelecting = 0;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake ()
	{
		_instance = this;
		_characterSelectionMembers = GetComponentsInChildren<CharacterSelectionMember> ();
		_currentSelectedCharacter = _characterSelectionMembers [_currentSelectedCharacterIndex];
		for (int i = 1; i < _characterSelectionMembers.Length; ++i)
			_characterSelectionMembers [i].SetSelected (false);			
	}

	void Start()
	{
		CameraSpecialEffect fadeInEffect = Camera.main.GetComponent<CameraSpecialEffect> ();
		fadeInEffect.SpecialEffectFadeScreen (false);
	}

	void Update()
	{
		if (_currentPlayerSelecting == Constants.PLAYER_ONE)
		{
			if (Input.GetKeyDown (KeyCode.A))
				PreviousCharacter ();
			if (Input.GetKeyDown (KeyCode.D))
				NextCharacter ();
			if (Input.GetKeyDown (KeyCode.G))
				SelectCharacter ();
		}
	}

	#endregion

	#region Private methods

	private void NextCharacter()
	{
		_currentSelectedCharacterIndex++;
		_currentSelectedCharacterIndex = _currentSelectedCharacterIndex % _characterSelectionMembers.Length;
		_currentSelectedCharacter.SetSelected (false);
		_currentSelectedCharacter = _characterSelectionMembers [_currentSelectedCharacterIndex];
		_currentSelectedCharacter.SetSelected (true);			
	}

	private void PreviousCharacter()
	{
		_currentSelectedCharacterIndex--;

		if (_currentSelectedCharacterIndex < 0)
			_currentSelectedCharacterIndex = _characterSelectionMembers.Length - 1;
		
		_currentSelectedCharacter.SetSelected  (false);
		_currentSelectedCharacter = _characterSelectionMembers [_currentSelectedCharacterIndex];
		_currentSelectedCharacter.SetSelected (true);
	}

	private void SelectCharacter()
	{
		if (_currentSelectedCharacter.inGamePrefab != null)
		{
			GameManager.Singleton.player1CharacterPrefab = _currentSelectedCharacter.inGamePrefab;
			CameraSpecialEffect fadeInEffect = Camera.main.GetComponent<CameraSpecialEffect> ();
			fadeInEffect.FadeFinishedEvent += delegate {
//				_loadingScreenGameObject.SetActive(true);
//				fadeInEffect.enabled = false;
				GameManager.Singleton.LoadScene ("level1");	
			};
			fadeInEffect.SpecialEffectFadeScreen (true);
		}
		else
			Debug.Log ("Character locked");
	}

	#endregion

	#region Public methods

	#endregion

}

