public enum RecordReplaySettingType{
	Enabled=1, Disabled=0
}

public static class RecordReplaySettingTypeExtension {
	public static string ToLangKey(this RecordReplaySettingType recordReplaySettingType){
		switch(recordReplaySettingType){
		case RecordReplaySettingType.Disabled:
			return "disabled";
		case RecordReplaySettingType.Enabled:
			return "enabled";
		}
		return null;
	}
}