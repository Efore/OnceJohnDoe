using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICharacterInfo : MonoBehaviour
{
	#region Private members

	[Header("Character bar data")]
	[SerializeField]private Image _characterFace = null;
	[SerializeField]private Slider _healthBar = null;
	[SerializeField]private Slider _mpBar = null;
	[SerializeField]private GameObject _mpBarParticles = null;
	[SerializeField]private Sprite _lowHealthFace = null;
	[SerializeField]private Sprite _alternativeFace = null;

	[Header("Scenes data")]
	[SerializeField]private string _victorySceneName = "";
	[SerializeField]private Texture _loadingScene = null;

	private Sprite _regularFace = null;
	private Sprite _currentFace = null;

	#endregion

	#region Public members

	public UnityEngine.UI.Text livesText = null;

	#endregion

	#region Properties

	public string VictorySceneName
	{
		get { return _victorySceneName; }
	}

	public Texture LoadingScene
	{
		get { return _loadingScene; } 
	}

	public Sprite CurrentFace
	{
		get { return _currentFace; }
	}

	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls

	void Awake()
	{
		_regularFace = _characterFace.sprite;
		_currentFace = _regularFace;
	}

	void Start()
	{
		if(_victorySceneName != "")
			GameManager.Singleton.PreloadScene (_victorySceneName);
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	public void HealthChangedCallback(float relativeHealth)
	{
		_healthBar.value = relativeHealth;

		if (_characterFace.sprite == _alternativeFace)
			return;
		
		if (relativeHealth < 0.3f)
			_currentFace = _lowHealthFace;
		else if (_currentFace != _regularFace)
			_currentFace = _regularFace;

		_characterFace.sprite = _currentFace;
	}

	public void MetalPointsChangedCallback(float relativeMP)
	{
		if (_mpBar == null)
			return;
		
		_mpBar.value = relativeMP;

		if (relativeMP == 1.0f)
			_mpBarParticles.SetActive (true);
		else
			_mpBarParticles.SetActive(false);
	}

	public void ToogleAlternativeFace(bool active)
	{
		if (active)
			_characterFace.sprite = _alternativeFace;
		else
			_characterFace.sprite = _currentFace;
	}

	#endregion



}
