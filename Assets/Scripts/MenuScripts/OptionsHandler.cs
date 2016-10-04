using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class OptionsHandler : MonoBehaviour {
	AudioHandler audioHandler;
	LanguageHandler languageHandler;
	private Language currentLanguage;
	private PlayerControlType currentPlayerControlType;
	private RecordReplaySettingType currentRecordReplayType;
	public Text languageLabel, musicVolumeLabel, audioVolumeLabel, controlLabel, recordReplayLabel, optionsTitleLabel, 
		cancelLabel, saveExitLabel, signInOutLabel, resetStatsLabel;
	public Text languageInfoText, musicVolumeLevelText, audioVolumeLevelText, controlInfoText, replayInfoText;
	public Slider musicVolumeSlider, audioVolumeSlider;
	public Button but_ControlLeft, but_ControlRight, but_ReplayLeft, but_ReplayRight, but_LanguageLeft, but_LanguageRight;
	// Use this for initialization
	void Start () {
		audioHandler = (AudioHandler)GameObject.FindGameObjectWithTag ("MusicPlayer").GetComponent (typeof(AudioHandler));
		languageHandler = (LanguageHandler)GameObject.FindGameObjectWithTag ("LanguageHandler").GetComponent (typeof(LanguageHandler));
		currentLanguage = languageHandler.GetCurrentLanguage ();
		musicVolumeSlider.normalizedValue = PlayerPrefs.GetFloat (GloVar.MusicVolumePrefName, 1f);
		audioVolumeSlider.normalizedValue = PlayerPrefs.GetFloat (GloVar.AudioVolumePrefName, 1f);
		currentPlayerControlType = (PlayerControlType)PlayerPrefs.GetInt (GloVar.PlayerControlChoicePrefName, 0);
		currentRecordReplayType = (RecordReplaySettingType)PlayerPrefs.GetInt (GloVar.RecordReplayChoicePrefName, 0);
		SetLabels ();
	}

	void GetFromPrefs(){
		currentLanguage = languageHandler.GetCurrentLanguage ();
		musicVolumeSlider.normalizedValue = PlayerPrefs.GetFloat (GloVar.MusicVolumePrefName, 1f);
		audioVolumeSlider.normalizedValue = PlayerPrefs.GetFloat (GloVar.AudioVolumePrefName, 1f);
		currentPlayerControlType = (PlayerControlType)PlayerPrefs.GetInt (GloVar.PlayerControlChoicePrefName, 0);
		currentRecordReplayType = (RecordReplaySettingType)PlayerPrefs.GetInt (GloVar.RecordReplayChoicePrefName, 0);
	}
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene(GloVar.MainMenuSceneName);; }
	}
		

	void SetLabels(){
		// TITLE & BUTTONS
		languageHandler.SetTextSettings (optionsTitleLabel, "options", FontSize.Large);
		languageHandler.SetTextSettings (cancelLabel, "cancel", FontSize.Medium);
		languageHandler.SetTextSettings (saveExitLabel, "saveexit", FontSize.Medium);
		languageHandler.SetTextSettings (resetStatsLabel, "resetstats", FontSize.Medium);
		if (PlayGamesPlatform.Instance.IsAuthenticated () == true){
			languageHandler.SetTextSettings(signInOutLabel, "signout", FontSize.Medium);
		}
		else{
			languageHandler.SetTextSettings(signInOutLabel, "signin", FontSize.Medium);
		}

		// LANGUAGE
		languageHandler.SetTextSettings (languageLabel, "language", FontSize.Medium);
		languageInfoText.text = currentLanguage.GetLanguageName();
		languageInfoText.font = languageHandler.GetFont ();
		languageInfoText.fontSize = languageHandler.GetFontSize (FontSize.Medium);
		//but_LanguageLeft.gameObject.SetActive ((int)currentLanguage > 0);
		//but_LanguageRight.gameObject.SetActive ((int)currentLanguage < Enum.GetNames (typeof(Language)).Length - 1);

		// MUSIC & AUDIO
		languageHandler.SetTextSettings (musicVolumeLabel, "musicvolume", FontSize.Medium);
		if (musicVolumeSlider.normalizedValue ==0){
			languageHandler.SetTextSettings (musicVolumeLevelText, "off", FontSize.Medium);
		}
		else{
			languageHandler.SetTextSettings (musicVolumeLevelText, "", FontSize.Medium);
			musicVolumeLevelText.text = "" + (int)(musicVolumeSlider.normalizedValue * 100);
		}
		languageHandler.SetTextSettings (audioVolumeLabel, "audiovolume", FontSize.Medium);
		if (audioVolumeSlider.normalizedValue ==0){
			languageHandler.SetTextSettings (audioVolumeLevelText, "off", FontSize.Medium);
		}
		else{
			languageHandler.SetTextSettings (audioVolumeLevelText, "", FontSize.Medium);
			audioVolumeLevelText.text = "" + (int)(audioVolumeSlider.normalizedValue * 100);
		}

		// CONTROL
		languageHandler.SetTextSettings (controlLabel, "control", FontSize.Medium);
		languageHandler.SetTextSettings (controlInfoText, PlayerControlTypeExtension.ToLangKey (currentPlayerControlType), FontSize.Medium);
		but_ControlLeft.gameObject.SetActive ((int)currentPlayerControlType > 0);
		but_ControlRight.gameObject.SetActive ((int)currentPlayerControlType<Enum.GetNames (typeof(PlayerControlType)).Length-1); 

		// RECORD REPLAY
		languageHandler.SetTextSettings (recordReplayLabel, "recording", FontSize.Medium);
		languageHandler.SetTextSettings (replayInfoText, currentRecordReplayType.ToLangKey (), FontSize.Medium);
		but_ReplayLeft.gameObject.SetActive ((int)currentRecordReplayType > 0);
		but_ReplayRight.gameObject.SetActive ((int)currentRecordReplayType < Enum.GetNames (typeof(RecordReplaySettingType)).Length - 1);
	}
	public void ClickLanguageLeft(){
		if ((int)currentLanguage==0){
			currentLanguage = (Language)(Enum.GetNames (typeof(Language)).Length - 1);
		}
		else{
			currentLanguage = (Language)((int)currentLanguage - 1);
		}
		languageHandler.SetLanguage (currentLanguage);
		SetLabels ();
	}
	public void ClickLanguageRight(){
		if ((int)currentLanguage < (Enum.GetNames (typeof(Language)).Length - 1)){
			currentLanguage = (Language)((int)currentLanguage + 1);
		}
		else{
			currentLanguage = (Language)0;
		}
		languageHandler.SetLanguage (currentLanguage);
		SetLabels ();
	}
	public void MusicSliderValueChanged(){
		audioHandler.SetVolume (musicVolumeSlider.normalizedValue);
		SetLabels ();
	}
	public void AudioSliderValueChanged(){
		SetLabels ();
	}
	public void ClickControlLeft(){
		currentPlayerControlType = (PlayerControlType)((int)currentPlayerControlType - 1);
		SetLabels ();
	}
	public void ClickControlRight(){
		currentPlayerControlType = (PlayerControlType)((int)currentPlayerControlType + 1);
		SetLabels ();
	}
	public void ClickRecordReplayLeft(){
		currentRecordReplayType = (RecordReplaySettingType)((int)currentRecordReplayType - 1);
		SetLabels ();
	}
	public void ClickRecordReplayRight(){
		currentRecordReplayType = (RecordReplaySettingType)((int)currentRecordReplayType + 1);
		SetLabels ();
	}

	public void ClickSaveExit(){
		PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)currentLanguage);
		PlayerPrefs.SetInt (GloVar.PlayerControlChoicePrefName, (int)currentPlayerControlType);
		PlayerPrefs.SetInt (GloVar.RecordReplayChoicePrefName, (int)currentRecordReplayType);
		PlayerPrefs.SetFloat (GloVar.MusicVolumePrefName, musicVolumeSlider.normalizedValue);
		PlayerPrefs.SetFloat (GloVar.AudioVolumePrefName, audioVolumeSlider.normalizedValue);
		SceneManager.LoadScene (GloVar.MainMenuSceneName);
	}
	public void ClickCancel(){
		languageHandler.SetLanguage ((Language)PlayerPrefs.GetInt(GloVar.LanguagePrefName, 0));
		SceneManager.LoadScene (GloVar.MainMenuSceneName);
	}
	public void ClickResetStats(){
		PlayerPrefs.DeleteAll();
		languageHandler.SetLanguage ((Language)PlayerPrefs.GetInt(GloVar.LanguagePrefName, 0));
		GetFromPrefs ();

		SetLabels ();
	}
	public void ClickSignInOut(){
		if (PlayGamesPlatform.Instance.IsAuthenticated () == true) {
			if (Social.Active.localUser.authenticated == true){
				PlayGamesPlatform.Instance.SignOut();
				PlayerPrefs.SetInt (GloVar.AutoSignInPrefName, 0);
			}
			else{
				Social.Active.localUser.Authenticate((bool success) =>{
					if (success){
						SetLabels ();
						Debug.Log("user authenticated");
					}
					else
						Debug.Log("authentication failed");
				});
			}

		}
		else{
			PlayGamesPlatform.Activate ();
			PlayerPrefs.SetInt (GloVar.AutoSignInPrefName, 1);
			Social.Active.localUser.Authenticate((bool success) =>{
				if (success){
					PlayerPrefs.SetInt (GloVar.AutoSignInPrefName, 1);
					SetLabels ();
					Debug.Log("user authenticated");
				}
				else
					Debug.Log("authentication failed");
			});
		}
		SetLabels ();
	}
}