public enum GameMode {
	Normal_SlowStart=0, Normal_FastStart=1, ThreeStrikes=2, Spiral=3, Slalom=4, HyperSpeed=5
}

public static class GameModeExtensions{
	public static string ToMenuKey(this GameMode gameMode){
		switch(gameMode){
		case GameMode.Normal_SlowStart:
			return "normalslow";
		case GameMode.Normal_FastStart:
			return "normalfast";
		case GameMode.ThreeStrikes:
			return "threestrikesmode";
		case GameMode.Slalom:
			return "slalommode";
		case GameMode.Spiral:
			return "spiralmode";
		case GameMode.HyperSpeed:
			return "hyperspeedmode";
		default:
			return "ERROR! Failed to retrieve game mode name";
		}
	}
	public static string ToUnlockKey(this GameMode gameMode){
		switch(gameMode){
		case GameMode.Normal_SlowStart:
			goto case GameMode.Normal_FastStart;
		case GameMode.Normal_FastStart:
			return "normalmode";
		case GameMode.ThreeStrikes:
			return "threestrikesmode";
		case GameMode.Spiral:
			return "spiralmode";
		case GameMode.Slalom:
			return "slalommode";
		case GameMode.HyperSpeed:
			return "hyperspeedmode";
		default:
			return "";
		}
	}
	public static string ToUnlockedKey(this GameMode gameMode){
		switch(gameMode){
		case GameMode.Normal_SlowStart:
			goto case GameMode.Normal_FastStart;
		case GameMode.Normal_FastStart:
			return "normalfast";
		case GameMode.ThreeStrikes:
			return "threestrikesmode";
		case GameMode.Spiral:
			return "spiralmode";
		case GameMode.Slalom:
			return "slalommode";
		case GameMode.HyperSpeed:
			return "hyperspeedmode";
		default:
			return "";
		}
	}
}