using UnityEngine;
using System.Collections;

public static class StatsGenerator{

	public static int TimesDiedAfterSection(GameMode gameMode, int section){
		int sum = 0;
		int furthestSection = PlayerPrefs.GetInt (GloVar.FurthestSectionPrefixPrefName + gameMode.ToUnlockKey());
		if (section >= furthestSection)
			return 0;
		for (int i = section+1; i <= furthestSection; i++) {
			sum += PlayerPrefs.GetInt (GloVar.DeathsInSectionPrefixPrefName + i + gameMode.ToUnlockKey ());
		}
		return sum;
	}
}