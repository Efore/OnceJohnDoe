using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]
public class PlaySoundInAnimation : MonoBehaviour
{
	#region Private members

	private AudioSource _audioSource = null;

	#endregion

	#region Public members

	#endregion

	#region Properties

	#endregion

	#region Events

	#endregion

	#region MonoBehaviour calls

	void Awake()
	{
		_audioSource = GetComponent<AudioSource> ();
	}	

	#endregion

	#region Private methods

	#endregion

	#region Public methods

	public void PlaySound(AudioClip audioClip)
	{
		_audioSource.PlayOneShot (audioClip);
	}

	#endregion

}
