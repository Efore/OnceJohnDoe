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
	private AudioSource _audioSource = null;
	[SerializeField]
	private AudioClip _changeCharacterSound = null;
	[SerializeField]
	private AudioClip _selectCharacterSound = null;

	private CharacterSelectionMember[] _characterSelectionMembers;
	private CharacterSelectionMember _currentSelectedCharacter = null;
	private int _currentSelectedCharacterIndex = 0;
	private int _currentPlayerSelecting = 0;

	private bool _charSelected = false;
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
		fadeInEffect.SpecialEffectFadeScreen (false,1.0f);
	}

	void Update()
	{
		if (_currentPlayerSelecting == Constants.PLAYER_ONE && !_charSelected)
		{
			if (Input.GetKeyDown (KeyCode.A))
				PreviousCharacter ();
			if (Input.GetKeyDown (KeyCode.D))
				NextCharacter ();
			if(Input.GetKeyDown(KeyCode.G) || Input.GetKeyDown(KeyCode.H) || Input.GetKeyDown(KeyCode.J) ||Input.GetKeyDown(KeyCode.Return) )
				SelectCharacter ();
		}
	}

	#endregion

	#region Private methods

	private void NextCharacter()
	{
		_audioSource.PlayOneShot (_changeCharacterSound);
		_currentSelectedCharacterIndex++;
		_currentSelectedCharacterIndex = _currentSelectedCharacterIndex % _characterSelectionMembers.Length;
		_currentSelectedCharacter.SetSelected (false);
		_currentSelectedCharacter = _characterSelectionMembers [_currentSelectedCharacterIndex];
		_currentSelectedCharacter.SetSelected (true);			
	}

	private void PreviousCharacter()
	{
		_audioSource.PlayOneShot (_changeCharacterSound);
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
			_charSelected = true;
			GameManager.Singleton.player1CharacterPrefab = _currentSelectedCharacter.inGamePrefab;
			CameraSpecialEffect fadeInEffect = Camera.main.GetComponent<CameraSpecialEffect> ();
			fadeInEffect.FadeFinishedEvent += delegate {
				GameManager.Singleton.LoadScene (GameManager.SceneIndexes.Level1);	
			};
			_audioSource.PlayOneShot (_selectCharacterSound);
			fadeInEffect.SpecialEffectFadeScreen (true, 0.75f);
		}
	}

	#endregion

	#region Public methods

	#endregion

}

