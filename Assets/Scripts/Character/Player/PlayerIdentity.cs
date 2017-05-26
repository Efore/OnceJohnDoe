using UnityEngine;
using System.Collections;

public class  PlayerIdentity : CharacterIdentity
{
	#region Private members

	#endregion

	#region Public members
	public GameObject CharacterUiInfo = null;
	public PlayerSpecialAttack PlayerSpecialAttack = null;
	public int playerOwner = 0;
	#endregion

	#region Properties
	#endregion

	#region Events
	#endregion

	#region MonoBehaviour calls

	protected override void Awake ()
	{
		base.Awake ();
	}

	protected override void Start ()
	{
		base.Start ();
		StartCoroutine (FadeOutCoroutine ());
	}

	protected override void OnEnable()
	{
		base.OnDisable();

	}

	protected override void OnDisable()
	{
		base.OnDisable();
		CharacterManager.Singleton.UnregisterPlayer(this);
	}

	#endregion

	#region Private methods
	private IEnumerator FadeOutCoroutine()
	{	
		if (CharacterManager.Singleton != null)
			CharacterManager.Singleton.LockPlayersInput (true);
		yield return new WaitForSeconds(8.0f);
		CameraManager.Singleton.SpecialEffectFadeScreen(false);
	}
	#endregion

	#region Public methods
	#endregion



}
