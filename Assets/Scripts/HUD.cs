using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class HUD : MonoBehaviour {

	LanguageHandler languageHandler;

	// Use this for initialization
	public bool moveLeft = false, moveRight = false;
	public Player player;
	private string highscoreName, scoreName, speedName, sectionName;
	private int arrowCounter = 0;
	private int recordingErrorCounter = 0;
	public Text scoreLabel, speedLabel, sectionLabel, highscoreLabel, sectionReachedLabel, newHighscoreLabel, leftArrowLabel, rightArrowLabel, unlockTextLabel,
		recordingInfoText, livesLabel;
	public Text leaderboardTextLabel, pauseTextLabel, watchReplayLabel, mainMenuLabel, restartLabel;
	public Button leaderboardButton, restartButton, mainMenuButton, pauseButton, watchReplayButton, moveLeftButton, moveRightButton;

	void Awake(){
		languageHandler = (LanguageHandler)GameObject.FindGameObjectWithTag ("LanguageHandler").GetComponent (typeof(LanguageHandler));
		languageHandler.SetTextSettings (sectionReachedLabel, "ready", FontSize.Large);
		languageHandler.SetTextSettings (pauseTextLabel, "pause", FontSize.Large);
		languageHandler.SetTextSettings (watchReplayLabel, "watchreplay", FontSize.Medium);
		languageHandler.SetTextSettings (mainMenuLabel, "mainmenu", FontSize.Medium);
		languageHandler.SetTextSettings (restartLabel, "restart", FontSize.Medium);
		languageHandler.SetTextSettings (leaderboardTextLabel, "leaderboard", FontSize.Medium);

		languageHandler.SetTextSettings (highscoreLabel, "highscore", FontSize.Medium);
		highscoreName = highscoreLabel.text;
		languageHandler.SetTextSettings (scoreLabel, "score", FontSize.Medium);
		scoreName = scoreLabel.text;

		languageHandler.SetTextSettings (speedLabel, "speed", FontSize.Medium);
		speedName = speedLabel.text;

		languageHandler.SetTextSettings (sectionLabel, "section", FontSize.Medium);
		sectionName = sectionLabel.text;
	}

	public LanguageHandler GetLanguageHandler(){
		return languageHandler;
	}

	public void SetValues(float score, float velocity, int section){
		SetValues (0, score, velocity, section);
	}

	public void SetValues(float highscore, float score, float velocity, int section){

		highscoreLabel.text = highscoreName + ": " + (int)highscore;
		scoreLabel.text = scoreName + ": " + (int)score;
		speedLabel.text = speedName +  ": " + (int)(velocity);
		sectionLabel.text = sectionName + ": " + section;

	}

	public void SectionReached(int section){
		languageHandler.SetTextSettings (sectionReachedLabel, "section", FontSize.Large);
		sectionReachedLabel.text += " " +section;
		sectionReachedLabel.CrossFadeAlpha (1, 0, true);
		sectionReachedLabel.CrossFadeAlpha (0, 5, false);
	}
	public void SectionClear(){
		sectionReachedLabel.CrossFadeAlpha (0, 0.5f, false);
	}

	public void NewHighscoreReached(){
		languageHandler.SetTextSettings (newHighscoreLabel, "newhighscore", FontSize.Medium);
		newHighscoreLabel.CrossFadeAlpha (1, 0, true);
		newHighscoreLabel.CrossFadeAlpha (0, 4, false);
	}
	public void FinalScoreDisplay(int score, bool isHighscore){
		newHighscoreLabel.CrossFadeAlpha (1, 0, true);
		if (isHighscore){
			languageHandler.SetTextSettings (newHighscoreLabel, "newhighscore", FontSize.Medium);
			newHighscoreLabel.text += ": " + score;
		}
		else{
			languageHandler.SetTextSettings (newHighscoreLabel, "score", FontSize.Medium);
			newHighscoreLabel.text += ": " + score;
		}
	}
	public void ShowMenu(){
		moveLeftButton.gameObject.SetActive (false);
		moveRightButton.gameObject.SetActive (false);
		restartButton.gameObject.SetActive (true);
		mainMenuButton.gameObject.SetActive (true);
		if (Application.isMobilePlatform && (RecordReplaySettingType)PlayerPrefs.GetInt(GloVar.RecordReplayChoicePrefName, 0)!=RecordReplaySettingType.Disabled) {
			watchReplayButton.gameObject.SetActive (true);
		}
		if (PlayGamesPlatform.Instance.IsAuthenticated () == true) {
			if (Social.Active.localUser.authenticated == true) {
				leaderboardButton.gameObject.SetActive (true);
			}
		}

	}
	public void ResetArrowCounter(){
		arrowCounter = 0;
	}
	public void ShowArrows(){
		PlayerControlType controlType = (PlayerControlType)PlayerPrefs.GetInt (GloVar.PlayerControlChoicePrefName);
		if (controlType != PlayerControlType.TwoHanded){
			moveLeftButton.gameObject.SetActive (true);
			moveRightButton.gameObject.SetActive (true);
			if (controlType == PlayerControlType.OneHandedRight){
				moveLeftButton.transform.position = new Vector3 (Screen.width*8f/10f, Screen.height*1f/8f);
				moveRightButton.transform.position = new Vector3(Screen.width*9f/10f, Screen.height*1f/8f);
			}
			else if (controlType == PlayerControlType.OneHandedLeft){
				moveLeftButton.transform.position = new Vector3 (Screen.width/10f, Screen.height*1f/8f);
				moveRightButton.transform.position = new Vector3(Screen.width*2f/10f, Screen.height*1f/8f);
			}
		}
		else{
			moveLeftButton.gameObject.SetActive (false);
			moveRightButton.gameObject.SetActive (false);
			if (Application.isMobilePlatform){
				leftArrowLabel.gameObject.SetActive (true);
				rightArrowLabel.gameObject.SetActive (true);
				//leftArrowLabel.text = "<";
				//rightArrowLabel.text = ">";
				arrowCounter++;
				Invoke ("RemoveArrows", 0.5f);
				if (arrowCounter<4){
					Invoke ("ShowArrows", 1);
				}
				//leftArrowLabel.CrossFadeAlpha (0, 4, false);
				//rightArrowLabel.CrossFadeAlpha (0, 4, false);
			}
		}
	}
	public void RemoveArrows(){
		leftArrowLabel.gameObject.SetActive (false);
		rightArrowLabel.gameObject.SetActive (false);
	}
	public void ShowUnlockText(string text){
		languageHandler.SetTextSettings (unlockTextLabel, "placeholder", FontSize.Medium);
		unlockTextLabel.text = text;

	}
	public void PauseButtonClick(){
		player.Pause ();
	}
	public void SetPauseButtonActive(bool enable){
		pauseButton.gameObject.SetActive (enable);
	}
	public void CantRecord(){
		languageHandler.SetTextSettings (recordingInfoText, "recordingfailed", FontSize.Medium);
		recordingInfoText.CrossFadeAlpha (0, 4f, false);
	}
	public void CanRecord(){
		languageHandler.SetTextSettings (recordingInfoText, "recordingok", FontSize.Medium);
		recordingInfoText.CrossFadeAlpha (0, 4f, false);
	}
	public void MoveLeftButtonDown(){
		moveLeft = true;
	}
	public void MoveLeftButtonUp(){
		moveLeft=false;
	}
	public void MoveRightButtonDown(){
		moveRight = true;
	}
	public void MoveRightButtonUp(){
		moveRight = false;
	}
	public float GetMoveButtonsDirection(){
		if (moveLeft && !moveRight) {
			return -1f;
		} else if (!moveLeft && moveRight) {
			return 1f;
		} else {
			return 0f;
		}
	}
	public void ScorePosted(bool success){
		recordingInfoText.CrossFadeAlpha (1, 0f, false);
		if (success){
			languageHandler.SetTextSettings (recordingInfoText, "scoreposted", FontSize.Medium);
		}
		else{
			languageHandler.SetTextSettings (recordingInfoText, "scorepostfailed", FontSize.Medium);
		}
	}
	public void SetLives(int lives){
		languageHandler.SetTextSettings (livesLabel, "lives", FontSize.Medium);
		livesLabel.text += ": " + lives;
	}

	public void RestartClick(){
		if (PlayerPrefs.GetInt (GloVar.GamePurchasedPrefName, 0) == 1) {
			SceneManager.LoadScene (GloVar.MainGameSceneName);
		}
		else{
			SceneManager.LoadScene (GloVar.PreGameSceneName);
		}
	}
	public void MainMenuClick(){
		SceneManager.LoadScene(GloVar.MainMenuSceneName);
	}
	public void WatchReplayClick(){
		ReplayHandler.WatchReplay ();
	}
	public void LeaderboardClick(){
		switch(player.GetGameMode()){
		case GameMode.Normal_SlowStart:
		case GameMode.Normal_FastStart:
			PlayGamesPlatform.Instance.ShowLeaderboardUI (GloVar.GPGSLeaderboardNormal);
			break;
		case GameMode.ThreeStrikes:
			PlayGamesPlatform.Instance.ShowLeaderboardUI (GloVar.GPGSLeaderboardThreeStrikes);
			break;
		case GameMode.Spiral:
			PlayGamesPlatform.Instance.ShowLeaderboardUI (GloVar.GPGSLeaderboardSpiral);
			break;
		case GameMode.Slalom:
			PlayGamesPlatform.Instance.ShowLeaderboardUI (GloVar.GPGSLeaderboardSlalom);
			break;
		case GameMode.HyperSpeed:
			PlayGamesPlatform.Instance.ShowLeaderboardUI (GloVar.GPGSLeaderboardHyperspeed);
			break;
		}
		return;
	}
}
