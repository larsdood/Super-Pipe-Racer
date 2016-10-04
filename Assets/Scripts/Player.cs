using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class Player : MonoBehaviour {
	RecordReplaySettingType recordSetting;
	PlayerControlType controlType;
	AudioHandler audioHandler;
	public SFXHandler sfxHandler;
	UnlockHandler unlockHandler;
	private bool isSFXMuted;
	private bool isRecording;
	private bool isPaused;
	public bool testPoints =false;
	private int oldscore;
	private bool alive;
	private bool controllable = false;
	public GameMode gameMode;
	public Avatar avatar;
	private int lives;

	private float dyingSpeed;

	private bool highscorePassed = false;

	public PipeSystem pipeSystem;
	public float startVelocitySlowRatio;
	public float startVelocityFastRatio;
	public float maxVelocity1Ratio;
	public float maxVelocity2Ratio;
	public float hyperSpeedVelocityRatio;
	public float maxVelocityAbsolute;
	public float acceleration;
	private float tempAcceleration;
	private float maxVelocity;
	private float velocity;
	private float tempVelocity;
	private Pipe currentPipe;
	private float score;
	private float deltaToRotation;
	private float systemRotation;
	private Transform world, rotater;
	private bool noScore;
	private float highscore;

	private float velocityScoreFactor;

	private float worldRotation, avatarRotation;

	public float rotationVelocity;

	public HUD hud;

	private int currentSection;

	// Use this for initialization
	void Start () {
		controlType = (PlayerControlType)PlayerPrefs.GetInt (GloVar.PlayerControlChoicePrefName, 0);
		unlockHandler = (UnlockHandler)GameObject.FindGameObjectWithTag ("UnlockHandler").GetComponent (typeof(UnlockHandler));
		audioHandler = (AudioHandler)GameObject.FindGameObjectWithTag ("MusicPlayer").GetComponent (typeof(AudioHandler));
		audioHandler.PlayNormalMode ();
		isSFXMuted = PlayerPrefs.GetFloat (GloVar.AudioVolumePrefName, 1f) == 0;
		isPaused = false;
		hud.SetPauseButtonActive (false);
		unlockHandler.StoreUnlockProgress ();
		velocityScoreFactor = 100f/maxVelocityAbsolute;
		startVelocityFastRatio *= maxVelocityAbsolute;
		startVelocitySlowRatio *= maxVelocityAbsolute;
		maxVelocity1Ratio *= maxVelocityAbsolute;
		maxVelocity2Ratio *= maxVelocityAbsolute;
		hyperSpeedVelocityRatio *= maxVelocityAbsolute;


		gameMode = (GameMode)PlayerPrefs.GetInt (GloVar.GameModePrefName);
		alive = true;
		maxVelocity = maxVelocity1Ratio;
		switch(gameMode){
		case GameMode.Normal_SlowStart:
		case GameMode.Normal_FastStart:
			highscore = oldscore = PlayerPrefs.GetInt (GloVar.NormalModeHighscorePrefName, 0);
			break;
		case GameMode.ThreeStrikes:
			lives = 3;
			hud.SetLives (lives);
			highscore = oldscore = PlayerPrefs.GetInt (GloVar.ThreeStrikesModeHighscorePrefName, 0);
			break;
		case GameMode.Slalom:
			highscore = oldscore = PlayerPrefs.GetInt (GloVar.SlalomModeHighscorePrefName, 0);
			break;
		case GameMode.Spiral:
			highscore = oldscore = PlayerPrefs.GetInt (GloVar.SpiralModeHighscorePrefName, 0);
			break;
		case GameMode.HyperSpeed:
			highscore = oldscore = PlayerPrefs.GetInt (GloVar.HyperSpeedModeHighscorePrefName, 0);
			break;
		default:
			Debug.Log ("Error: Couldn't determinte game mode for high score load");
			highscore = oldscore = PlayerPrefs.GetInt (GloVar.NormalModeHighscorePrefName, 0);
			break;
		}

		Application.targetFrameRate = 60;
		noScore = true;
		if (gameMode == GameMode.Normal_SlowStart){
			velocity = startVelocitySlowRatio;
		}
		else if (gameMode == GameMode.HyperSpeed){
			velocity = hyperSpeedVelocityRatio;
		}
		else{
			velocity = startVelocityFastRatio;
		}
		currentSection = 0;
		world = pipeSystem.transform.parent;
		rotater = transform.GetChild (0);
		currentPipe = pipeSystem.SetupFirstPipe (this);
		SetupCurrentPipe ();

		recordSetting = (RecordReplaySettingType)PlayerPrefs.GetInt(GloVar.RecordReplayChoicePrefName, 0);
		hud.SetValues (highscore, score, velocity, currentSection);
		if (recordSetting == RecordReplaySettingType.Disabled){
			isRecording = false;
		}
		else {
			if(ReplayHandler.BeginRecording ()){
				hud.CanRecord ();
				isRecording = true;
			}
			else{
				hud.CantRecord ();
				isRecording = false;
			}
		}

	}

	public void Pause(){
		if (alive){
			if (!isPaused){
				if (isRecording){
					ReplayHandler.PauseRecording ();
				}
				audioHandler.PauseMusic ();
				sfxHandler.Mute(true);
				sfxHandler.StopTurningSFX ();
				avatar.TogglePauseVisualEffects (true);
				tempVelocity = velocity;
				velocity = 0f;
				tempAcceleration = acceleration;
				acceleration = 0f;
				isPaused = true;
			}
			else{
				if (isRecording){
					ReplayHandler.ResumeRecording ();
				}
				audioHandler.UnpauseMusic ();
				sfxHandler.Mute (false);
				avatar.TogglePauseVisualEffects (false);
				velocity = tempVelocity;
				acceleration = tempAcceleration;
				isPaused = false;
			}
		}

	}

	public void SetAvatar(Avatar avatar){
		this.avatar = avatar;
	}
	
	// Update is called once per frame
	void Update () {
		/*
		if (!isRecording && alive && recordSetting != RecordReplaySettingType.Disabled){
			if(ReplayHandler.BeginRecording ()){
				hud.CanRecord ();
				isRecording = true;
			}
			else{
				hud.CantRecord ();
				isRecording = false;
			}
		}*/
		if (gameMode != GameMode.HyperSpeed){
			if (velocity < maxVelocity && !noScore){
				velocity += acceleration * Time.deltaTime;
			}
			else if (velocity > maxVelocity){
				velocity = maxVelocity;
			}
		}
		float delta = velocity * Time.deltaTime;
		if (!noScore && alive)
			score += (delta*10);
		if (testPoints)
			score += (delta * 100);
		if(!isPaused){
			systemRotation += delta * deltaToRotation;
		}
		if (systemRotation >= currentPipe.CurveAngle) {
			delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
			currentPipe = pipeSystem.SetupNextPipe ();
			SetupCurrentPipe ();
			systemRotation = delta * deltaToRotation;
		}
		pipeSystem.transform.localRotation = 
			Quaternion.Euler (0f, 0f, systemRotation);

		UpdateAvatarRotation ();
		if (alive && !isPaused){
			if (score >= highscore){
				if (!highscorePassed){
					if (oldscore != 0) {
						hud.NewHighscoreReached ();
						if (!isSFXMuted)
							sfxHandler.PlayHighscoreSFX ();
					}
					highscorePassed = true;
				}
				highscore = score;

			}

			hud.SetValues (highscore, score, velocity*velocityScoreFactor, currentSection);	
		}
	}

	private void UpdateAvatarRotation(){
		if (controllable && !isPaused){
			float rotationInput = 0f;

			if (Application.isMobilePlatform) {
				if (controlType != PlayerControlType.TwoHanded){
					rotationInput = hud.GetMoveButtonsDirection ();
				}
				else if (Input.touchCount == 1){
					if (Input.GetTouch (0).position.x < Screen.width * 1f/3f) {
						rotationInput = -1f;
					} else if (Input.GetTouch(0).position.x > Screen.width*2f/3f)
						rotationInput = 1f;
				}/*

				else {
					rotationInput = Input.acceleration.x*3;
					if (Input.acceleration.x < -0.05f){
						rotationInput = -1f;
					}
					else if (Input.acceleration.x > 0.05f){
						rotationInput = 1f;
					}
				}*/
			}
			else{
				rotationInput = Input.GetAxis ("Horizontal");
				if (controlType != PlayerControlType.TwoHanded){
					rotationInput = hud.GetMoveButtonsDirection ();
				}
			}

			if (rotationInput > 1f) {
				rotationInput = 1f;
			} else if (rotationInput < -1f) {
				rotationInput = -1f;
			}

			if (!isSFXMuted){
				if (alive) {
					if (rotationInput == 0) {
						sfxHandler.PlayMiddleTurningSFX ();
					} else if (rotationInput > 0f) {
						sfxHandler.PlayRightTurningSFX ();
					} else if (rotationInput < 0f) {
						sfxHandler.PlayLeftTurningSFX ();
					}
				} else
					sfxHandler.StopTurningSFX ();
			}
			avatarRotation += rotationVelocity * Time.deltaTime * rotationInput;
			if (avatarRotation < 0f)
				avatarRotation += 360f;
			else if (avatarRotation > 360f)
				avatarRotation -= 360f;
			rotater.localRotation = Quaternion.Euler (avatarRotation, 0f, 0f);
			avatar.SetRotation (rotationInput);
			//avatar.SetRotation (rotationInput== 0f ? 0 : rotationInput>0f ? 1 : -1); 
		}
	}

	private void SetupCurrentPipe(){
		deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
		worldRotation += currentPipe.RelativeRotation;
		if (worldRotation < 0f){
			worldRotation += 360f;
		}
		else if (worldRotation >= 360f) {
			worldRotation -= 360f;
		}
		world.localRotation = Quaternion.Euler (worldRotation, 0f, 0f);
	}




	public void SetSection(int section){
		currentSection = section;
		if (section >1){
			
			hud.SectionReached (section);
		}

	}
	public void SetNoScore(bool noScore){
		this.noScore = noScore; 
		if (!noScore){
			hud.SetPauseButtonActive (false);
			hud.SectionClear ();
		}
		else{
			if(currentSection>1 && alive){
				if (!isSFXMuted)
					sfxHandler.PlaySectionSFX ();
				hud.SetPauseButtonActive (true);
			}

		}
	}
	public void ShowSectionOneHud(){
		if (!isSFXMuted)
			sfxHandler.PlayStartSFX ();
		hud.SectionReached (1); 
		hud.ShowArrows ();
	}
	public void Die(){
		if (gameMode==GameMode.ThreeStrikes && lives >= 1){
			alive = true;
			avatar.Ressurrect ();
			velocity = tempVelocity;
			acceleration = tempAcceleration;
			if (!isSFXMuted)
				sfxHandler.PlayStartSFX ();
			hud.ResetArrowCounter ();
			hud.ShowArrows ();
		}
		else{
			velocity = 0f;
			acceleration = 0f;
			controllable = false;
			PlayerPrefs.SetInt(GloVar.DeathsInSectionPrefixPrefName + currentSection + gameMode.ToUnlockKey (), 
				PlayerPrefs.GetInt(GloVar.DeathsInSectionPrefixPrefName + currentSection + gameMode.ToUnlockKey (), 0)+1);
			int furthestSection = PlayerPrefs.GetInt (GloVar.FurthestSectionPrefixPrefName + gameMode.ToUnlockKey (), 0);
			if (currentSection>furthestSection){
				PlayerPrefs.SetInt (GloVar.FurthestSectionPrefixPrefName + gameMode.ToUnlockKey (), currentSection);
			}
			ReplayHandler.StopRecording ();
			ReplayHandler.SetMetaData (gameMode, (int)score, currentSection);
			hud.ShowMenu ();
			hud.FinalScoreDisplay ((int)(score), highscorePassed);
			hud.ShowUnlockText (unlockHandler.CheckForUnlocks (hud.GetLanguageHandler()));
		}
	}

	public void Dying(){
		if (gameMode==GameMode.ThreeStrikes){
			lives--;
			hud.SetLives (lives);
		}
		if (!isSFXMuted)
			sfxHandler.PlayCrashSFX ();
		dyingSpeed = velocity / 1f;
		alive = false;
		if (gameMode==GameMode.ThreeStrikes && lives>=1){
			tempVelocity = velocity;
			tempAcceleration = acceleration;
			return;
		}
		switch(gameMode){
		case GameMode.Normal_SlowStart:
		case GameMode.Normal_FastStart:
			PlayerPrefs.SetInt (GloVar.NormalModeHighscorePrefName, (int)(highscore));
			break;
		case GameMode.ThreeStrikes:
			PlayerPrefs.SetInt (GloVar.ThreeStrikesModeHighscorePrefName, (int)(highscore));
			break;
		case GameMode.Slalom:
			PlayerPrefs.SetInt (GloVar.SlalomModeHighscorePrefName, (int)(highscore));
			break;
		case GameMode.Spiral:
			PlayerPrefs.SetInt (GloVar.SpiralModeHighscorePrefName, (int)(highscore));
			break;
		case GameMode.HyperSpeed:
			PlayerPrefs.SetInt (GloVar.HyperSpeedModeHighscorePrefName, (int)(highscore));
			break;
		default:
			Debug.Log ("Error: Could not determine gamemode for posting highscore");
			break;
		}
		if (PlayGamesPlatform.Instance.IsAuthenticated () == true){
			switch(gameMode){
			case GameMode.Normal_SlowStart:
			case GameMode.Normal_FastStart:
				Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQBg", (bool success) => {
					if (success){
						hud.ScorePosted(true);
						Debug.Log ("Score posted to leaderboard");
					}
					else if (!success){
						Debug.Log("Attempt 1: Score failed to post");
						Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQBg", (bool success2) => {
							if (success2){
								hud.ScorePosted(true);
								Debug.Log ("Score posted to leaderboard");
							}
							else if (!success2){
								hud.ScorePosted(false);
								Debug.Log("Attempt 2: Score failed to post");
							}
						});
					}
				});
				break;
			case GameMode.ThreeStrikes:
				Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQCg", (bool success) => {
					if (success){
						hud.ScorePosted(true);
						Debug.Log ("Score posted to leaderboard");
					}
					else if (!success){
						Debug.Log("Attempt 1: Score failed to post");
						Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQCg", (bool success2) => {
							if (success2){
								hud.ScorePosted(true);
								Debug.Log ("Score posted to leaderboard");
							}
							else if (!success2){
								hud.ScorePosted(false);
								Debug.Log("Attempt 2: Score failed to post");
							}
						});
					}
				});
				break;
			case GameMode.Slalom:
				Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQCA", (bool success) => {
					if (success){
						hud.ScorePosted(true);
						Debug.Log ("Score posted to leaderboard");
					}
					else if (!success){
						Debug.Log("Attempt 1: Score failed to post");
						Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQCA", (bool success2) => {
							if (success2){
								hud.ScorePosted(true);
								Debug.Log ("Score posted to leaderboard");
							}
							else if (!success2){
								hud.ScorePosted(false);
								Debug.Log("Attempt 2: Score failed to post");
							}
						});
					}
				});
				break;
			case GameMode.Spiral:
				Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQBw", (bool success) => {
					if (success){
						hud.ScorePosted(true);
						Debug.Log ("Score posted to leaderboard");
					}
					else if (!success){
						Debug.Log("Attempt 1: Score failed to post");
						Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQBw", (bool success2) => {
							if (success2){
								hud.ScorePosted(true);
								Debug.Log ("Score posted to leaderboard");
							}
							else if (!success2){
								hud.ScorePosted(false);
								Debug.Log("Attempt 2: Score failed to post");
							}
						});
					}
				});
				break;

			case GameMode.HyperSpeed:
				Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQCQ", (bool success) => {
					if (success){
						hud.ScorePosted(true);
						Debug.Log ("Score posted to leaderboard");
					}
					else if (!success){
						Debug.Log("Attempt 1: Score failed to post");
						Social.ReportScore ((long)score, "CgkIyOLKsZgIEAIQCQ", (bool success2) => {
							if (success2){
								hud.ScorePosted(true);
								Debug.Log ("Score posted to leaderboard");
							}
							else if (!success2){
								hud.ScorePosted(false);
								Debug.Log("Attempt 2: Score failed to post");
							}
						});
					}
				});
				break;
				break;
			default:
				Debug.Log ("Error: Couldn't determine game mode for high score save");
				break;
			}
		}
	}

	public void SpeedDown(float time){
		velocity -= dyingSpeed *time;
	}
	public void SetMaxVel2(){
		maxVelocity = maxVelocity2Ratio;
	}
	public void SetMaxVel3(){
		maxVelocity = maxVelocityAbsolute;
	}

	public GameMode GetGameMode(){
		return gameMode;
	}
	public void Controllable(){
		controllable = true;
	}
}
