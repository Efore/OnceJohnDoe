using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
	public enum EnemyTier { ONE, TWO, THREE};
	[System.Serializable]
	public struct EnemyToSpawn
	{
		public GameObject enemyPrefab;
		public int knapsackValue;
	}

	#region Singleton

	private static StageManager _instance = null;

	public static StageManager Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Private members

	[SerializeField]
	private Transform _topLimit = null;
	[SerializeField]
	private Transform _bottomLimit = null;
	[SerializeField]
	private Transform _leftLimit = null;
	[SerializeField]
	private Transform _rightLimit = null;
	[SerializeField]
	private AudioSource _audioSource = null;

	public Transform player1SpawnPos = null;
	public Transform player2SpawnPos = null;

	[Header("Tier 1")]
	public EnemyToSpawn[] enemiesToSpawnT1;

	[Header("Tier 2")]
	public EnemyToSpawn[] enemiesToSpawnT2;

	[Header("Tier 3")]
	public EnemyToSpawn[] enemiesToSpawnT3;

	private float _initialRightLimit;
	private float _initialLeftLimit;
	private float _stageHeight;
	private float _stageDepth;

	#endregion

	#region Public members

	#endregion

	#region Properties

	public Transform LeftLimit
	{
		get{ return _leftLimit; }
	}

	public Transform RightLimit
	{
		get{ return _rightLimit; }
	}

	public AudioSource AudioSource
	{
		get { return _audioSource; }
	}

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake ()
	{
		_instance = this;
		_stageHeight = _topLimit.position.y - _bottomLimit.position.y;
		_stageDepth = _topLimit.position.z - _bottomLimit.position.z;
		_initialLeftLimit = _leftLimit.position.x;
		_initialRightLimit = _rightLimit.position.x;
	}

	void Start()
	{
		SetOriginalStageLimits();
	}

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	public void SetOriginalStageLimits()
	{		
		SetOriginalLeftLimit();
		SetOriginalRightLimit();
	}

	public void SetOriginalLeftLimit()
	{
		SetStageLimits(_initialLeftLimit,_rightLimit.position.x);
	}

	public void SetOriginalRightLimit()
	{
		SetStageLimits(_leftLimit.position.x,_initialRightLimit);
	}

	public void SetStageLimits(float leftLimit, float rightLimit)
	{
		_leftLimit.position = new Vector3(leftLimit,_leftLimit.position.y,_leftLimit.position.z);
		_rightLimit.position = new Vector3(rightLimit,_rightLimit.position.y,_rightLimit.position.z);
		CameraManager.Singleton.UpdateCameraLimits();
	}

	public Vector3 Get3DMovement(Vector3 oldPosition, Vector2 movement)
	{
		oldPosition += Utils.Singleton.Vector2ToVector3 (movement);
		return Get3DPosition (oldPosition);
	}

	public Vector3 Get3DPosition(Vector3 position)
	{
		float difY = _topLimit.position.y - position.y;
		float difZ = (difY * _stageDepth) / _stageHeight;
		float newZ = _topLimit.position.z - difZ;

		return new Vector3 (position.x, position.y, newZ);
	}

	#endregion

}

