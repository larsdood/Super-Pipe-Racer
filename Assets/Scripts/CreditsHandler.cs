using UnityEngine;
public static class CreditsHandler {
	public static void ReduceByX(int x){
		PlayerPrefs.SetInt (GloVar.CreditsPrefName, PlayerPrefs.GetInt (GloVar.CreditsPrefName, GloVar.InitialCredits)-x);
	}
	public static bool IsAtLeastX(int x){
		return PlayerPrefs.GetInt (GloVar.CreditsPrefName, GloVar.InitialCredits) >= x;
	}
	public static void IncreaseByX(int x){
		PlayerPrefs.SetInt (GloVar.CreditsPrefName, PlayerPrefs.GetInt (GloVar.CreditsPrefName, GloVar.InitialCredits) + x);
	}
}