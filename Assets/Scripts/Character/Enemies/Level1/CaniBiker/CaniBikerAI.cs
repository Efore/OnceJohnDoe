using UnityEngine;
using System.Collections;

public class CaniBikerAI : EnemyAI
{
	#region Private members
	private const float SECS_TO_CHARGE_AGAIN = 2.0f;

	[SerializeField]
	private GameObject _caniPrefab;
	[SerializeField]
	private Transform _caniGenerationPosition;

	//private CaniAI _caniToGenerate = null;
	private Vector2 _chargingDirection = Constants.Vector2.zero;
	private Vector3 _posStopFalling = Vector3.zero;

	private CameraBorder _lastCameraBorder = null;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	protected override void Start ()
	{
		base.Start ();
		_characterIdentity.CharacterHit.CharacterGetsHitEvent += StartFalling;
		TransformRef.position = new Vector3 (TransformRef.position.x, Target.TransformRef.position.y, TransformRef.position.z);
		StartCoroutine(ChooseTarget (false));
	}

	protected override void Update ()
	{	
		if (_posStopFalling != Vector3.zero)
		{
			TransformRef.position = Vector3.Lerp(TransformRef.position,_posStopFalling, 0.1f);
		}

		if (_chargingDirection == Constants.Vector2.right) {
			_enemyInput.GoRight = true;
			_enemyInput.GoLeft = false;
		} else if (_chargingDirection == Constants.Vector2.left) {
			_enemyInput.GoLeft = true;
			_enemyInput.GoRight = false;
		} else {
			_enemyInput.GoLeft = false;
			_enemyInput.GoRight = false;
		}
			
	}

	#endregion

	#region Private methods

	private void PrepareCani()
	{		
		Quaternion rot = _chargingDirection ==Constants.Vector2.right ? Quaternion.identity : Quaternion.Euler (0.0f, 180.0f, 0.0f);
		Instantiate (_caniPrefab, _caniGenerationPosition.position, rot);			
	}

	private IEnumerator ChooseTarget(bool wait = true)
	{
		float secs = wait ? SECS_TO_CHARGE_AGAIN : 0.01f;
		yield return new WaitForSeconds (secs);

		Target = CharacterManager.Singleton.GetRandomPlayer();

		_chargingDirection = Constants.Vector2.left;

		if (TransformRef.position.x < Target.TransformRef.position.x)
			_chargingDirection = Constants.Vector2.right;


		TransformRef.position = new Vector3 (TransformRef.position.x + _chargingDirection.x * 3, Target.TransformRef.position.y, TransformRef.position.z);

	}

	private void StartFalling(CharacterIdentity attacker, bool bigHit)
	{
		_posStopFalling = TransformRef.position + (Utils.Singleton.Vector2ToVector3(_characterIdentity.CharacterMovement.Movement.normalized) * 4);
		_characterIdentity.CharacterHit.enabled = false;
		ObjectPoolManager.Singleton.Instantiate(FxManager.Singleton.fxPrefabHitGround,
			TransformRef.position,Constants.Quaternion.identity);
	}

	#endregion

	#region Public methods

	public void CameraLimitReached(CameraBorder cameraBorder)
	{	
		if (_lastCameraBorder == null)
		{
			_lastCameraBorder = cameraBorder;
		}	
		else if (_lastCameraBorder != cameraBorder)
		{			
			_lastCameraBorder = cameraBorder;
			StartCoroutine (ChooseTarget ());
		}

	}

	#endregion

}
