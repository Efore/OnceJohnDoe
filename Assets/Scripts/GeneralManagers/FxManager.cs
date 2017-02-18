using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FxManager : MonoBehaviour
{
	#region Private members
	private static FxManager _instance = null;

	public GameObject fxPrefabHitCharacter = null;
	public GameObject fxPrefabHitCharacterText = null;
	public GameObject fxPrefabHitGround = null;
	public GameObject fxPrefabBlood = null;
	public GameObject fxSpawningThunder = null;

	#endregion

	#region Public members
	#endregion

	#region Properties
	public static FxManager Singleton
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

	#endregion



}

