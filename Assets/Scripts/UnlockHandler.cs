using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using System.Text;

public class UnlockHandler : MonoBehaviour{

	private List<UnlockableObject> unlockables;
	private List<UnlockableObject> unlockableShips;
	private List<UnlockableObject> unlockableModes;
	UnlockableObject fastModeUnlockReq, spiralModeUnlockReq, slalomModeUnlockReq, hypedSpeedModeUnlockReq, threeStrikesModeUnlockReq;
	UnlockableObject greenShipUnlockReq, pinkShipUnlockReq, mintShipUnlockReq, blackRedShipUnlockReq,
		redYellowShipUnlockReq, pinkBlueShipUnlockReq, redWhiteShipUnlockReq, tealShipUnlockReq, customShipUnlockReq, customShipTest;

	private bool[] wasUnlocked;

	void Awake(){
		GameObject[] unlockHandlers = GameObject.FindGameObjectsWithTag ("UnlockHandler");
		if (unlockHandlers [0] != this.gameObject)
			Destroy (this.gameObject);
		DontDestroyOnLoad(this);

		// MODES
		fastModeUnlockReq = new UnlockableObject (this,UnlockableObjectType.Mode, 1, UnlockCriteriaType.ScoreXPoints, GameMode.Normal_SlowStart, 3000);
		threeStrikesModeUnlockReq = new UnlockableObject(this, UnlockableObjectType.Mode, 2, UnlockCriteriaType.SurviveXTimesYSection, GameMode.Normal_FastStart, 3, 3);
		spiralModeUnlockReq = new UnlockableObject (this,UnlockableObjectType.Mode, 3, UnlockCriteriaType.ScoreXPoints, GameMode.Normal_FastStart, 5000);
		slalomModeUnlockReq = new UnlockableObject (this,UnlockableObjectType.Mode, 4, UnlockCriteriaType.SurviveXTimesYSection, GameMode.Normal_FastStart, 3, 5);
		hypedSpeedModeUnlockReq = new UnlockableObject (this, UnlockableObjectType.Mode, 5, UnlockCriteriaType.BuyGame, GameMode.HyperSpeed, 0);

		// SHIPS
		greenShipUnlockReq = new UnlockableObject (this, UnlockableObjectType.Ship, GloVar.GreenShipID, UnlockCriteriaType.DieXTimesYSection, GameMode.Normal_FastStart, 5, 1);
		pinkShipUnlockReq = new UnlockableObject (this,UnlockableObjectType.Ship, GloVar.PinkShipID, UnlockCriteriaType.ScoreXPoints, GameMode.Normal_FastStart, 2000);
		mintShipUnlockReq = new UnlockableObject (this,UnlockableObjectType.Ship, GloVar.MintShipID, UnlockCriteriaType.SurviveXTimesYSection, GameMode.Spiral, 3, 3);
		blackRedShipUnlockReq = new UnlockableObject (this,UnlockableObjectType.Ship, GloVar.BlackRedShipID, UnlockCriteriaType.ScoreXPoints, GameMode.Slalom, 3000);
		redYellowShipUnlockReq = new UnlockableObject (this, UnlockableObjectType.Ship, GloVar.PurpleYellowShip, UnlockCriteriaType.ScoreXPoints, GameMode.ThreeStrikes, 15000);
		pinkBlueShipUnlockReq = new UnlockableObject (this, UnlockableObjectType.Ship, GloVar.AquaYellowShipID, UnlockCriteriaType.SurviveXTimesYSection, GameMode.Normal_FastStart, 5, 10);
		redWhiteShipUnlockReq = new UnlockableObject(this, UnlockableObjectType.Ship, GloVar.RedWhiteShipID, UnlockCriteriaType.ScoreXPoints, GameMode.Spiral, 12000);
		tealShipUnlockReq = new UnlockableObject (this, UnlockableObjectType.Ship, GloVar.TealShipID, UnlockCriteriaType.SurviveXTimesYSection, GameMode.Slalom, 3, 5);
		customShipUnlockReq = new UnlockableObject (this, UnlockableObjectType.Ship, GloVar.CustomShipID, UnlockCriteriaType.BuyGame, GameMode.Normal_SlowStart, 0);
	}

	public void StoreUnlockProgress(){
		wasUnlocked = new bool[unlockables.Count];
		int i = 0;
		foreach (UnlockableObject unlockable in unlockables){
			wasUnlocked [i] = unlockable.IsUnlocked ();
			i++;
		}
	}

