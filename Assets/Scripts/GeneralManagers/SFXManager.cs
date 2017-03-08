
using UnityEngine;
using System.Collections;

public class SFXManager : MonoBehaviour
{
	#region Singleton

	private static SFXManager _instance = null;

	public static SFXManager Singleton
	{
		get { return _instance; }
	}

	#endregion

	#region Private members

	[SerializeField]
	private AudioClip[] _hitSounds = null;

	[SerializeField]
	private AudioClip[] _hitTheGroundSounds = null;

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

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	public void PlayHitSound(AudioSource source)
	{
		source.PlayOneShot(_hitSounds[Random.Range(0,_hitSounds.Length)]);
	}

	public void PlayHitTheGroundSound(AudioSource source)
	{		
		source.PlayOneShot(_hitTheGroundSounds[Random.Range(0,_hitSounds.Length)]);
	}

	#endregion

}

