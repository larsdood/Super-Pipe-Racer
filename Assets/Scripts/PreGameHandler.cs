using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PreGameHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameMode gameMode = (GameMode)PlayerPrefs.GetInt (GloVar.GameModePrefName);
		if (PlayerPrefs.GetInt(GloVar.GamePurchasedPrefName, 0)==0){
			if (gameMode==GameMode.ThreeStrikes){
				if (CreditsHandler.IsAtLeastX (GloVar.CreditsPerThreeStrikesGame)){
					CreditsHandler.ReduceByX (GloVar.CreditsPerThreeStrikesGame);
					SceneManager.LoadScene (GloVar.MainGameSceneName);
				}
				else{
					SceneManager.LoadScene (GloVar.CreditsSceneName);
				}
			}
			else if (CreditsHandler.IsAtLeastX (GloVar.CreditsPerGame)){
				CreditsHandler.ReduceByX (GloVar.CreditsPerGame);
				SceneManager.LoadScene (GloVar.MainGameSceneName);
			}
			else{
				SceneManager.LoadScene (GloVar.CreditsSceneName);
			}
		}
		else{
			SceneManager.LoadScene (GloVar.MainGameSceneName);
		}
	}
}
