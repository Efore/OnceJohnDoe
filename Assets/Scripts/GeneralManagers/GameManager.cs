﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public enum SceneIndexes
	{
		Null = -1,
		InitialScene = 0,
		Intro = 1,
		CharSelection = 2,
		Level1 = 3,
		VictoryChopo = 4,
		VictoryDani = 5
	}

	#region Singleton

	private static GameManager _instance = null;

	public static GameManager Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Private members

	private PlayerIdentity _player1Character = null;
	private PlayerIdentity _player2Character = null;

	private AsyncOperation _asyncOp = null;

	private List<string> _scenesPreloaded =  new List<string>();

	#endregion

	#region Public members

	public GameObject player1CharacterPrefab = null;
	public GameObject player2CharacterPrefab = null;

	public int player1lives = 0;
	public int player2lives = 0;

	#endregion

	#region Properties

	public PlayerIdentity Player1Character 
	{
		get { return _player1Character; }
	}

	public PlayerIdentity Player2Character 
	{
		get { return _player2Character; }
	}

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake ()
	{
		_instance = this;
		DontDestroyOnLoad (this.gameObject);
		ResetGame ();
	}

	#endregion

	#region Private methods

	private IEnumerator PreloadSceneCoroutine(SceneIndexes sceneIndex)
	{	
		_asyncOp = SceneManager.LoadSceneAsync ((int)sceneIndex);
		_asyncOp.allowSceneActivation = false;
		yield return _asyncOp;
	}

	private void ManagePlayer1Lives()
	{
		player1lives--;
		_player1Character.CharacterAnimation.SetAnimationInt ("Lives", player1lives);
		UIManager.Singleton.player1Info.livesText.text = "X " + player1lives;

		if (player1lives > 0)
		{
			Instantiate (FxManager.Singleton.fxSpawningThunder, _player1Character.TransformRef.position, Quaternion.identity);

			_player1Character.PlayerSpecialAttack.PushAwayEnemies ();
			_player1Character.CharacterHit.RespawnCharacter ();
			_player1Character.PlayerSpecialAttack.RespawnCharacter ();
		}
		else
		{
			if (player2lives == 0)
			{
				//Endgame
				Application.Quit();
			}
		}
	}

	private void ManagePlayer2Lives()
	{
		player2lives--;
		_player2Character.CharacterAnimation.SetAnimationInt ("Lives", player2lives);
		UIManager.Singleton.player2Info.livesText.text = "X " + player2lives;
		if (player2lives > 0)
		{
			Instantiate (FxManager.Singleton.fxSpawningThunder, _player2Character.TransformRef.position, Quaternion.identity);

			_player2Character.PlayerSpecialAttack.PushAwayEnemies ();
			_player2Character.CharacterHit.RespawnCharacter ();
			_player2Character.PlayerSpecialAttack.RespawnCharacter ();
		}
		else
		{
			if (player1lives == 0)
			{
				//Endgame
				Application.Quit ();
			}
		}
	}

	#endregion

	#region Public methods

	public void CreatePlayersCharacters()
	{
		if (player1CharacterPrefab != null)
		{
			_player1Character = (Instantiate (player1CharacterPrefab, StageManager.Singleton.player1SpawnPos.position, Quaternion.identity) as GameObject)
				.GetComponent<PlayerIdentity> ();

			_player1Character.CharacterHit.CharacterDefeatedEvent += ManagePlayer1Lives;
			player1lives = _player1Character.CharacterStats.InitialLives;
			_player1Character.CharacterAnimation.SetAnimationInt ("Lives", player1lives);

			(_player1Character.CharacterInput as PlayerInput).player = Constants.PLAYER_ONE;

			UIManager.Singleton.AddPlayerInfo (Constants.PLAYER_ONE, _player1Character.CharacterUiInfo, player1lives);
			CharacterManager.Singleton.RegisterPlayer(_player1Character,Constants.PLAYER_ONE);
		}
		if (player2CharacterPrefab != null)
		{
			_player2Character = (Instantiate (player2CharacterPrefab, StageManager.Singleton.player2SpawnPos.position, Quaternion.identity) as GameObject)
				.GetComponent<PlayerIdentity> ();

			_player2Character.CharacterHit.CharacterDefeatedEvent += ManagePlayer2Lives;
			player2lives = _player2Character.CharacterStats.InitialLives;
			_player2Character.CharacterAnimation.SetAnimationInt ("Lives", player2lives);

			(_player2Character.CharacterInput as PlayerInput).player = Constants.PLAYER_TWO;

			UIManager.Singleton.AddPlayerInfo (Constants.PLAYER_TWO, _player2Character.CharacterUiInfo, player2lives);
			CharacterManager.Singleton.RegisterPlayer(_player2Character,Constants.PLAYER_TWO);
		}
	}

	public void LoadScene(SceneIndexes sceneIndex)
	{
		SceneManager.LoadScene ((int)sceneIndex);
	}

	public void PreloadScene(SceneIndexes sceneIndex)
	{		
		StartCoroutine (PreloadSceneCoroutine (sceneIndex));
	}

	public void ResetGame()
	{
		LoadScene (SceneIndexes.Intro);
	}

	#endregion

}

