using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemiesTrigger : EnhancedMonoBehaviour
{
	[System.Serializable]
	public class Wave
	{
		public int knapsackSize;
		public StageManager.EnemyTier enemyTier;

		[HideInInspector]
		public List<StageManager.EnemyToSpawn> potentialEnemiesToSpawn = new List<StageManager.EnemyToSpawn>();
		[HideInInspector]
		public List<GameObject> enemiesToSpawn = new List<GameObject>();

		public int enemiesToKill;

	}

	#region Private members


	[SerializeField]private Transform _leftLimitPosition = null;
	[SerializeField]private Transform _rightLimitPosition = null;

	[SerializeField]private Transform[] _spawnPoints = null;
	[SerializeField]private Wave[] _waves = null;

	private int _currentWave = 0;

	#endregion

	#region Public members
	#endregion

	#region Properties
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();
		FillAllWaves();
	}

	void OnTriggerEnter(Collider other)
	{
		StageManager.Singleton.SetStageLimits(_leftLimitPosition.position.x, _rightLimitPosition.position.x);
		GetComponent<BoxCollider>().enabled = false;
		if(_waves.Length > 0)
			StartCoroutine(SpawnEnemies(_waves[_currentWave]));
	}

	#endregion

	#region Private methods

	private IEnumerator SpawnEnemies(Wave wave, float secsBeforeSpawning = 0.0f)
	{
		yield return new WaitForSeconds (secsBeforeSpawning); 
		for(int i = 0; i < wave.enemiesToKill; ++i)
		{
			GameObject enemy = GameObject.Instantiate(wave.enemiesToSpawn[i],
				_spawnPoints[i % _spawnPoints.Length].position, Quaternion.identity) as GameObject;
			
			enemy.GetComponent<CharacterHit>().CharacterDiesEvent += EnemyKilled;

			if (enemy.GetComponent<EnemyTameable> () != null)
				enemy.GetComponent<EnemyTameable> ().EnemyTamedEvent += EnemyTamed;
			
			yield return new WaitForSeconds(0.1f);
		}
	}

	private void FillAllWaves()
	{
		foreach(Wave wave in _waves)
		{
			FillPotentialEnemiesToSpawnList(wave);
			FillEnemyBag(wave);
		}
	}

	private void EnemyTamed(EnemyIdentity enemy)
	{
		enemy.GetComponent<CharacterHit> ().CharacterDiesEvent -= EnemyKilled;
		EnemyKilled ();
	}

	private void EnemyKilled ()
	{
		_waves[_currentWave].enemiesToKill --;

		if(_waves[_currentWave].enemiesToKill == 0)
		{
			if(_waves.Length > _currentWave + 1)
			{
				_currentWave ++;
				StartCoroutine(SpawnEnemies(_waves[_currentWave],1.0f));
			}
			else 
			{
				FightOver();
			}
		}
	}

	private void FightOver()
	{
		StageManager.Singleton.SetOriginalRightLimit();
		//gameObject.SetActive (false);
	}

	private void FillPotentialEnemiesToSpawnList(Wave wave)
	{
		switch(wave.enemyTier)
		{
			case StageManager.EnemyTier.ONE:
				foreach(StageManager.EnemyToSpawn enemy in StageManager.Singleton.enemiesToSpawnT1)
				{
					if(!wave.potentialEnemiesToSpawn.Contains(enemy))
					{
						wave.potentialEnemiesToSpawn.Add(enemy);
					}
				}
				break;

			case StageManager.EnemyTier.TWO:
				foreach(StageManager.EnemyToSpawn enemy in StageManager.Singleton.enemiesToSpawnT2)
				{
					if(!wave.potentialEnemiesToSpawn.Contains(enemy))
					{
						wave.potentialEnemiesToSpawn.Add(enemy);
					}
				}
				break;

			case StageManager.EnemyTier.THREE:
				foreach(StageManager.EnemyToSpawn enemy in StageManager.Singleton.enemiesToSpawnT3)
				{
					if(!wave.potentialEnemiesToSpawn.Contains(enemy))
					{
						wave.potentialEnemiesToSpawn.Add(enemy);
					}
				}
				break;
		}
	}

	private void FillEnemyBag(Wave wave)
	{
		foreach(StageManager.EnemyToSpawn enemy in wave.potentialEnemiesToSpawn)
		{
			if(enemy.knapsackValue < wave.knapsackSize)
			{
				wave.enemiesToSpawn.Add(enemy.enemyPrefab);
				wave.knapsackSize -= enemy.knapsackValue;
			}
			else
				break;
		}
		while (wave.knapsackSize > 0)
		{
			int random = Random.Range(0,wave.potentialEnemiesToSpawn.Count);
			bool fittable = false;
			do
			{
				if(wave.potentialEnemiesToSpawn[random].knapsackValue > wave.knapsackSize)
					random--;
				else
					fittable = true;
			}while(!fittable);
			wave.enemiesToSpawn.Add(wave.potentialEnemiesToSpawn[random].enemyPrefab);
			wave.knapsackSize -= wave.potentialEnemiesToSpawn[random].knapsackValue;
		}
		wave.enemiesToKill = wave.enemiesToSpawn.Count;
	}

	#endregion

	#region Public methods
	#endregion



}
