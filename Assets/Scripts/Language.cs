public enum Language{
	English=0, Spanish = 1, Portuguese = 2, French = 3, Korean = 4, Japanese = 5, Chinese = 6
}
public static class LanguageExtension{
	public static string GetLanguageName(this Language language){
		switch (language) {
		case Language.English:
			return "English";
		case Language.Spanish:
			return "Español";
		case Language.Portuguese:
			return "Português";
		case Language.French:
			return "Français";
		case Language.Korean:
			return "한국어";
		case Language.Japanese:
			return "日本語";
		case Language.Chinese:
			return "中文";
		
		default:
			return "";
		}
	}
}