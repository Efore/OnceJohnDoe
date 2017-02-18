using UnityEngine.Events;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChoniAI : EnemyAI
{
	#region Private members
	[SerializeField]
	protected float _delayToAttack = 0.5f;

	[SerializeField]
	protected ChoniThreatDetector _threatDetector;

	[SerializeField]
	protected List<GameObject> _projectilePrefabs;

	[SerializeField]
	protected Transform _positionToSpawnProjectile;

	protected Projectile _projectileThrown = null;

	private GameObject _handBag = null;
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
		_characterIdentity.CharacterAnimation.AnimationEndsEvents.AddListener("Attack1",new UnityAction(AnimationAttackEndsCallback));
		_handBag = new GameObject (this.name + "Handbag");
	}

	protected override void Update ()
	{
		if(_threatDetector.IsInDanger)
			return;		
		base.Update ();
	}

	protected override void OnDisable ()
	{
		base.OnDisable ();
		//Destroy (_handBag);
	}

	#endregion

	#region Private methods

	private void AnimationAttackEndsCallback()	
	{
		if(ReadyToAttack)
		{
			//CancelProjectileThrowingAnimation ();
			ReadyToAttack = false;
		}
	}

	private void CancelProjectileThrowingAnimation()
	{
		_characterIdentity.CharacterAnimation.SetAnimationInt("ProjectileType",-1);
	}

	private void ActiveProjectile()
	{
		if (_projectileThrown == null)
			return;
		_projectileThrown.gameObject.SetActive(true);
		_projectileThrown = null;
	}

	protected override IEnumerator StartAttackCoroutine()
	{
		_readyToAttack = true;
		int randProjectile = Random.Range(0,_projectilePrefabs.Count);
		_projectileThrown = ObjectPoolManager.Singleton.Instantiate(_projectilePrefabs[randProjectile],
			_positionToSpawnProjectile.position,Quaternion.identity,_handBag.transform).GetComponent<Projectile>();		
		_projectileThrown.gameObject.SetActive (false);
		_projectileThrown.ProjectileDirection = _characterIdentity.CharacterMovement.HeadingDirection;
		_projectileThrown.owner = this._characterIdentity;
		_characterIdentity.CharacterAnimation.SetAnimationInt("ProjectileType",randProjectile);

		do{			
			yield return new WaitForSeconds(_delayToAttack);
		}while(_targetInRangeOfAttack != null && _targetInRangeOfAttack.CharacterHit.IsBeingHit);

		if(_targetInRangeOfAttack == null)
		{
			ReadyToAttack = false;
			CancelProjectileThrowingAnimation ();
			_characterIdentity.CharacterAttack.StopAttack();
			yield break;
		}

		_enemyInput.Attack = true;	
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		_enemyInput.Attack = false;

	}

	#endregion

	#region Public methods
	public override void SetTargetInRangeOfAttack (CharacterIdentity targetToAttack)
	{
		base.SetTargetInRangeOfAttack (targetToAttack);
		if(_targetInRangeOfAttack == null)
		{
			CancelProjectileThrowingAnimation ();
		}
	}
	#endregion



}
