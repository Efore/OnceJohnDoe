using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class UIManager : MonoBehaviour
{
	#region Private members

	private static UIManager _instance = null;

	[SerializeField] private Transform _player1InfoPos = null;
	[SerializeField] private Transform _player2InfoPos = null;
	[SerializeField] private Transform _bossInfoPos = null;

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
	}
	#endregion

	#region Private methods
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

	#endregion



}
