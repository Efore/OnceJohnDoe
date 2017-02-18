using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterManager : MonoBehaviour
{
	#region Private members
	private static CharacterManager _instance = null;
	private List<PlayerIdentity> _players = new List<PlayerIdentity>();
	private List<EnemyIdentity> _enemies = new List<EnemyIdentity>();
	private BossIdentity _currentBoss;

	#endregion

	#region Public members
	#endregion

	#region Properties

	public static CharacterManager Singleton
	{
		get { return _instance; }
	}

	public List<EnemyIdentity> Enemies
	{
		get { return _enemies; }
	}

	public BossIdentity CurrentBoss
	{
		get { return _currentBoss; }
	}

	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls
	void Awake()
	{
		_instance = this;
		GameManager.Singleton.CreatePlayersCharacters ();
	}
	#endregion

	#region Private methods
	#endregion

	#region Public methods

	public void RegisterPlayer(PlayerIdentity playerCharacter, int player)
	{
		if(!_players.Contains(playerCharacter))
		{
			if (player == Constants.PLAYER_ONE)
			{
				playerCharacter.CharacterHit.HealthChangeEvent += UIManager.Singleton.player1Info.HealthChangedCallback;
				UIManager.Singleton.player1Info.HealthChangedCallback (1.0f);
				playerCharacter.PlayerSpecialAttack.MetalPointsChangedEvent += UIManager.Singleton.player1Info.MetalPointsChangedCallback;
				UIManager.Singleton.player1Info.MetalPointsChangedCallback (1.0f);
			}
			else
			{
				playerCharacter.CharacterHit.HealthChangeEvent += UIManager.Singleton.player2Info.HealthChangedCallback;
				UIManager.Singleton.player2Info.HealthChangedCallback (1.0f);
				playerCharacter.PlayerSpecialAttack.MetalPointsChangedEvent += UIManager.Singleton.player2Info.MetalPointsChangedCallback;
				UIManager.Singleton.player2Info.MetalPointsChangedCallback (1.0f);
			}
			_players.Add(playerCharacter);
		}
	}

	public void UnregisterPlayer(PlayerIdentity player)
	{
		_players.Remove(player);
	}

	public void RegisterEnemy(EnemyIdentity enemy)
	{
		if(!_enemies.Contains(enemy))
			_enemies.Add(enemy);
	}

	public void UnregisterEnemy(EnemyIdentity enemy)
	{
		_enemies.Remove(enemy);
	}

	public void RegisterBoss(BossIdentity boss)
	{
		_currentBoss = boss;
		boss.CharacterHit.HealthChangeEvent += UIManager.Singleton.bossInfo.HealthChangedCallback;
	}

	public void UnregisterBoss()
	{		
		if (_currentBoss == null)
			return;
		_currentBoss.CharacterHit.HealthChangeEvent -= UIManager.Singleton.bossInfo.HealthChangedCallback;
		_currentBoss = null;
	}

	public PlayerIdentity GetPlayer(int index)
	{
		if(_players.Count > index)
			return _players[index];
		return null;
	}

	public PlayerIdentity GetRandomPlayer()
	{
		if(_players.Count > 0)
			return GetPlayer(Random.Range(0,_players.Count));
		return null;
	}

	public EnemyIdentity GetEnemy(int index)
	{
		return _enemies[index];
	}

	public EnemyIdentity GetRandomEnemy()
	{
		return GetEnemy(Random.Range(0,_enemies.Count));
	}

	public void LockPlayersInput(bool locked)
	{
		CharacterManager.Singleton.GetPlayer (Constants.PLAYER_ONE).CharacterInput.LockInput = locked;
		if(CharacterManager.Singleton.GetPlayer (Constants.PLAYER_TWO) != null)
			CharacterManager.Singleton.GetPlayer (Constants.PLAYER_TWO).CharacterInput.LockInput = locked;
	}
	#endregion



}
