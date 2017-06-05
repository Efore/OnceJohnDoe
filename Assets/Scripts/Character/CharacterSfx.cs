using UnityEngine;
using System.Collections;

public class CharacterSfx : CharacterComponent
{
	#region Private members
	[Header("Sfx audio data")]
	[SerializeField]
	private AudioSource _sfxAudioSource = null;

	[Header("Voice audio data")]
	[SerializeField]
	private AudioClip _smallHitVoice = null;
	[SerializeField]
	private AudioClip _bigHitVoice = null;
	[SerializeField]
	private AudioClip _deathVoice = null;
	[SerializeField]
	private AudioSource _voiceAudioSource = null;

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

		_characterIdentity.CharacterHit.CharacterGetsHitEvent += delegate(CharacterIdentity attacker, int hitNum) {		

			SFXManager.Singleton.PlayHitSound(_sfxAudioSource);

			AudioClip voice = null;

			if(_characterIdentity.CharacterHit.CurrentHealth > 0.0f)
			{	
				if(hitNum == 2)
					voice = _smallHitVoice;
				else if(hitNum == 3)
					voice = _bigHitVoice;
			}
			else
				voice = _deathVoice;
			
			if(voice != null)
				_voiceAudioSource.PlayOneShot(voice);
		};
	}

	#endregion

	#region Private methods

	private void RaiseHitTheGroundSound()
	{
		SFXManager.Singleton.PlayHitTheGroundSound (_sfxAudioSource);
	}

	#endregion

	#region Public methods

	#endregion

}