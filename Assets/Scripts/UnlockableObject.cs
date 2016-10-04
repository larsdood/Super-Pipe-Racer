using UnityEngine;
using System.Collections;

public class UnlockableObject{
	private UnlockableObjectType objectType;
	private UnlockCriteriaType criteriaType;
	private GameMode gameMode;
	private int score;
	private int section;
	private int times;
	private int objectID;

	public UnlockableObject(UnlockHandler unlockHandler, UnlockableObjectType objectType, int objectID, UnlockCriteriaType criteriaType, GameMode gameMode, int x, int y = 0){
		this.objectID = objectID;
		this.objectType = objectType;
		this.criteriaType = criteriaType;
		this.gameMode = gameMode;
		switch(criteriaType){
		case UnlockCriteriaType.ScoreXPoints:
			score = x;
			break;
		case UnlockCriteriaType.DieXTimesYSection:
			times = x;
			section = y;
			break;
		case UnlockCriteriaType.SurviveXTimesYSection:
			times = x;
			section = y;
			break;
		case UnlockCriteriaType.BuyGame:
			break;
		default:
			Debug.Log ("Error: Not valid unlock type for generating unlock requirement");
			break;
		}
		unlockHandler.AddToUnlockables (this);
	}

	public bool IsUnlocked(){
		switch (criteriaType) {
		case UnlockCriteriaType.ScoreXPoints:
			switch (gameMode) {
			case GameMode.Normal_SlowStart:
				goto case GameMode.Normal_FastStart;
			case GameMode.Normal_FastStart:
				return PlayerPrefs.GetInt (GloVar.NormalModeHighscorePrefName, 0) >= score;
			case GameMode.Spiral:
				return PlayerPrefs.GetInt (GloVar.SpiralModeHighscorePrefName, 0) >= score;
			case GameMode.Slalom:
				return PlayerPrefs.GetInt (GloVar.SlalomModeHighscorePrefName, 0) >= score;
			case GameMode.ThreeStrikes:
				return PlayerPrefs.GetInt (GloVar.ThreeStrikesModeHighscorePrefName, 0) >= score;
			case GameMode.HyperSpeed:
				return PlayerPrefs.GetInt (GloVar.HyperSpeedModeHighscorePrefName, 0) >= score;
			}
			break;
		case UnlockCriteriaType.DieXTimesYSection:
			switch (gameMode) {
			default:
				return PlayerPrefs.GetInt (GloVar.DeathsInSectionPrefixPrefName + section + gameMode.ToUnlockKey (), 0 ) >= times;
			}
		case UnlockCriteriaType.SurviveXTimesYSection:
			return StatsGenerator.TimesDiedAfterSection (gameMode, section-1)>=times;
		case UnlockCriteriaType.BuyGame:
			return PlayerPrefs.GetInt (GloVar.GamePurchasedPrefName, 0) == 1 ? true : false;
		}

		return false;
	}

	public int GetTimes(){
		return times;
	}
	public int GetScore(){
		return score;
	}
	public int GetSection(){
		return section;
	}
	public GameMode GetGameMode(){
		return gameMode;
	}

	public UnlockableObjectType GetObjectType(){
		return objectType;
	}

	public UnlockCriteriaType GetCriteriaType(){
		return criteriaType;
	}

	public int GetObjectID(){
		return objectID;
	}
}