	public void AddToUnlockables(UnlockableObject unlockable){
		if (unlockables == null){
			unlockables = new List<UnlockableObject> ();
			unlockableShips = new List<UnlockableObject> ();
			unlockableModes = new List<UnlockableObject> ();
		} 
		unlockables.Add (unlockable);
		if (unlockable.GetObjectType () == UnlockableObjectType.Ship){
			unlockableShips.Add (unlockable);
		}
		else if (unlockable.GetObjectType () == UnlockableObjectType.Mode){
			unlockableModes.Add (unlockable);
		}
	}

	public string CheckForUnlocks(LanguageHandler languageHandler){
		string unlocktext = "";
		int i = 0;
		foreach (UnlockableObject unlockable in unlockables){
			Debug.Log (wasUnlocked [i]);
			if (unlockable.IsUnlocked() && !wasUnlocked[i]){
				unlocktext += languageHandler.GetUnlockedText (unlockable) + "\n";
				//unlocktext += unlockable.UnlockText + "\n";
			}
			i++;
		}
		unlocktext = unlocktext.TrimEnd ('\n');
		if (PlayGamesPlatform.Instance.IsAuthenticated () == true) {
			if (Social.Active.localUser.authenticated == true) {
				HandleAchievements ();
			}
		}
		return unlocktext;
	}
		
	public bool IsModeUnlocked(GameMode gameMode){
		switch(gameMode){
		case GameMode.Normal_SlowStart:
			return true;
		case GameMode.Normal_FastStart:
			return fastModeUnlockReq.IsUnlocked ();
			//return PlayerPrefs.GetInt (GlobalVariables.NormalModeHighscorePrefName, 0) >= GlobalVariables.FastModeUnlockRequirement;
		case GameMode.ThreeStrikes:
			return threeStrikesModeUnlockReq.IsUnlocked ();
		case GameMode.Spiral:
			return spiralModeUnlockReq.IsUnlocked ();
			//return PlayerPrefs.GetInt (GlobalVariables.NormalModeHighscorePrefName, 0) >= GlobalVariables.SpiralModeUnlockRequirement;
		case GameMode.Slalom:
			return slalomModeUnlockReq.IsUnlocked ();
		case GameMode.HyperSpeed:
			return hypedSpeedModeUnlockReq.IsUnlocked ();
		default:
			return true;
		}
	}
	public UnlockableObject GetUnlockRequirements(GameMode gameMode){
		switch (gameMode){
		case GameMode.Normal_FastStart:
			return fastModeUnlockReq;
		case GameMode.Slalom:
			return slalomModeUnlockReq;
		case GameMode.ThreeStrikes:
			return threeStrikesModeUnlockReq;
		case GameMode.Spiral:
			return spiralModeUnlockReq;
		case GameMode.HyperSpeed:
			return hypedSpeedModeUnlockReq;
		default:
			return null;
		}
	}
	public UnlockableObject GetUnlockRequirements(ShipObject shipObject){
		switch (shipObject.ShipLangKey){
		case "ship1":
			return greenShipUnlockReq;
		case "ship2":
			return pinkShipUnlockReq;
		case "ship3":
			return mintShipUnlockReq;
		case "ship4":
			return blackRedShipUnlockReq;
		case "ship5":
			return redYellowShipUnlockReq;
		case "ship6":
			return pinkBlueShipUnlockReq;
		case "ship7":
			return redWhiteShipUnlockReq;
		case "ship8":
			return tealShipUnlockReq;
		case "ship9":
			return customShipUnlockReq;
		}
		return null;
	}

	public bool IsShipUnlocked (int id){
		foreach (UnlockableObject unlockableShip in unlockableShips){
			if (unlockableShip.GetObjectID () == id){
				return (unlockableShip.IsUnlocked ());
			}
		}
		Debug.Log ("Could not find ship in question");
		return false;
	}
	public UnlockableObject[] GetUnlockableShips(){
		return unlockableShips.ToArray ();
	}

