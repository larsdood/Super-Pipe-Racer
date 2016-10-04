using UnityEngine;
using System.Collections;

public class SFXHandler : MonoBehaviour {

	public AudioSource mainAudioSource, turningAudioSource;

	float volume;

	public AudioClip clip_Crash, clip_Section, clip_Highscore, clip_Start, clip_Turning;

	void Awake(){
		volume = PlayerPrefs.GetFloat (GloVar.AudioVolumePrefName, 1f);
		turningAudioSource.volume = 0;
	}

	public void Mute(bool toggle){
		if (toggle) {
			volume = 0;
			mainAudioSource.Pause ();
			turningAudioSource.Pause ();
		}
		else{
			volume = PlayerPrefs.GetFloat (GloVar.AudioVolumePrefName, 1f);
			mainAudioSource.UnPause ();
			turningAudioSource.UnPause ();
		}
	}

	public void PlayCrashSFX(){
		mainAudioSource.PlayOneShot (clip_Crash, volume);
	}
	public void PlaySectionSFX(){
		mainAudioSource.PlayOneShot (clip_Section, volume);
	}
	public void PlayHighscoreSFX(){
		mainAudioSource.PlayOneShot(clip_Highscore, volume);
	}
	public void PlayStartSFX(){
		mainAudioSource.PlayOneShot (clip_Start, volume);
	}

	public void PlayLeftTurningSFX(){
		if (turningAudioSource.volume < 0.5f){
			turningAudioSource.volume += 0.2f;
		}
		if (turningAudioSource.panStereo >= -0.6f)
			turningAudioSource.panStereo -= 0.05f;
		if (!turningAudioSource.isPlaying){
			turningAudioSource.PlayOneShot (clip_Turning, volume);
		}
	}
	public void PlayRightTurningSFX(){
		if (turningAudioSource.volume < 0.5f){
			turningAudioSource.volume += 0.2f;
		}
		if (turningAudioSource.panStereo <= 0.6f)
			turningAudioSource.panStereo += 0.05f;
		if (!turningAudioSource.isPlaying){
			turningAudioSource.PlayOneShot (clip_Turning, volume);
		}
	}
	public void PlayMiddleTurningSFX(){
		if (turningAudioSource.volume < 0.3f){
			turningAudioSource.volume += 0.01f;
		}
		else if (turningAudioSource.volume > 0.3f){
			turningAudioSource.volume -= 0.1f;
		}
		if (turningAudioSource.panStereo > 0f)
			turningAudioSource.panStereo -= 0.025f;
		else if (turningAudioSource.panStereo < 0f)
			turningAudioSource.panStereo += 0.025f;
		if (!turningAudioSource.isPlaying){
			turningAudioSource.PlayOneShot (clip_Turning, volume);
		}
	}
	public void StopTurningSFX(){
		if (turningAudioSource.volume == 0){
			if (turningAudioSource.isPlaying)
				turningAudioSource.Stop ();
		}
		else{
			turningAudioSource.volume -= 0.05f;
		}

	}

}
