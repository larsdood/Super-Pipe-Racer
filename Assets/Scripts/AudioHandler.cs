using UnityEngine;
using System.Collections;

public class AudioHandler : MonoBehaviour {
	public AudioSource audioSource;
	public AudioClip mainMenuTheme, normalModeTheme;
	// Use this for initialization
	void Awake () {
		GameObject[] musicPlayers = GameObject.FindGameObjectsWithTag ("MusicPlayer");
		if (musicPlayers [0] != this.gameObject)
			Destroy (this.gameObject);
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	private void Play(int songID){
		
	}
	public void PlayNormalMode(){
		if (audioSource.isPlaying && audioSource.clip == normalModeTheme){
			return;
		}
		audioSource.volume = PlayerPrefs.GetFloat (GloVar.MusicVolumePrefName, 1f);
		if (audioSource.volume!=0f){
			audioSource.clip = normalModeTheme;
			audioSource.Play ();
		}
	}
	public void PlayMainMenu(){
		if (audioSource.isPlaying && audioSource.clip == mainMenuTheme){
			return;
		} 
		audioSource.volume = PlayerPrefs.GetFloat (GloVar.MusicVolumePrefName, 1f);
		if (audioSource.volume!=0f){
			audioSource.clip = mainMenuTheme;
			audioSource.Play ();
		}
	}
	public void SetVolume(float volume){
		audioSource.volume = volume;
	}
	public void PauseMusic(){
		audioSource.Pause ();
	}
	public void UnpauseMusic(){
		audioSource.UnPause ();
	}
}
