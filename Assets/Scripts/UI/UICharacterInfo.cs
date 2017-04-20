using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class UICharacterInfo : MonoBehaviour
{
	#region Private members

	[SerializeField]private UnityEngine.UI.Image _characterFace = null;
	[SerializeField]private UnityEngine.UI.Slider _healthBar = null;
	[SerializeField]private UnityEngine.UI.Slider _mpBar = null;
	[SerializeField]private GameObject _mpBarParticles = null;
	[SerializeField]private Sprite _lowHealthFace = null;
	[SerializeField]private Sprite _alternativeFace = null;
	[SerializeField]private string _victorySceneName = "";

	private Sprite _regularFace = null;
	#endregion

	#region Public members

	public UnityEngine.UI.Text livesText = null;

	#endregion

	#region Properties

	public string VictorySceneName
	{
		get { return _victorySceneName; }
	}

	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls

	void Awake()
	{
		_regularFace = _characterFace.sprite;
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
			_characterFace.sprite = _lowHealthFace;
		else if (_characterFace.sprite != _regularFace)
			_characterFace.sprite = _regularFace;
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
			_characterFace.sprite = _regularFace;
	}

	#endregion



}
