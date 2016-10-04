public enum PlayerControlType{
	TwoHanded=0, OneHandedLeft=1, OneHandedRight=2
}

public static class PlayerControlTypeExtension{
	public static string ToLangKey(this PlayerControlType playerControlType){
		switch(playerControlType){
		case PlayerControlType.TwoHanded:
			return "twohanded";
		case PlayerControlType.OneHandedLeft:
			return "onehandedleft";
		case PlayerControlType.OneHandedRight:
			return "onehandedright";
		}
		return null;
	}
}