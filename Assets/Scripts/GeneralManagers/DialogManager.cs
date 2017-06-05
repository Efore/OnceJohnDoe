using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
	#region Singleton

	private static DialogManager _instance = null;

	public static DialogManager Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Private members
	[SerializeField]
	private DialogUnit _firstDialog = null;

	[Header("UI Manager references")]
	[SerializeField]
	private Image _uiDialogBackground = null;
	[SerializeField]
	private Image _uiSpeaker1Image = null;
	[SerializeField]
	private Image _uiSpeaker2Image = null;
	[SerializeField]
	private Text _uiDialogText = null;

	private DialogUnit _currentDialog = null;
	private float _timeToShowNextChar = 0.0f;
	private int _textIterator = 0;
	private char[] _textToShow = null;
	private string _currentText = "";
	private bool _skipingText = false;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	public delegate void CurrentDialogEnded(DialogUnit dialog);
	public event CurrentDialogEnded CurrentDialogEndedEvent;

	#endregion

	#region MonoBehaviour calls

	void Awake ()
	{
		_instance = this;
	}

	void Start()
	{
		if (CameraManager.Singleton != null)
		{
			CameraManager.Singleton.CameraSpecialEffect.FadeFinishedEvent += delegate() {
				ShowDialog (_firstDialog);
			};
		}
	}

	void Update()
	{
		if (_currentDialog != null && !_skipingText)
		{
			if (Input.GetKeyDown (KeyCode.Space))
				_skipingText = true;
		}
	}


	#endregion

	#region Private methods

	private IEnumerator ShowDialogCoroutine(DialogUnit dialog)
	{
		if(StageManager.Singleton != null)
			StageManager.Singleton.AudioSource.volume = 0.3f;
		if(CharacterManager.Singleton != null)
			CharacterManager.Singleton.LockPlayersInput (true);
		
		_uiDialogBackground.gameObject.SetActive (true);
		_currentDialog = dialog;

		for (_textIterator = 0; _textIterator < dialog.Texts.Length; ++_textIterator)
		{
			_skipingText = false;
			_textToShow = dialog.Texts [_textIterator].text.ToCharArray ();
			_timeToShowNextChar = Mathf.Min(0.05f,dialog.timeToShowText / _textToShow.Length);

			int speaker = dialog.Texts [_textIterator].speaker;

			_uiSpeaker1Image.sprite = dialog.playerIsSpeaker1 ? UIManager.Singleton.player1Info.CurrentFace : dialog.speaker1Sprite;
			_uiSpeaker2Image.sprite = dialog.playerIsSpeaker2 ? UIManager.Singleton.player1Info.CurrentFace : dialog.speaker2Sprite;

			if (speaker == 1)
			{
				_uiSpeaker1Image.enabled = true;
				_uiSpeaker2Image.enabled = false;
			}
			else
			{
				_uiSpeaker1Image.enabled = false;
				_uiSpeaker2Image.enabled = true;
			}

			WaitForSeconds waitForSeconds = new WaitForSeconds (_timeToShowNextChar);
			_currentText = "";
			for (int i = 0; i < _textToShow.Length && !_skipingText; ++i)
			{
				_currentText += _textToShow [i];
				_uiDialogText.text = _currentText;
				yield return waitForSeconds;
			}

			if (_skipingText)
			{
				_uiDialogText.text = new string (_textToShow);
				_skipingText = false;
				yield return new WaitForSeconds (1.0f);
			}
			else
				yield return new WaitForSeconds (dialog.timeToShowText);
		}

		if (CurrentDialogEndedEvent != null)
			CurrentDialogEndedEvent (dialog);
		
		Clean ();

		if(StageManager.Singleton != null)
			StageManager.Singleton.AudioSource.volume = 1.0f;
		if(CharacterManager.Singleton != null)
			CharacterManager.Singleton.LockPlayersInput (false);
	}

	private void Clean()
	{
		_uiDialogBackground.gameObject.SetActive (false);
		_currentDialog = null;
		_timeToShowNextChar = 0.0f;
		_textIterator = 0;
		_textToShow = null;
		_currentText = "";
		_skipingText = false;
	}

	#endregion

	#region Public methods

	public void ShowDialog(DialogUnit dialog)
	{
		if(dialog != null)
			StartCoroutine (ShowDialogCoroutine (dialog));
	}

	#endregion

}

