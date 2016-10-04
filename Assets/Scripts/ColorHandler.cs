using UnityEngine;
using System.Collections;

public static class ColorHandler {

	public static ColorGroup getColorGroup(int i){
		switch(i){
		case GloVar.DefaultShipID: 
			return new ColorGroup (new Color32 (0, 7, 66, 255), new Color32 (7, 7, 7, 255), new Color32 (126, 126, 126, 255));
		case GloVar.GreenShipID:
			return new ColorGroup (new Color32 (0, 54, 7, 255), new Color32 (6, 17, 7, 255), new Color32 (12, 38, 27, 255));
		case GloVar.PinkShipID:
			return new ColorGroup (new Color32 (31, 9, 45, 255), new Color32 (49, 27, 48, 255), new Color32 (231, 68, 199, 255));
		case GloVar.MintShipID:
			return new ColorGroup (new Color32 (17, 11, 11, 255), new Color32 (12, 30, 18, 255), new Color32 (79, 143, 92, 255));
		case GloVar.BlackRedShipID:
			return new ColorGroup (new Color32 (4, 4, 4, 255), new Color32 (6, 6, 6, 255), new Color32 (101, 0, 0, 255));

		case GloVar.PurpleYellowShip:
			return new ColorGroup (new Color32 (80, 77, 30, 255), new Color32 (13, 4, 12, 255), new Color32 (118, 35, 131, 255));
		case GloVar.AquaYellowShipID:
			return new ColorGroup (new Color32 (4, 44, 60, 255), new Color32 (22, 23, 14, 255), new Color32 (184 , 170, 95, 255));

		case GloVar.RedWhiteShipID:
			return new ColorGroup (new Color32 (83, 83, 83, 255), new Color32 (34, 34, 34, 255), new Color32 (122, 0, 0, 255));
		case GloVar.TealShipID:
			return new ColorGroup (new Color32 (59, 131, 125, 255), new Color32 (13, 43, 49, 255), new Color32 (16, 59, 68, 255));

		case GloVar.CustomShipID:
			float c1r = PlayerPrefs.GetFloat (GloVar.CustomShipColor1RPrefName, 0.4f);
			float c1g = PlayerPrefs.GetFloat (GloVar.CustomShipColor1GPrefName, 0.4f);
			float c1b = PlayerPrefs.GetFloat (GloVar.CustomShipColor1BPrefName, 0.4f);
			float c2r = PlayerPrefs.GetFloat (GloVar.CustomShipColor2RPrefName, 0.4f);
			float c2g = PlayerPrefs.GetFloat (GloVar.CustomShipColor2GPrefName, 0.4f);
			float c2b = PlayerPrefs.GetFloat (GloVar.CustomShipColor2BPrefName, 0.4f);
			float c3r = PlayerPrefs.GetFloat (GloVar.CustomShipColor3RPrefName, 0.4f);
			float c3g = PlayerPrefs.GetFloat (GloVar.CustomShipColor3GPrefName, 0.4f);
			float c3b = PlayerPrefs.GetFloat (GloVar.CustomShipColor3BPrefName, 0.4f);
			return new ColorGroup (new Color (c1r, c1g, c1b), new Color (c2r, c2g, c2b), new Color (c3r, c3g, c3b));
		default:
			return null;
		}
	}
}