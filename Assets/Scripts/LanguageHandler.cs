using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class LanguageHandler : MonoBehaviour {
	public Font font_prstartk;
	public Font font_meiryoBold;
	public Font font_yugothicb;
	public Font font_yahei;
	UnlockHandler unlockHandler;

	void Awake(){
		
		unlockHandler = (UnlockHandler)GameObject.FindGameObjectWithTag ("UnlockHandler").GetComponent (typeof(UnlockHandler));
		GameObject[] languageHandlers = GameObject.FindGameObjectsWithTag ("LanguageHandler");
		if (languageHandlers [0] != this.gameObject)
			Destroy (this.gameObject);
		DontDestroyOnLoad(this);
		if (PlayerPrefs.GetInt(GloVar.LanguagePrefName, -1)==-1){
			switch(Application.systemLanguage){
			case SystemLanguage.Chinese:
			case SystemLanguage.ChineseSimplified:
			case SystemLanguage.ChineseTraditional:
				PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)Language.Chinese);
				break;
			case SystemLanguage.Japanese:
				PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)Language.Japanese);
				break;
			case SystemLanguage.Korean:
				PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)Language.Korean);
				break;
			case SystemLanguage.French:
				PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)Language.French);
				break;
			case SystemLanguage.Portuguese:
				PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)Language.Portuguese);
				break;
			case SystemLanguage.Spanish:
				PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)Language.Spanish);
				break;
			default:
				PlayerPrefs.SetInt (GloVar.LanguagePrefName, (int)Language.English);
				break;
			}
		}
		SetLanguage ((Language)PlayerPrefs.GetInt (GloVar.LanguagePrefName, 0));
	}

	Dictionary<string, string> currentDict;
	Language currentLanguage;

	public void SetTextSettings(Text label, string key, FontSize fontSize=FontSize.Medium){
		label.text = GetWord (key);
		label.font = GetFont ();
		label.lineSpacing = GetLineSpacing ();
		label.fontSize = GetFontSize (fontSize);
	}

	public string GetWord(string key){
		if (key.Equals ("placeholder"))
			return "";
		key = key.ToLower ();
		if (currentDict.ContainsKey (key))
			return currentDict [key];
		else
			return "error: key " + key + " unrecognized";
	}

	public void SetLanguage(Language language){
		currentLanguage = language;
		switch (language){
		case Language.English:
			PopulateEnglish ();
			return;
		case Language.Spanish:
			PopulateSpanish ();
			return;
		case Language.Portuguese:
			PopulatePortuguese ();
			return;
		case Language.French:
			PopulateFrench ();
			return;
		case Language.Japanese:
			PopulateJapanese ();
			return;
		case Language.Chinese:
			PopulateChinese ();
			return;
		case Language.Korean:
			PopulateKorean ();
			return;
		default:
			goto case Language.English;
		}
	}

	public Language GetCurrentLanguage(){
		return currentLanguage;
	}

	public Font GetFont(){
		switch(currentLanguage){

		case Language.English:
			return font_prstartk;
		
		case Language.Chinese:
			return font_yahei;
		case Language.Korean:
			return font_yugothicb;

		case Language.Japanese:
		case Language.French:
		case Language.Spanish:
		case Language.Portuguese:
			return font_meiryoBold;
		default:
			return font_prstartk;
		}
	}

	public float GetLineSpacing(){
		switch(currentLanguage){

		case Language.English:
			return 1.2f;
		case Language.Japanese:
		case Language.Chinese:
			return 1.0f;
		case Language.French:
		case Language.Spanish:
		case Language.Portuguese:
			return 0.8f;
		default:
			return 1.0f;
		}
	}

	public int GetFontSize(FontSize fontSize){
		switch(currentLanguage){

		case Language.English:
			switch(fontSize){
			case FontSize.Small:
				return 14;
			case FontSize.Medium:
				return 16;
			case FontSize.Large:
				return 18;
			case FontSize.ExtraLarge:
				return 25;
			default:
				return -1;
			}
		case Language.Portuguese:
		case Language.Spanish:
		case Language.French:
			switch(fontSize){
			case FontSize.Small:
				return 15;
			case FontSize.Medium:
				return 17;
			case FontSize.Large:
				return 21;
			case FontSize.ExtraLarge:
				return 24;
			default:
				return -1;
			}
		case Language.Japanese:
			switch(fontSize){
			case FontSize.Small:
				return 18;
			case FontSize.Medium:
				return 20;
			case FontSize.Large:
				return 24;
			case FontSize.ExtraLarge:
				return 27;
			default:
				return -1;
			}
		case Language.Chinese:
		case Language.Korean:
			switch(fontSize){
			case FontSize.Small:
				return 20;
			case FontSize.Medium:
				return 23;
			case FontSize.Large:
				return 23;
			case FontSize.ExtraLarge:
				return 28;
			default:
				return -1;
			}
		default:
			return -1;
		}

	}

	public string GetUnlockedText(UnlockableObject uObj){
		if (uObj.GetObjectType()==UnlockableObjectType.Mode){
			switch(currentLanguage){
			case Language.French:
				return GetWord (""+((GameMode)uObj.GetObjectID()).ToUnlockedKey()) + " " + GetWord("unlocked") +"!";
			case Language.English:
				return GetWord (""+((GameMode)uObj.GetObjectID()).ToUnlockedKey()) + " " + GetWord("unlocked") +"!";
			case Language.Japanese:
				return GetWord ("" + ((GameMode)uObj.GetObjectID ()).ToUnlockedKey ()) + "を" + GetWord ("unlocked")+"!";
			case Language.Chinese:
				return  GetWord ("" + ((GameMode)uObj.GetObjectID ()).ToUnlockedKey ()) + GetWord("unlocked");
			case Language.Korean:
				return GetWord ("" + ((GameMode)uObj.GetObjectID ()).ToUnlockedKey ()) + "를 " + GetWord ("unlocked")+"!";
			case Language.Spanish:
				return (""+((GameMode)uObj.GetObjectID()).ToUnlockedKey()) + " " + GetWord("unlocked") +"!";
			default:
				goto case Language.English;
			}

		}else if (uObj.GetObjectType ()==UnlockableObjectType.Ship){
			switch(currentLanguage){
			case Language.French:
				return GetWord ("ship" + uObj.GetObjectID()) + " " + GetWord ("unlocked") + "!";
			case Language.Spanish:
				return GetWord ("ship" + uObj.GetObjectID()) + " " + GetWord ("unlocked") + "!";
			case Language.English:
				return GetWord ("ship" + uObj.GetObjectID()) + " unlocked!";
			case Language.Japanese:
				return GetWord ("ship" + uObj.GetObjectID()) + "を"+ GetWord("unlocked") + "!";
			case Language.Chinese:
				return  GetWord("ship" + uObj.GetObjectID ()) + GetWord("unlocked");
			case Language.Korean:
				return GetWord ("ship" + uObj.GetObjectID ()) + "를 " + GetWord ("unlocked")+"!";
			default: 
				goto case Language.English;
			}
		}
		return "error: type is not unlockable object (???)";
	}

	public string GetModeUnlockDescription(GameMode gameMode){
		return GenerateUnlockText (unlockHandler.GetUnlockRequirements (gameMode));
	}

	public string GetShipUnlockDescription(ShipObject shipObject){
		if (shipObject.ShipLangKey == "ship0")
			return GetWord ("defaultship");
		return GenerateUnlockText (unlockHandler.GetUnlockRequirements (shipObject));
	}

	public string GenerateUnlockText(UnlockableObject uObj){
		
		switch(uObj.GetCriteriaType()){

		case UnlockCriteriaType.ScoreXPoints:
			switch(currentLanguage){
			case Language.Spanish:
				return "Debe conseguir " + uObj.GetScore () + " Puntos en " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " para Desbloquear";
			case Language.Portuguese:
				return "Atinja " + uObj.GetScore () + " Pontos no " + GetWord(uObj.GetGameMode ().ToUnlockKey ()) + " para desbloquear";
			case Language.French:
				return "Obtenez " + uObj.GetScore () + " points en " + GetWord(uObj.GetGameMode ().ToUnlockKey ()) + " pour le débloquer";
			case Language.English:
				return "Score " + uObj.GetScore () + " points in\n" + GetWord(uObj.GetGameMode ().ToUnlockKey ()) + " to " + GetWord("unlock");
			case Language.Japanese:
				return "" + GetWord(uObj.GetGameMode().ToUnlockKey()) + "で" + uObj.GetScore() + "点取ると" + GetWord("unlock");
			case Language.Chinese:
				return "在" + GetWord(uObj.GetGameMode().ToUnlockKey()) +"中获取" + uObj.GetScore () + "分可解锁";
			case Language.Korean:
				return "" + GetWord(uObj.GetGameMode().ToUnlockKey()) + "에서\n" + uObj.GetScore() + GetWord("score") + " 바드면 " + GetWord("unlock");
			default:
				return "error";
			}

		case UnlockCriteriaType.DieXTimesYSection:
			switch(currentLanguage){
			case Language.Spanish:
				return "Debe morir " + uObj.GetTimes () + " veces en la sección " + uObj.GetSection () + " en " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " para desbloquear";
			case Language.Portuguese:
				return "Morra " + uObj.GetTimes () + " vezes na seção " + uObj.GetSection () + " no " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " para desbloquear";
			case Language.French:
				return "Mourrez " + uObj.GetTimes () + " fois dans la section " + uObj.GetSection () + " en " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " pour le débloquer";
			case Language.English:
				return "Die " + uObj.GetTimes () + " times in section " + uObj.GetSection () + "\nin " + GetWord(uObj.GetGameMode().ToUnlockKey ()) + " to " + GetWord("unlock");
			case Language.Japanese:
				return GetWord(uObj.GetGameMode().ToUnlockKey ()) + "の" + GetWord ("section") + uObj.GetSection () + "で" + uObj.GetTimes () + "回死ぬと" + GetWord("unlock");
			case Language.Chinese:
				return "在" + GetWord(uObj.GetGameMode().ToUnlockKey()) +"中" + uObj.GetSection () + "区域内丧命" + uObj.GetTimes () + "次可解锁";
			case Language.Korean:
				return GetWord(uObj.GetGameMode().ToUnlockKey ()) + "의 " + GetWord ("section") + uObj.GetSection () + "에서\n" + uObj.GetTimes () + "회 죽으면 " + GetWord("unlock");
			default:
				return "error";
			}

		case UnlockCriteriaType.SurviveXTimesYSection:
			switch(currentLanguage){
			case Language.Spanish:
				return "Llegue a la sección " + uObj.GetSection () + " al menos " + uObj.GetTimes () + " veces en " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " para desbloquear";
			case Language.Portuguese:
				return "Chegue à seção " + uObj.GetSection () + " pelo menos " + uObj.GetTimes () + " vezes no " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " para desbloquear";
			case Language.French:
				return "Atteignez " + uObj.GetTimes () + " fois la section " + uObj.GetSection () + " en " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " pour le débloquer";
			case Language.English:
				return "Make it to section " + uObj.GetSection () + " at least\n" + uObj.GetTimes () + " times in " + GetWord (uObj.GetGameMode ().ToUnlockKey ()) + " to " + GetWord("unlock");
			case Language.Japanese:
				return GetWord(uObj.GetGameMode().ToUnlockKey()) + "で" + GetWord ("section")+uObj.GetSection () + "まで\n"+ uObj.GetTimes() + "回クリアすると" + GetWord("unlock");
			case Language.Chinese:
				return "在" + GetWord(uObj.GetGameMode().ToUnlockKey()) +"中进入" + uObj.GetSection () + "区域" + uObj.GetTimes () + "次可解锁";
			case Language.Korean:
				return GetWord(uObj.GetGameMode().ToUnlockKey()) + "에서" + GetWord ("section")+uObj.GetSection () + "까지\n"+ uObj.GetTimes() + "회 생존 하면 " + GetWord("unlock");
			default:
				return "error";
			}
		case UnlockCriteriaType.BuyGame:
			switch(currentLanguage){
			case Language.Spanish:
				return "Apoye a los desarrolladores comprando este juego para desbloquear";
			case Language.Portuguese:
				return "Ajude os desenvolvedores comprando esse jogo para desbloquear";
			case Language.French:
				return "Soutenez les développeurs pour le débloquer";
			case Language.English:
				return "Support the developers by\nbuying this game to unlock";
			case Language.Japanese:
				return "このゲームを買うと" + GetWord("unlock");
			case Language.Chinese:
				return "支持游戏开发者！购买游戏可解锁！";
			case Language.Korean:
				return "개임을 사면 " + GetWord ("unlock");
			default:
				return "error";
			}
		default:
			return "error";
		}
	}

	// SPECIAL ENUM METHODS:

	public string GetModeDescription(GameMode gameMode){
		switch(gameMode){
		case GameMode.Normal_SlowStart:
			return GetWord("normalslowdesc");
		case GameMode.Normal_FastStart:
			return GetWord("normalfastdesc");
		case GameMode.ThreeStrikes:
			return GetWord ("threestrikesdesc");
		case GameMode.Spiral:
			return GetWord("spiraldesc");
		case GameMode.Slalom:
			return GetWord("slalomdesc");
		case GameMode.HyperSpeed:
			return GetWord ("hyperspeeddesc");
		default:
			return "Error: Did not find mode description";
		}
	}

	public void PopulateEnglish(){
		currentDict = new Dictionary<string,string>();

		// MAIN MENU
		currentDict.Add ("play", "Play");
		currentDict.Add ("quit", "Quit");
		currentDict.Add ("options", "Options");
		currentDict.Add ("selectship", "Select\nShip");
		currentDict.Add ("leaderboards", "Leaderboards"); 
		currentDict.Add ("getcredits", "Get\nCredits");
		currentDict.Add ("credits", "Credits"); 
		currentDict.Add ("highscore", "Highscore"); 
		currentDict.Add ("achievements", "Achievements");

		// MODES
		currentDict.Add ("normalmode", "Normal Mode"); 
		currentDict.Add ("normalslow", "Normal (Slow Start)");
		currentDict.Add ("normalfast", "Normal (Fast Start)");
		currentDict.Add ("slalommode", "Slalom Mode");
		currentDict.Add ("spiralmode", "Spiral Mode"); 
		currentDict.Add ("hyperspeedmode", "Hyperspeed Mode");
		currentDict.Add ("threestrikesmode", "Three Strikes Mode");

		// MODE DESCRIPTIONS
		currentDict.Add ("normalslowdesc", "Normal mode with slow start\nFor beginners");
		currentDict.Add ("normalfastdesc", "Normal mode with fast start");
		currentDict.Add ("slalomdesc", "Only slalom\nGood finger exercise");
		currentDict.Add ("spiraldesc", "Only spirals\nDon't get dizzy!"); 
		currentDict.Add ("hyperspeeddesc", "Simple obstacles at lightning speed!");
		currentDict.Add ("threestrikesdesc", "Three strikes and you're out!") ;

		//OPTIONS
		currentDict.Add ("language", "Language");
		currentDict.Add ("musicvolume", "Music Volume");
		currentDict.Add ("audiovolume", "Audio Volume");
		currentDict.Add ("control", "Control"); 
		currentDict.Add ("recording", "Record Replay"); 
		currentDict.Add ("twohanded", "Two-Handed"); 
		currentDict.Add	("onehandedleft", "Left Hand");
		currentDict.Add ("onehandedright", "Right Hand"); 
		currentDict.Add ("enabled", "Enabled");
		currentDict.Add ("disabled", "Disabled"); 
		currentDict.Add ("cancel", "Cancel"); 
		currentDict.Add ("saveexit", "Save &\nReturn"); 
		currentDict.Add ("off", "Off");

		currentDict.Add ("resetstats", "Reset\nStats");
		currentDict.Add ("signout", "Sign Out");
		currentDict.Add ("signin", "Sign In");

		// MESSAGES
		currentDict.Add ("unlocked", "unlocked");
		currentDict.Add ("unlock", "unlock"); 

		// MAIN GAME
		currentDict.Add ("recordingok", "Recording Game");
		currentDict.Add ("recordingfailed", "Not Recording");
		currentDict.Add ("ready", "READY");
		currentDict.Add ("score", "Score"); 
		currentDict.Add ("speed", "Speed"); 
		currentDict.Add ("section", "Section"); 
		currentDict.Add ("newhighscore", "New Highscore"); 
		currentDict.Add ("pause", "Pause");
		currentDict.Add ("unpause", "Unpause");
		currentDict.Add ("watchreplay", "Watch Replay");
		currentDict.Add ("mainmenu", "Main Menu");
		currentDict.Add ("restart", "Restart");
		currentDict.Add ("lives", "Lives");
		currentDict.Add ("leaderboard", "Leaderboard");
		currentDict.Add ("scoreposted", "Score Posted");
		currentDict.Add ("scorepostfailed", "Failed to post score");

		// SHIP NAMES
		currentDict.Add ("ship0", "Blue Knight");
		currentDict.Add ("ship1", "Greenhorn Cruiser");
		currentDict.Add ("ship2", "Pink Vengeance");
		currentDict.Add ("ship3", "Chocomint Fighter");
		currentDict.Add ("ship4", "Maester IV"); 
		currentDict.Add ("ship5", "Radical Bomber") ;
		currentDict.Add ("ship6", "Neo Lightning"); 
		currentDict.Add ("ship7", "Admiral");
		currentDict.Add ("ship8", "Type X");
		currentDict.Add ("ship9", "Custom Ship");

		// CREDITS MENU
		currentDict.Add ("watchvideo", "Watch Ad");
		currentDict.Add ("loadingvideo", "Loading..."); 
		currentDict.Add ("buygame", "Buy Game");
		currentDict.Add ("buygameinfo", "Infinite credits\nUnlock " + GetWord ("hyperspeedmode") + "\nUnlock " + GetWord ("ship9"));
		currentDict.Add ("watchvideoinfo", "Watch a short advertisement \nto get " + GloVar.CreditsPerViewing + " credits");

		// SELECT SHIP MENU
		currentDict.Add ("defaultship", "Your default starting ship");
		currentDict.Add ("select", "Select");

		// GOOGLE PLAY ENABLE MESSAGES
		currentDict.Add ("gpgsenableachievements", "Google Play Games is necessary\nfor using Achievements");
		currentDict.Add ("gpgsenableleaderboards", "Google Play Games is necessary\nfor using Leaderboards");


	}

	public void PopulateFrench(){
		currentDict = new Dictionary<string,string>();

		// MAIN MENU
		currentDict.Add ("play", "Jouer");
		currentDict.Add ("quit", "Quitter");
		currentDict.Add ("options", "Options");
		currentDict.Add ("selectship", "Sélectionner\nle vaisseau");
		currentDict.Add ("leaderboards", "Classement"); 
		currentDict.Add ("getcredits", "Obtenir\ndes crédits");
		currentDict.Add ("credits", "Crédits"); 
		currentDict.Add ("highscore", "Record"); 
		currentDict.Add ("achievements", "Accomplissements");

		// MODES
		currentDict.Add ("normalmode", "Mode Normal"); 
		currentDict.Add ("normalslow", "Normal (départ lent)");
		currentDict.Add ("normalfast", "Normal (départ rapide)");
		currentDict.Add ("slalommode", "Mode Slalom");
		currentDict.Add ("spiralmode", "Mode Spirale"); 
		currentDict.Add ("hyperspeedmode", "Mode Vitesse Lumière");
		currentDict.Add ("threestrikesmode", "Mode Trois Essais");

		// MODE DESCRIPTIONS
		currentDict.Add ("normalslowdesc", "Mode normal avec départ lent\nPour débutants");
		currentDict.Add ("normalfastdesc", "Mode normal avec départ rapide");
		currentDict.Add ("slalomdesc", "Rien que du slalom\nPréparez-vous à muscler vos doigts!");
		currentDict.Add ("spiraldesc", "Rien que des spirales\nAttention au mal de mer!"); 
		currentDict.Add ("hyperspeeddesc", "Évitez les obstacles à la vitesse lumière!");
		currentDict.Add ("threestrikesdesc", "Trois essais et vous êtes mort!") ; 

		//OPTIONS
		currentDict.Add ("language", "Langue");
		currentDict.Add ("musicvolume", "Volume musique");
		currentDict.Add ("audiovolume", "Volume audio");
		currentDict.Add ("control", "Contrôles"); 
		currentDict.Add ("recording", "Enregistrer la partie"); 
		currentDict.Add ("twohanded", "Deux mains"); 
		currentDict.Add	("onehandedleft", "Main gauche");
		currentDict.Add ("onehandedright", "Main droite"); 
		currentDict.Add ("enabled", "Activer");
		currentDict.Add ("disabled", "Désactiver"); 
		currentDict.Add ("cancel", "Annuler"); 
		currentDict.Add ("saveexit", "Sauvegarder et\nRevenir"); 
		currentDict.Add ("off", "Désactivé");

		currentDict.Add ("resetstats", "Réinitialiser\nles statistiques");
		currentDict.Add ("signout", "Se déconnecter");
		currentDict.Add ("signin", "Se connecter");

		// MESSAGES
		currentDict.Add ("unlocked", "Déverrouillé");
		currentDict.Add ("unlock", "Déverrouiller"); 

		// MAIN GAME
		currentDict.Add ("recordingok", "Enregistrement de la\npartie en cours");
		currentDict.Add ("recordingfailed", "Not Recording");
		currentDict.Add ("ready", "PRÊT");
		currentDict.Add ("score", "Score"); 
		currentDict.Add ("speed", "Vitesse"); 
		currentDict.Add ("section", "Section"); 
		currentDict.Add ("newhighscore", "Nouveau Record"); 
		currentDict.Add ("pause", "Pause");
		currentDict.Add ("unpause", "Retour");
		currentDict.Add ("watchreplay", "Revoir\nla Partie");
		currentDict.Add ("mainmenu", "Menu\nPrincipal");
		currentDict.Add ("restart", "Rejouer");
		currentDict.Add ("lives", "Vies");
		currentDict.Add ("leaderboard", "Classement");
		currentDict.Add ("scoreposted", "Score publié");
		currentDict.Add ("scorepostfailed", "Impossible de publier le score");

		// SHIP NAMES
		currentDict.Add ("ship0", "Le Chevalier de Saphir");
		currentDict.Add ("ship1", "Vegetor");
		currentDict.Add ("ship2", "Le Vengeur Pourpre");
		currentDict.Add ("ship3", "Le Guerrier Chocomenthe");
		currentDict.Add ("ship4", "Maestro IV"); 
		currentDict.Add ("ship5", "La Forteresse Volante") ;
		currentDict.Add ("ship6", "Neo Lux"); 
		currentDict.Add ("ship7", "L'Amiral");
		currentDict.Add ("ship8", "Proto-X");
		currentDict.Add ("ship9", "Vaisseau Personnalisé");

		// CREDITS MENU
		currentDict.Add ("watchvideo", "Regarder\nune pub");
		currentDict.Add ("loadingvideo", "Chargement ..."); 
		currentDict.Add ("buygame", "Acheter le jeu");
		currentDict.Add ("buygameinfo", "Crédit infini\nDébloque le " + GetWord ("hyperspeedmode") + "\nDébloque le " + GetWord ("ship9"));
		currentDict.Add ("watchvideoinfo", "Regarder une courte publicité pour obtenir " + GloVar.CreditsPerViewing + " crédits");

		// SELECT SHIP MENU
		currentDict.Add ("defaultship", "Votre vaisseau initial par défaut");
		currentDict.Add ("select", "Sélectionner");

		// GOOGLE PLAY ENABLE MESSAGES
		currentDict.Add ("gpgsenableachievements", "Vous devez être connecté à Google Play Games\npour avoir accès aux accomplissements");
		currentDict.Add ("gpgsenableleaderboards", "Vous devez être connecté à Google Play Games\npour avoir accès au classement\ndes meilleurs joueurs");
	}

	public void PopulateJapanese(){
		currentDict = new Dictionary<string, string> ();

		// MAIN MENU
		currentDict.Add ("play", "スタート");
		currentDict.Add ("quit", "終了"); 
		currentDict.Add ("options", "オプション");
		currentDict.Add ("selectship", "シップ変更"); 
		currentDict.Add ("leaderboards", "スコアボード");
		currentDict.Add ("getcredits", "クレジット");
		currentDict.Add ("credits", "クレジット"); 
		currentDict.Add ("highscore", "ハイスコア");
		currentDict.Add ("achievements", "アチーブメント");

		// MODES
		currentDict.Add ("normalmode", "普通モード"); 
		currentDict.Add ("normalslow", "普通　遅いスタート");
		currentDict.Add ("normalfast", "普通　速いスタート");
		currentDict.Add ("slalommode", "スラロームモード");
		currentDict.Add ("spiralmode", "スパイラルモード"); 
		currentDict.Add ("hyperspeedmode", "超高速モード");
		currentDict.Add ("threestrikesmode", "三振モード");

		// MODE DESCRIPTIONS
		currentDict.Add ("normalslowdesc", "スタートが遅い普通モード\n初心者向け");
		currentDict.Add ("normalfastdesc", "スタートが速い普通モード");
		currentDict.Add ("slalomdesc", "スラロムだけのコース");
		currentDict.Add ("spiraldesc", "スパイラルだけのコース"); 
		currentDict.Add ("hyperspeeddesc", "一番速いモード" );
		currentDict.Add ("threestrikesdesc", "三振するとアウト") ;

		//OPTIONS
		currentDict.Add ("language", "言語");
		currentDict.Add ("musicvolume", "BGM");
		currentDict.Add ("audiovolume", "効果音");
		currentDict.Add ("control", "コントロール"); 
		currentDict.Add ("recording", "プレイをキャプチャー");
		currentDict.Add ("twohanded", "両手"); 
		currentDict.Add	("onehandedleft", "左手");
		currentDict.Add ("onehandedright", "右手"); 
		currentDict.Add ("enabled", "オン");
		currentDict.Add ("disabled", "オフ"); 
		currentDict.Add ("cancel", "キャンセル"); 
		currentDict.Add ("saveexit", "実行"); 
		currentDict.Add ("off", "オフ");

		currentDict.Add ("resetstats", "リセット");
		currentDict.Add ("signout", "ログアウト");
		currentDict.Add ("signin", "ログイン");



		// MAIN GAME
		currentDict.Add ("recordingok", "キャプチャー中");
		currentDict.Add ("recordingfailed", "録画　オフ");
		currentDict.Add ("ready", "READY");
		currentDict.Add ("score", "スコア");
		currentDict.Add ("speed", "スピード"); 
		currentDict.Add ("section", "セクション"); 
		currentDict.Add ("newhighscore", "ニュー　ハイスコア"); 
		currentDict.Add ("pause", "ポーズ");
		currentDict.Add ("unpause", "続く");
		currentDict.Add ("watchreplay", "リプレイ見る");
		currentDict.Add ("mainmenu", "メインメニュー");
		currentDict.Add ("restart", "リスタート");
		currentDict.Add ("lives", "ライフ");
		currentDict.Add ("leaderboard", "スコアボード");
		currentDict.Add ("scoreposted", "スコアをアップした");
		currentDict.Add ("scorepostfailed", "スコアをアップできなかった");


		// SHIP NAMES
		currentDict.Add ("ship0", "ブルー武士");
		currentDict.Add ("ship1", "グリーンホーン・クルーザー");
		currentDict.Add ("ship2", "ピンク・ヴェンデッタ");
		currentDict.Add ("ship3", "チョコミント・ファイター");
		currentDict.Add ("ship4", "マエストロ　ＩＶ"); 
		currentDict.Add ("ship5", "ラディカル・ボンバー");
		currentDict.Add ("ship6", "ネオ雷光"); 
		currentDict.Add ("ship7", "海軍将官船");
		currentDict.Add ("ship8", "タイプＸ");
		currentDict.Add ("ship9", "カスタムデザイン");

		// MESSAGES
		currentDict.Add ("unlocked", "ゲット");
		currentDict.Add("unlock", "ゲット");

		// CREDITS MENU
		currentDict.Add ("watchvideo", "広告を見る");
		currentDict.Add ("loadingvideo", "準備中。。。"); 
		currentDict.Add ("buygame", "ゲームを買う");
		currentDict.Add ("buygameinfo", "無限のクレジット\n" + GetWord ("hyperspeedmode") + "\n" + GetWord ("ship9")+ "を" + GetWord("unlock"));
		currentDict.Add ("watchvideoinfo", "広告を見ると\n" + GloVar.CreditsPerViewing + GetWord("credits") + "をゲット");

		// SELECT SHIP MENU
		currentDict.Add ("defaultship", "デフォルトの宇宙船");
		currentDict.Add ("select", "選ぶ");

		// GOOGLE PLAY ENABLE MESSAGES
		currentDict.Add ("gpgsenableachievements", "アチーブメントを使うため、\nGoogle Play Gamesは必要です");
		currentDict.Add ("gpgsenableleaderboards", "スコアボードを使うため、\nGoogle Play Gamesは必要です");


	}

	public void PopulateChinese(){
		currentDict = new Dictionary<string, string> ();

		// MAIN MENU
		currentDict.Add ("play", "开始");
		currentDict.Add ("quit", "退出"); 
		currentDict.Add ("options", "设置");
		currentDict.Add ("selectship", "选择航舰"); 
		currentDict.Add ("leaderboards", "排行榜");
		currentDict.Add ("getcredits", "获取生命");
		currentDict.Add ("credits", "生命"); 
		currentDict.Add ("highscore", "高分");
		currentDict.Add ("achievements", "成就");

		// MODES
		currentDict.Add ("normalmode", "正常模式"); 
		currentDict.Add ("normalslow", "正常模式（慢启）");
		currentDict.Add ("normalfast", "正常模式（快启）");
		currentDict.Add ("slalommode", "激流回转模式");
		currentDict.Add ("spiralmode", "螺旋模式"); 
		currentDict.Add ("hyperspeedmode", "超音速模式");
		currentDict.Add ("threestrikesmode", "三条命模式");

		// MODE DESCRIPTIONS
		currentDict.Add ("normalslowdesc", "新手推荐\n缓慢启动航舰");
		currentDict.Add ("normalfastdesc", "快速启动航舰");
		currentDict.Add ("slalomdesc", "激流回转\n让手指活动起来");
		currentDict.Add ("spiraldesc", "螺旋\n不要转晕了哦！"); 
		currentDict.Add ("hyperspeeddesc", "超音速通过简单的障碍");
		currentDict.Add ("threestrikesdesc", "你只有三次机会哦！") ;

		//OPTIONS
		currentDict.Add ("language", "语言");
		currentDict.Add ("musicvolume", "音乐音量");
		currentDict.Add ("audiovolume", "特效音量");
		currentDict.Add ("control", "操作设置"); 
		currentDict.Add ("recording", "录制游戏过程");
		currentDict.Add ("twohanded", "双手操作"); 
		currentDict.Add	("onehandedleft", "左手操作");
		currentDict.Add ("onehandedright", "右手操作"); 
		currentDict.Add ("enabled", "录制");
		currentDict.Add ("disabled", "停止"); 
		currentDict.Add ("cancel", "取消"); 
		currentDict.Add ("saveexit", "保存"); 
		currentDict.Add ("off", "静音");

		currentDict.Add ("resetstats", "初始化");
		currentDict.Add ("signout", "退出");
		currentDict.Add ("signin", "登录");



		// MAIN GAME
		currentDict.Add ("recordingok", "录制中");
		currentDict.Add ("recordingfailed", "");
		currentDict.Add ("ready", "预备");
		currentDict.Add ("score", "分数");
		currentDict.Add ("speed", "速度"); 
		currentDict.Add ("section", "区域"); 
		currentDict.Add ("newhighscore", "刷新记录"); 
		currentDict.Add ("pause", "暂停");
		currentDict.Add ("unpause", "开始");
		currentDict.Add ("watchreplay", "播放游戏");
		currentDict.Add ("mainmenu", "主菜单");
		currentDict.Add ("restart", "重启");
		currentDict.Add ("lives", "生命");
		currentDict.Add ("leaderboard", "排行榜");
		currentDict.Add ("scoreposted", "发布成绩");
		currentDict.Add ("scorepostfailed", "发布失败");


		// SHIP NAMES
		currentDict.Add ("ship0", "蔚蓝骑士");
		currentDict.Add ("ship1", "青始巡洋舰");
		currentDict.Add ("ship2", "粉色复仇者");
		currentDict.Add ("ship3", "薄核克力战斗机");
		currentDict.Add ("ship4", "第四代宗师"); 
		currentDict.Add ("ship5", "偏激轰炸机");
		currentDict.Add ("ship6", "新闪电"); 
		currentDict.Add ("ship7", "海军将航舰");
		currentDict.Add ("ship8", "X 类");
		currentDict.Add ("ship9", "个人航舰设计");

		// MESSAGES
		currentDict.Add ("unlocked", "已解锁");
		currentDict.Add("unlock", "解锁");

		// CREDITS MENU
		currentDict.Add ("watchvideo", "观看广告");
		currentDict.Add ("loadingvideo", "请等候……"); 
		currentDict.Add ("buygame", "购买游戏");
		currentDict.Add ("buygameinfo", "无穷积分\n" + GetWord ("hyperspeedmode") + GetWord("unlock") + "\n" + GetWord ("ship9")+ GetWord("unlock"));
		currentDict.Add ("watchvideoinfo", "观看广告以获取" + GloVar.CreditsPerViewing +  "分");

		// SELECT SHIP MENU
		currentDict.Add ("defaultship", "默认航舰");
		currentDict.Add ("select", "选择");

		// GOOGLE PLAY ENABLE MESSAGES
		currentDict.Add ("gpgsenableachievements", "允许Google Play Games\n存取成就");
		currentDict.Add ("gpgsenableleaderboards", "允许Google Play Games\n存取高分榜");
	}

	public void PopulateKorean(){
		currentDict = new Dictionary<string,string>();

		// MAIN MENU
		currentDict.Add ("play", "게임 시작");
		currentDict.Add ("quit", "그만하기");
		currentDict.Add ("options", "설정");
		currentDict.Add ("selectship", "우주선 선택");
		currentDict.Add ("leaderboards", "명예의전당"); 
		currentDict.Add ("getcredits", "크레딧 획득");
		currentDict.Add ("credits", "크레딧"); 
		currentDict.Add ("highscore", "최고점수"); 
		currentDict.Add ("achievements", "업적");

		// MODES
		currentDict.Add ("normalmode", "노멀모드"); 
		currentDict.Add ("normalslow", "느린 노멀모드");
		currentDict.Add ("normalfast", "빠른 노멀모드");
		currentDict.Add ("slalommode", "알파인모드");
		currentDict.Add ("spiralmode", "나선모드"); 
		currentDict.Add ("hyperspeedmode", "하이퍼스피드모");
		currentDict.Add ("threestrikesmode", "삼진 모드");

		// MODE DESCRIPTIONS
		currentDict.Add ("normalslowdesc", "스타트가 느린 노멀모드");
		currentDict.Add ("normalfastdesc", "스타트가 빠른 노멀모드");
		currentDict.Add ("slalomdesc", "회전 활강이 같은 모드");
		currentDict.Add ("spiraldesc", "나선이 많은 모드"); 
		currentDict.Add ("hyperspeeddesc", "속도가 최대치!");
		currentDict.Add ("threestrikesdesc", "삼진하면 죽습니다") ;

		//OPTIONS
		currentDict.Add ("language", "언어");
		currentDict.Add ("musicvolume", "배경음");
		currentDict.Add ("audiovolume", "효과음");
		currentDict.Add ("control", "컨트롤설정"); 
		currentDict.Add ("recording", "비디오녹화"); 
		currentDict.Add ("twohanded", "양손조작"); 
		currentDict.Add	("onehandedleft", "왼손조작");
		currentDict.Add ("onehandedright", "오른손조작"); 
		currentDict.Add ("enabled", "On");
		currentDict.Add ("disabled", "Off"); 
		currentDict.Add ("cancel", "취소"); 
		currentDict.Add ("saveexit", "저장후종료"); 
		currentDict.Add ("off", "음소거");

		currentDict.Add ("resetstats", "초기화");
		currentDict.Add ("signout", "로그아웃");
		currentDict.Add ("signin", "로그인");

		// MESSAGES
		currentDict.Add ("unlocked", "열렸습니다");
		currentDict.Add ("unlock", "얼기"); 

		// MAIN GAME
		currentDict.Add ("recordingok", "녹화중");
		currentDict.Add ("recordingfailed", "");
		currentDict.Add ("ready", "준비");
		currentDict.Add ("score", "점수"); 
		currentDict.Add ("speed", "속도"); 
		currentDict.Add ("section", "스테이지"); 
		currentDict.Add ("newhighscore", "새로운 최고점수"); 
		currentDict.Add ("pause", "일시정지");
		currentDict.Add ("unpause", "다시 시작");
		currentDict.Add ("watchreplay", "리플레이 보기");
		currentDict.Add ("mainmenu", "메인메뉴");
		currentDict.Add ("restart", "다시 시작");
		currentDict.Add ("lives", "남은 비행사");
		currentDict.Add ("leaderboard", "나의 순위표");
		currentDict.Add ("scoreposted", "점수보내기");
		currentDict.Add ("scorepostfailed", "점수보내기 실패");

		// SHIP NAMES
		currentDict.Add ("ship0", "블루 나이트");
		currentDict.Add ("ship1", "그린 순양함");
		currentDict.Add ("ship2", "핑크 복수");
		currentDict.Add ("ship3", "초코민트 전투기");
		currentDict.Add ("ship4", "거장 IV"); 
		currentDict.Add ("ship5", "라디컬 폭격기") ;
		currentDict.Add ("ship6", "네오 번개"); 
		currentDict.Add ("ship7", "플래그 배");
		currentDict.Add ("ship8", "X 타이프");
		currentDict.Add ("ship9", "주문 설계");

		// CREDITS MENU
		currentDict.Add ("watchvideo", "광고보기");
		currentDict.Add ("loadingvideo", "로딩중..."); 
		currentDict.Add ("buygame", "게임 사기");
		currentDict.Add ("buygameinfo", "무제한 크레딧\n" + "하이퍼스피드모드 언락" + "\n" + GetWord ("ship9") + "를 언락");
		currentDict.Add ("watchvideoinfo", "" + GloVar.CreditsPerViewing + "크레딧 획득을\n위해 광고 보기");

		// SELECT SHIP MENU
		currentDict.Add ("defaultship", "기본 배");
		currentDict.Add ("select", "선택");

		// GOOGLE PLAY ENABLE MESSAGES
		currentDict.Add ("gpgsenableachievements", "Google Play Games가\n필요합니다");
		currentDict.Add ("gpgsenableleaderboards", "Google Play Games가\n필요합니다");
	}

	public void PopulateSpanish(){
		currentDict = new Dictionary<string,string>();

		// MAIN MENU
		currentDict.Add ("play", "Jugar");
		currentDict.Add ("quit", "Salir");
		currentDict.Add ("options", "Opciones");
		currentDict.Add ("selectship", "Seleccionar\nNave");
		currentDict.Add ("leaderboards", "Tablas de\nclasificación"); 
		currentDict.Add ("getcredits", "Obtener\ncréditos");
		currentDict.Add ("credits", "Créditos"); 
		currentDict.Add ("highscore", "Record"); 
		currentDict.Add ("achievements", "Logros");

		// MODES
		currentDict.Add ("normalmode", "Modo Normal"); 
		currentDict.Add ("normalslow", "Modo Normal (Inicio lento)");
		currentDict.Add ("normalfast", "Modo Normal (Inicio Rápido)");
		currentDict.Add ("slalommode", "Modo Eslalon");
		currentDict.Add ("spiralmode", "Modo Espiral"); 
		currentDict.Add ("hyperspeedmode", "Modo de Hipervelocidad");
		currentDict.Add ("threestrikesmode", "Modo de Tres Strikes");

		// MODE DESCRIPTIONS
		currentDict.Add ("normalslowdesc", "Modo normal con inicio lento\nPara principiantes");
		currentDict.Add ("normalfastdesc", "Modo Normal con inicio rápido");
		currentDict.Add ("slalomdesc", "Sólo eslalon\nBuen ejercicio para los dedos");
		currentDict.Add ("spiraldesc", "Sólo espirales\n¡Sin marearse!"); 
		currentDict.Add ("hyperspeeddesc", "¡Obstáculos sencillos a la velocidad de la luz!");
		currentDict.Add ("threestrikesdesc", "¡Tres strikes y estás fuera!") ;

		//OPTIONS
		currentDict.Add ("language", "Idioma");
		currentDict.Add ("musicvolume", "Volumen de la música");
		currentDict.Add ("audiovolume", "Volumen del audio");
		currentDict.Add ("control", "Control"); 
		currentDict.Add ("recording", "Grabar el juego"); 
		currentDict.Add ("twohanded", "Dos manos"); 
		currentDict.Add	("onehandedleft", "Mano izquierda");
		currentDict.Add ("onehandedright", "Mano derecha"); 
		currentDict.Add ("enabled", "Habilitado");
		currentDict.Add ("disabled", "Inhabilitado"); 
		currentDict.Add ("cancel", "Cancelar"); 
		currentDict.Add ("saveexit", "Guardar y\nVolver"); 
		currentDict.Add ("off", "Silencio");

		currentDict.Add ("resetstats", "Restablecer estadísticas");
		currentDict.Add ("signout", "Desconectarse");
		currentDict.Add ("signin", "Conectarse");

		// MESSAGES
		currentDict.Add ("unlocked", "desbloqueado");
		currentDict.Add ("unlock", "desbloquear"); 

		// MAIN GAME
		currentDict.Add ("recordingok", "Grabando juego");
		currentDict.Add ("recordingfailed", "");
		currentDict.Add ("ready", "PREPARADO");
		currentDict.Add ("score", "Puntaje"); 
		currentDict.Add ("speed", "Velocidad"); 
		currentDict.Add ("section", "Sección"); 
		currentDict.Add ("newhighscore", "¡Nuevo record!"); 
		currentDict.Add ("pause", "Pausa");
		currentDict.Add ("unpause", "Continuar");
		currentDict.Add ("watchreplay", "Ver\nRepetición");
		currentDict.Add ("mainmenu", "Menú\nPrincipal");
		currentDict.Add ("restart", "Reiniciar");
		currentDict.Add ("lives", "Vidas");
		currentDict.Add ("leaderboard", "Tabla de\nposiciones");
		currentDict.Add ("scoreposted", "Puntaje publicado");
		currentDict.Add ("scorepostfailed", "Falla en publicación de puntaje");

		// SHIP NAMES
		currentDict.Add ("ship0", "Caballero Azul");
		currentDict.Add ("ship1", "Crucero Novato");
		currentDict.Add ("ship2", "La Venganza Rosa");
		currentDict.Add ("ship3", "Guerrero de Chocomenta");
		currentDict.Add ("ship4", "El IV Maestro"); 
		currentDict.Add ("ship5", "Bombardero Radical") ;
		currentDict.Add ("ship6", "Neo Relámpago"); 
		currentDict.Add ("ship7", "El Almirante");
		currentDict.Add ("ship8", "La Nave X");
		currentDict.Add ("ship9", "Diseño personalizado");

		// CREDITS MENU
		currentDict.Add ("watchvideo", "Ver\nVideo");
		currentDict.Add ("loadingvideo", "Cargando..."); 
		currentDict.Add ("buygame", "Comprar juego");
		currentDict.Add ("buygameinfo", "Créditos Infinitos\nDesbloquear el Modo de Hipervelocidad\nDesbloquear Nave Personalizada");
		currentDict.Add ("watchvideoinfo", "Vea un breve anuncio para obtener " + GloVar.CreditsPerViewing + " créditos");

		// SELECT SHIP MENU
		currentDict.Add ("defaultship", "Su nave inicial predeterminada");
		currentDict.Add ("select", "Seleccionar");

		// GOOGLE PLAY ENABLE MESSAGES
		currentDict.Add ("gpgsenableachievements", "Habilitar Google Play Games para usar los Logros");
		currentDict.Add ("gpgsenableleaderboards", "Habilitar Google Play Games para usar las Tablas de clasificación");
	}

	public void PopulatePortuguese(){
		currentDict = new Dictionary<string,string>();

		// MAIN MENU
		currentDict.Add ("play", "Jogar");
		currentDict.Add ("quit", "Sair");
		currentDict.Add ("options", "Opções");
		currentDict.Add ("selectship", "Selecionar\nNave");
		currentDict.Add ("leaderboards", "Placar de\nLíderes"); 
		currentDict.Add ("getcredits", "Consiga\nCréditos");
		currentDict.Add ("credits", "Créditos"); 
		currentDict.Add ("highscore", "Maior Pontuação"); 
		currentDict.Add ("achievements", "Conquistas");

		// MODES
		currentDict.Add ("normalmode", "Modo Normal"); 
		currentDict.Add ("normalslow", "Normal (Começo Lento)");
		currentDict.Add ("normalfast", "Normal (Começo Rápido)");
		currentDict.Add ("slalommode", "Modo Zigue-Zague");
		currentDict.Add ("spiralmode", "Modo Espiral"); 
		currentDict.Add ("hyperspeedmode", "Modo Hipervelocidade");
		currentDict.Add ("threestrikesmode", "Modo Três Batidas");

		// MODE DESCRIPTIONS
		currentDict.Add ("normalslowdesc", "Modo normal com começo lento\nPara iniciantes");
		currentDict.Add ("normalfastdesc", "Modo normal com começo rápido");
		currentDict.Add ("slalomdesc", "Apenas zigue-zague\nBom exercício para os dedos");
		currentDict.Add ("spiraldesc", "Apenas espiral\nNão fique tonto!"); 
		currentDict.Add ("hyperspeeddesc", "Obstáculos simples na velocidade da luz!");
		currentDict.Add ("threestrikesdesc", "Três batidas e você está fora!") ;

		//OPTIONS
		currentDict.Add ("language", "Idioma");
		currentDict.Add ("musicvolume", "Música");
		currentDict.Add ("audiovolume", "Efeitos Sonoros");
		currentDict.Add ("control", "Controles"); 
		currentDict.Add ("recording", "Gravar Jogadas"); 
		currentDict.Add ("twohanded", "Duas Mãos"); 
		currentDict.Add	("onehandedleft", "Mão Esquerda");
		currentDict.Add ("onehandedright", "Mão Direita"); 
		currentDict.Add ("enabled", "Habilitado");
		currentDict.Add ("disabled", "Desabilitado"); 
		currentDict.Add ("cancel", "Cancelar"); 
		currentDict.Add ("saveexit", "Salvar &\nRetornar"); 
		currentDict.Add ("off", "Desligado");

		currentDict.Add ("resetstats", "Redefinir\nEstatísticas");
		currentDict.Add ("signout", "Sair");
		currentDict.Add ("signin", "Entrar");

		// MESSAGES
		currentDict.Add ("unlocked", "Desbloqueado");
		currentDict.Add ("unlock", "Desbloquear"); 

		// MAIN GAME
		currentDict.Add ("recordingok", "Gravando a Jogada");
		currentDict.Add ("recordingfailed", "");
		currentDict.Add ("ready", "PRONTO");
		currentDict.Add ("score", "Pontuação"); 
		currentDict.Add ("speed", "Velocidade"); 
		currentDict.Add ("section", "Seção"); 
		currentDict.Add ("newhighscore", "Novo Recorde"); 
		currentDict.Add ("pause", "Pausar");
		currentDict.Add ("unpause", "Retomar");
		currentDict.Add ("watchreplay", "Assistir\nReplay");
		currentDict.Add ("mainmenu", "Menu\nPrincipal");
		currentDict.Add ("restart", "Recomeçar");
		currentDict.Add ("lives", "Vidas");
		currentDict.Add ("leaderboard", "Placar de\nLíderes");
		currentDict.Add ("scoreposted", "Pontuação Publicada");
		currentDict.Add ("scorepostfailed", "Falha ao Publicar Pontuação");

		// SHIP NAMES
		currentDict.Add ("ship0", "Cavaleiro Azul");
		currentDict.Add ("ship1", "Cruzeiro Novato");
		currentDict.Add ("ship2", "Vingança Rosa");
		currentDict.Add ("ship3", "Lutador Chocomint");
		currentDict.Add ("ship4", "Meistre IV"); 
		currentDict.Add ("ship5", "Bombardeiro Radical") ;
		currentDict.Add ("ship6", "Relâmpago Neo"); 
		currentDict.Add ("ship7", "Almirante");
		currentDict.Add ("ship8", "Tipo X");
		currentDict.Add ("ship9", "Design Customizável");

		// CREDITS MENU
		currentDict.Add ("watchvideo", "Assista à\npublicidade");
		currentDict.Add ("loadingvideo", "Carregando..."); 
		currentDict.Add ("buygame", "Comprar Jogo");
		currentDict.Add ("buygameinfo", "Créditos Infinitos\nDesbloqueia Modo de Hipervelocidade\nDesbloqueia Nave Customizável");
		currentDict.Add ("watchvideoinfo", "Assista à uma pequena publicidade para ganhar " + GloVar.CreditsPerViewing + "  créditos");

		// SELECT SHIP MENU
		currentDict.Add ("defaultship", "Sua nave padrão de início");
		currentDict.Add ("select", "Selecionar");

		// GOOGLE PLAY ENABLE MESSAGES
		currentDict.Add ("gpgsenableachievements", "Permitir que o Google Play Games acesse as Conquistas");
		currentDict.Add ("gpgsenableleaderboards", "Permitir que o Google Play Games acesse o Placar de Líderes");
	}
}
