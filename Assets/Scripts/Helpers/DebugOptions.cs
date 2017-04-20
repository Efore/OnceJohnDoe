using UnityEngine;
using System.Collections;

public class DebugOptions : MonoBehaviour
{
	#region Singleton

	private static DebugOptions _instance = null;

	public static DebugOptions Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Private members

	[SerializeField]
	private Transform _startBossPosition = null;

	[SerializeField]
	private KeyCode _keyForMovingToBossPosition;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake ()
	{
		_instance = this;
	}

	void Update()
	{
		if (!Debug.isDebugBuild)
			return;
		
		if (Input.GetKeyDown (_keyForMovingToBossPosition))
			CharacterManager.Singleton.GetPlayer (0).gameObject.transform.position = _startBossPosition.position;
	}
	#endregion

	#region Private methods

	#endregion

	#region Public methods

	#endregion

}

