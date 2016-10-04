using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using UnityEngine.Purchasing;

public class MainMenuHandler : MonoBehaviour, IStoreListener{
	int credits;
	AudioHandler audioHandler;
	LanguageHandler languageHandler;
	UnlockHandler unlockHandler;
	public Text playTextLabel, optionsTextLabel, selectShipTextLabel, 	leaderboardsTextLabel, achievementsTextLabel, 
		quitTextLabel, getCreditsTextLabel, GPGSEnableTextLabel;
	public Text highscoreLabel, currentModeLabel, modeDescriptionLabel, creditsLabel, playButtonLabel, quitButtonLabel;
	public Button playButton, getCreditsButton;
	private int highscore;
	private int currentSelection;
	private int numberOfGameModes;
	private float horizontalInput;
	private bool readyToReadHorizontalInput = true;
	private bool gpgsActivated = false;
	public string androidGameID;
	public string iphoneGameID;
	private static IStoreController m_StoreController;
	private static IExtensionProvider m_StoreExtensionProvider;

	public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs e){
		return PurchaseProcessingResult.Complete;
	}
	public void OnInitializeFailed(InitializationFailureReason error){
		Debug.Log ("OnInitializeFailed Reason: " + error);
	}
	public void OnPurchaseFailed(Product i, PurchaseFailureReason p){}
	public void OnInitialized(IStoreController controller, IExtensionProvider extensions){
		Debug.Log ("OnInitialized: PASS");

		Product product = controller.products.WithID(GloVar.kProductIDUnlockFullGame);
		if (product != null && product.hasReceipt)
		{
			PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 1);
		}
		else{
			PlayerPrefs.SetInt (GloVar.GamePurchasedPrefName, 0);
		}
		SetLabels();
	}

	void Start(){		
		languageHandler = (LanguageHandler)GameObject.FindGameObjectWithTag ("LanguageHandler").GetComponent (typeof(LanguageHandler));
		numberOfGameModes = Enum.GetNames (typeof(GameMode)).Length;
		credits = PlayerPrefs.GetInt (GloVar.CreditsPrefName, GloVar.InitialCredits);
		unlockHandler = (UnlockHandler)GameObject.FindGameObjectWithTag ("UnlockHandler").GetComponent (typeof(UnlockHandler));
		audioHandler = (AudioHandler)GameObject.FindGameObjectWithTag ("MusicPlayer").GetComponent (typeof(AudioHandler));
		audioHandler.PlayMainMenu ();

		currentSelection = PlayerPrefs.GetInt (GloVar.GameModePrefName);
		if (m_StoreController == null){
			if (m_StoreController == null || m_StoreExtensionProvider == null){
				var builder = ConfigurationBuilder.Instance (StandardPurchasingModule.Instance ());
				builder.AddProduct (GloVar.kProductIDUnlockFullGame, ProductType.NonConsumable);
				UnityPurchasing.Initialize (this, builder);
			}
		}
		
		if (PlayerPrefs.GetInt(GloVar.AutoSignInPrefName, 1) == 1){
			if (PlayGamesPlatform.Instance.IsAuthenticated () == false){
				if (!Application.isEditor){
					PlayGamesPlatform.Activate ();
				}
			}
			if (!Social.Active.localUser.authenticated && !Application.isEditor){
				Social.Active.localUser.Authenticate ((bool success) =>{
					if (success){
						PlayerPrefs.SetInt (GloVar.AutoSignInPrefName, 1);
						Debug.Log("user authenticated");
					}
					else{
						PlayerPrefs.SetInt (GloVar.AutoSignInPrefName, 0);
						Debug.Log("authentication failed");
						// ASSUMING PLAYER DOES NOT WANT TO USE SOCIAL: SET TO SIGN OFF
					}
						
				});
			}
		}
		if (Advertisement.isSupported){
			if (Application.platform == RuntimePlatform.Android){
				Advertisement.Initialize (androidGameID);
			}
			else if (Application.platform == RuntimePlatform.IPhonePlayer){
				Advertisement.Initialize (iphoneGameID);
			}
		}


		SetLabels ();
	}
	void SetFromPrefs(){
		
	}
	void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) { Application.Quit (); }
		horizontalInput = Input.GetAxis ("Horizontal");
		if (readyToReadHorizontalInput){
			if (horizontalInput<0 && readyToReadHorizontalInput){
				readyToReadHorizontalInput = false;
				LeftClick ();
				Invoke ("EnableInput", 0.3f);
			}
			else if (horizontalInput>0){
				readyToReadHorizontalInput = false;
				RightClick ();
				Invoke ("EnableInput", 0.3f);
			}
		}
	}
	public void EnableInput(){
		readyToReadHorizontalInput = true;
	}

	public void SetLabels(){
		// BUTTON TEXTS
		languageHandler.SetTextSettings (playTextLabel, "play", FontSize.ExtraLarge);
		languageHandler.SetTextSettings (optionsTextLabel, "options", FontSize.Large);
		languageHandler.SetTextSettings (selectShipTextLabel, "selectship", FontSize.Large);
		languageHandler.SetTextSettings (leaderboardsTextLabel, "leaderboards", FontSize.Medium);
		languageHandler.SetTextSettings (achievementsTextLabel, "achievements", FontSize.Medium);
		languageHandler.SetTextSettings (quitTextLabel, "quit", FontSize.ExtraLarge);
		languageHandler.SetTextSettings (getCreditsTextLabel, "getcredits", FontSize.Large);

		// HIDE CREDITS?
		if (PlayerPrefs.GetInt(GloVar.GamePurchasedPrefName, 0)==1){
			getCreditsButton.gameObject.SetActive (false);
			creditsLabel.gameObject.SetActive (false);
		}

		languageHandler.SetTextSettings (currentModeLabel, GameModeExtensions.ToMenuKey ((GameMode)currentSelection), FontSize.Large);

		if (unlockHandler.IsModeUnlocked ((GameMode)currentSelection)){
			currentModeLabel.color = Color.white;
			modeDescriptionLabel.text = languageHandler.GetModeDescription ((GameMode)currentSelection);
			modeDescriptionLabel.fontSize = languageHandler.GetFontSize (FontSize.Small);
			modeDescriptionLabel.font = languageHandler.GetFont ();
			playButton.enabled = true;
			playButton.gameObject.SetActive (true);
		}
		else{
			currentModeLabel.color = Color.gray;
			modeDescriptionLabel.text = languageHandler.GetModeUnlockDescription ((GameMode)currentSelection);
			playButton.gameObject.SetActive (false);
		}
		switch ((GameMode)currentSelection) {
		case GameMode.Normal_SlowStart:
		case GameMode.Normal_FastStart:
			highscore = PlayerPrefs.GetInt (GloVar.NormalModeHighscorePrefName, 0);
			break;
		case GameMode.ThreeStrikes:
			highscore = PlayerPrefs.GetInt (GloVar.ThreeStrikesModeHighscorePrefName, 0);
			break;
		case GameMode.Slalom:
			highscore = PlayerPrefs.GetInt (GloVar.SlalomModeHighscorePrefName, 0);
			break;
		case GameMode.Spiral:
			highscore = PlayerPrefs.GetInt (GloVar.SpiralModeHighscorePrefName, 0);
			break;
		case GameMode.HyperSpeed:
			highscore = PlayerPrefs.GetInt (GloVar.HyperSpeedModeHighscorePrefName, 0);
			break;
		default:
			Debug.Log ("Error: Couldn't determinte game mode for high score load");
			highscore = PlayerPrefs.GetInt (GloVar.NormalModeHighscorePrefName, 0);
			break;
		}
		languageHandler.SetTextSettings (highscoreLabel, "highscore", FontSize.Medium);
		highscoreLabel.text += ": " + highscore;
		UpdateCreditsLabel ();

	}
	public void UpdateCreditsLabel(){
		if (PlayerPrefs.GetInt(GloVar.GamePurchasedPrefName, 0) == 0){
			languageHandler.SetTextSettings (creditsLabel, "credits", FontSize.Medium);
			creditsLabel.text += ": " + credits;
		}
	}
	public void LeftClick(){
		if (currentSelection > 0) {
			currentSelection--;
			SetLabels ();
		} else{
			currentSelection = numberOfGameModes-1;
			SetLabels ();
		}
	}
	public void RightClick(){
		if (currentSelection<numberOfGameModes-1){
			currentSelection++;
			SetLabels ();
		}
		else{
			currentSelection = 0;
			SetLabels ();
		}
	}
	public void PlayClick(){
		PlayerPrefs.SetInt (GloVar.GameModePrefName, currentSelection);
		if (PlayerPrefs.GetInt (GloVar.GamePurchasedPrefName, 0) == 1) {
			SceneManager.LoadScene (GloVar.MainGameSceneName);
		}
		else{
			SceneManager.LoadScene (GloVar.PreGameSceneName);
		}
	}
	public void ShipSelectClick(){
		SceneManager.LoadScene (GloVar.ShipSelectionSceneName);
	}

	public void QuitGameClick(){
		Application.Quit ();
	}
		
	public void LeaderboardsClick(){
		if (PlayGamesPlatform.Instance.IsAuthenticated () == true) {
			if (Social.Active.localUser.authenticated == true){
				Social.ShowLeaderboardUI ();
			}
			else{
				Social.Active.localUser.Authenticate((bool success) =>{
					if (success)
						Debug.Log("user authenticated");
					else
						Debug.Log("authentication failed");
				});
			}

		}
		else{
			languageHandler.SetTextSettings (GPGSEnableTextLabel, "gpgsenableleaderboards", FontSize.Medium);
			Invoke("ClearGPGSEnableText", 4f);
			PlayGamesPlatform.Activate ();
			if (!Social.Active.localUser.authenticated && !Application.isEditor){
				Social.Active.localUser.Authenticate ((bool success) =>{
					if (success)
						Debug.Log("user authenticated");
					else{
						Debug.Log("authentication failed");
					}
				});
			}
		}
	}
	public void AchievementsClick(){
		if (PlayGamesPlatform.Instance.IsAuthenticated () == true) {
			if (Social.Active.localUser.authenticated == true){
				Social.ShowAchievementsUI ();
			}
			else{
				Social.Active.localUser.Authenticate((bool success) =>{
					if (success)
						Debug.Log("user authenticated");
					else
						Debug.Log("authentication failed");
				});
			}

		}
		else{
			languageHandler.SetTextSettings (GPGSEnableTextLabel, "gpgsenableachievements", FontSize.Medium);
			Invoke("ClearGPGSEnableText", 4f);
			PlayGamesPlatform.Activate ();
			if (!Social.Active.localUser.authenticated && !Application.isEditor){
				Social.Active.localUser.Authenticate ((bool success) =>{
					if (success)
						Debug.Log("user authenticated");
					else{
						Debug.Log("authentication failed");
					}
				});
			}
		}
	}
	public void ClearGPGSEnableText(){
		GPGSEnableTextLabel.text = "";
	}
	public void OptionsClick(){
		SceneManager.LoadScene (GloVar.OptionsSceneName);
	}
	public void CreditsClick(){
		SceneManager.LoadScene (GloVar.CreditsSceneName);
	}
}