	public void HandleAchievements(){
		string modeTrackerStr = PlayerPrefs.GetString (GloVar.ModesAchievementsTracker, GloVar.ModesATDefault);
		if (IsModeUnlocked (GameMode.Normal_FastStart) && modeTrackerStr[0]=='x') {
			Social.ReportProgress ("CgkIyOLKsZgIEAIQAQ", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(modeTrackerStr);
					strBuilder[0] = 'Y';
					modeTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ModesAchievementsTracker, modeTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		if (IsModeUnlocked (GameMode.ThreeStrikes) && modeTrackerStr[1]=='x') {
			Social.ReportProgress ("CgkIyOLKsZgIEAIQCw", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(modeTrackerStr);
					strBuilder[1] = 'Y';
					modeTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ModesAchievementsTracker, modeTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		if (IsModeUnlocked (GameMode.Spiral) && modeTrackerStr[2]=='x') {
			Social.ReportProgress ("CgkIyOLKsZgIEAIQAg", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(modeTrackerStr);
					strBuilder[2] = 'Y';
					modeTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ModesAchievementsTracker, modeTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		if (IsModeUnlocked (GameMode.Slalom) && modeTrackerStr[3]=='x') {
			Social.ReportProgress ("CgkIyOLKsZgIEAIQDA", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(modeTrackerStr);
					strBuilder[3] = 'Y';
					modeTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ModesAchievementsTracker, modeTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}

		string masterAchievementTrackerStr = PlayerPrefs.GetString (GloVar.MasterAchievementsTracker, GloVar.MasterATDefault);
		// FAST MODE MASTER
		if (PlayerPrefs.GetInt (GloVar.NormalModeHighscorePrefName, 0) >= GloVar.Master_NormalMode && masterAchievementTrackerStr[0]=='x'){
			Social.ReportProgress (GoogleConstants.achievement_normal_mode_master, 100.0f, (bool success) => {
				if (success){
					StringBuilder strBuilder = new StringBuilder(masterAchievementTrackerStr);
					strBuilder[0] = 'Y';
					masterAchievementTrackerStr = strBuilder.ToString();
					PlayerPrefs.SetString(GloVar.MasterAchievementsTracker, masterAchievementTrackerStr);
					Debug.Log ("Achievement unlocked");
				}
				else{
					Debug.Log ("Achievement unlock fail"); 
				}
			});
		}
		// THREE STRIKES MASTER
		if (PlayerPrefs.GetInt (GloVar.ThreeStrikesModeHighscorePrefName, 0) >= GloVar.Master_ThreeStrikes && masterAchievementTrackerStr[1]=='x'){
			Social.ReportProgress (GoogleConstants.achievement_three_strikes_mode_master, 100.0f, (bool success) => {
				if (success){
					StringBuilder strBuilder = new StringBuilder(masterAchievementTrackerStr);
					strBuilder[1] = 'Y';
					masterAchievementTrackerStr = strBuilder.ToString();
					PlayerPrefs.SetString(GloVar.MasterAchievementsTracker, masterAchievementTrackerStr);
					Debug.Log ("Achievement unlocked");
				}
				else{
					Debug.Log ("Achievement unlock fail"); 
				}
			});
		}
		// SPIRAL MODE MASTER
		if (PlayerPrefs.GetInt (GloVar.SpiralModeHighscorePrefName, 0) >= GloVar.Master_Spiral && masterAchievementTrackerStr[2]=='x'){
			Social.ReportProgress (GoogleConstants.achievement_spiral_mode_master, 100.0f, (bool success) => {
				if (success){
					StringBuilder strBuilder = new StringBuilder(masterAchievementTrackerStr);
					strBuilder[2] = 'Y';
					masterAchievementTrackerStr = strBuilder.ToString();
					PlayerPrefs.SetString(GloVar.MasterAchievementsTracker, masterAchievementTrackerStr);
					Debug.Log ("Achievement unlocked");
				}
				else{
					Debug.Log ("Achievement unlock fail"); 
				}
			});
		}
		// SLALOM MODE MASTER
		if (PlayerPrefs.GetInt (GloVar.SlalomModeHighscorePrefName, 0) >= GloVar.Master_Slalom && masterAchievementTrackerStr[3]=='x'){
			Social.ReportProgress (GoogleConstants.achievement_slalom_mode_master, 100.0f, (bool success) => {
				if (success){
					StringBuilder strBuilder = new StringBuilder(masterAchievementTrackerStr);
					strBuilder[3] = 'Y';
					masterAchievementTrackerStr = strBuilder.ToString();
					PlayerPrefs.SetString(GloVar.MasterAchievementsTracker, masterAchievementTrackerStr);
					Debug.Log ("Achievement unlocked");
				}
				else{
					Debug.Log ("Achievement unlock fail"); 
				}
			});
		}
		// HYPERSPEED MODE
		if (PlayerPrefs.GetInt (GloVar.HyperSpeedModeHighscorePrefName, 0) > GloVar.Master_Hyperspeed && masterAchievementTrackerStr[4]=='x'){
			Social.ReportProgress (GoogleConstants.achievement_hyperspeed_mode_master, 100.0f, (bool success) => {
				if (success){
					StringBuilder strBuilder = new StringBuilder(masterAchievementTrackerStr);
					strBuilder[4] = 'Y';
					masterAchievementTrackerStr = strBuilder.ToString();
					PlayerPrefs.SetString(GloVar.MasterAchievementsTracker, masterAchievementTrackerStr);
					Debug.Log ("Achievement unlocked");
				}
				else{
					Debug.Log ("Achievement unlock fail"); 
				}
			});
		}

		string shipTrackerStr = PlayerPrefs.GetString (GloVar.ShipsAchievementsTracker, GloVar.ShipsATDefault);
		// GREENHORN CRUISER
		if (IsShipUnlocked (1) && shipTrackerStr[0]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQAw", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[0] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		// PINK VENGEANCE
		if (IsShipUnlocked (2) && shipTrackerStr[1]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQBA", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[1] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		// CHOCOMINT FIGHTER
		if (IsShipUnlocked (3) && shipTrackerStr[2]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQDQ", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[2] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		// MAESTER IV
		if (IsShipUnlocked (4) && shipTrackerStr[3]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQDg", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[3] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		// RADICAL BOMBER
		if (IsShipUnlocked (5) && shipTrackerStr[4]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQDw", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[4] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		// NEO LIGHTNING
		if (IsShipUnlocked (6) && shipTrackerStr[5]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQEA", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[5] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		// ADMIRAL
		if (IsShipUnlocked (7) && shipTrackerStr[6]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQEQ", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[6] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}
		// ADMIRAL
		if (IsShipUnlocked (8) && shipTrackerStr[7]=='x'){
			Social.ReportProgress ("CgkIyOLKsZgIEAIQEg", 100.0f, (bool success) => {
				if (success) {
					StringBuilder strBuilder = new StringBuilder(shipTrackerStr);
					strBuilder[7] = 'Y';
					shipTrackerStr = strBuilder.ToString ();
					PlayerPrefs.SetString (GloVar.ShipsAchievementsTracker, shipTrackerStr);
					Debug.Log ("Achievement unlocked");
				} else {
					Debug.Log ("Achievement unlock fail");
				}
			});
		}

		int gamesCounter = PlayerPrefs.GetInt (GloVar.PlaysAchievementTracker, 0)+1;
		bool allOK = true;
		if (gamesCounter<=1021){
			PlayGamesPlatform.Instance.IncrementAchievement(
				"CgkIyOLKsZgIEAIQGA", 1, (bool success) => {
					if (!success)
						allOK = false;
				});
		}
		if (gamesCounter<=521){
			PlayGamesPlatform.Instance.IncrementAchievement(
				"CgkIyOLKsZgIEAIQFw", 1, (bool success) => {
					if (!success)
						allOK = false;
				});
		}
		if (gamesCounter<=121){
			PlayGamesPlatform.Instance.IncrementAchievement(
				"CgkIyOLKsZgIEAIQFg", 1, (bool success) => {
					if (!success)
						allOK = false;
				});
		}
		if (gamesCounter<=71){
			PlayGamesPlatform.Instance.IncrementAchievement(
				"CgkIyOLKsZgIEAIQFQ", 1, (bool success) => {
					if (!success)
						allOK = false;
				});
		}
		if (gamesCounter<=31){
			PlayGamesPlatform.Instance.IncrementAchievement(
				"CgkIyOLKsZgIEAIQFA", 1, (bool success) => {
					if (!success)
						allOK = false;
				});
		}
		if (gamesCounter<=26){
			PlayGamesPlatform.Instance.IncrementAchievement(
				"CgkIyOLKsZgIEAIQEw", 1, (bool success) => {
					if (!success)
						allOK = false;
				});
		}
		if (!allOK)
			gamesCounter--;
		PlayerPrefs.SetInt (GloVar.PlaysAchievementTracker, gamesCounter);
	}
}