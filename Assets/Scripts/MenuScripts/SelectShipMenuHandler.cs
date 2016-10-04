using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;

public class SelectShipMenuHandler : MonoBehaviour {
	UnlockableObject[] unlockableShips;
	public int numberOfShips = 9;
	public Text shipNameLabel, unlockTextLabel, selectLabel;
	public Button selectButton;
	public GameObject shipGameObject, colorSliders;
	public float rotationStep;
	private int currentSelection;
	public Slider slider_C1R, slider_C1G, slider_C1B, slider_C2R, slider_C2G, slider_C2B, slider_C3R, slider_C3G, slider_C3B;
	private float C1R, C1G, C1B, C2R, C2G, C2B, C3R, C3G, C3B;
	ColorGroup currentColorGroup;
	ShipObject currentShipObject;
	MeshRenderer meshRenderer;
	LanguageHandler languageHandler;
	UnlockHandler unlockHandler;

	private float horizontalInput;
	private bool readyToReadHorizontalInput = true;


	public void Start(){
		unlockHandler = (UnlockHandler)GameObject.FindGameObjectWithTag ("UnlockHandler").GetComponent (typeof(UnlockHandler));
		languageHandler = (LanguageHandler)GameObject.FindGameObjectWithTag ("LanguageHandler").GetComponent (typeof(LanguageHandler));

		languageHandler.SetTextSettings (selectLabel, "select", FontSize.Medium);
		unlockableShips = unlockHandler.GetUnlockableShips ();
		numberOfShips = unlockableShips.Length + 1;
		currentSelection = PlayerPrefs.GetInt(GloVar.ShipModelChoicePrefName, 0);
		meshRenderer = shipGameObject.GetComponent<MeshRenderer> ();
		ReloadModel ();
	}

	public void Update(){
		if (Input.GetKeyDown(KeyCode.Escape)) { SceneManager.LoadScene(GloVar.MainMenuSceneName);; }
		shipGameObject.transform.Rotate (0, 0, -rotationStep);
		horizontalInput = Input.GetAxis ("Horizontal");
		if (readyToReadHorizontalInput){
			if (horizontalInput<0 && readyToReadHorizontalInput){
				readyToReadHorizontalInput = false;
				LeftClick ();
				Invoke ("EnableInput", 0.3f);
			}
			else if (horizontalInput>0){
				readyToReadHorizontalInput = false;
				RightClick ();
				Invoke ("EnableInput", 0.3f);
			}
		}
	}
	public void EnableInput(){
		readyToReadHorizontalInput = true;
	}

	public void SelectClick(){
		if (currentShipObject.ShipLangKey == "ship" + GloVar.CustomShipID) {
			if (PlayGamesPlatform.Instance.IsAuthenticated () == true) {
				if (Social.Active.localUser.authenticated == true) {
					Social.ReportProgress (GoogleConstants.achievement_spaceship_designer, 100.0f, (bool success) => {
						if (success) {
							Debug.Log ("Achievement unlocked");
						} else {
							Debug.Log ("Achievement unlock fail");
						}
					});
				}
			}

		}
		PlayerPrefs.SetInt (GloVar.ShipModelChoicePrefName, currentSelection);
		SceneManager.LoadScene(GloVar.MainMenuSceneName);
	}

	public void LeftClick(){
		currentSelection--;
		if (currentSelection<0){
			currentSelection = numberOfShips-1;
		}
		ReloadModel ();
	}
	public void RightClick(){
		currentSelection++;
		if (currentSelection >= numberOfShips){
			currentSelection = 0;
		}
		ReloadModel ();
	}

	public void ReloadModel(){
		currentShipObject = new ShipObject (currentSelection);
		if (currentShipObject.ShipLangKey == "ship" + GloVar.CustomShipID) {
			DisplayCustomColorsHUD (true);
		}
		else{
			DisplayCustomColorsHUD (false);
		}
		currentColorGroup = currentShipObject.ColorGroup;
		meshRenderer.materials [0].SetColor ("_Color", currentColorGroup.Color1);
		meshRenderer.materials [1].SetColor ("_Color", currentColorGroup.Color2);
		meshRenderer.materials [2].SetColor ("_Color", currentColorGroup.Color3);

		languageHandler.SetTextSettings (shipNameLabel, "ship" + currentSelection, FontSize.Large);


		unlockTextLabel.text = languageHandler.GetShipUnlockDescription (currentShipObject);
		unlockTextLabel.font = languageHandler.GetFont ();
		unlockTextLabel.fontSize = languageHandler.GetFontSize (FontSize.Medium);
		//unlockTextLabel.text = currentShipObject.UnlockText;
		CheckUnlocked ();
	}

	public void DisplayCustomColorsHUD (bool enable){
		colorSliders.SetActive (enable);
		if (enable){
			slider_C1R.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor1RPrefName, 0.4f);
			slider_C1B.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor1BPrefName, 0.4f);
			slider_C1G.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor1GPrefName, 0.4f);
			slider_C2R.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor2RPrefName, 0.4f);
			slider_C2B.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor2BPrefName, 0.4f);
			slider_C2G.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor2GPrefName, 0.4f);
			slider_C3R.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor3RPrefName, 0.4f);
			slider_C3B.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor3BPrefName, 0.4f);
			slider_C3G.value = PlayerPrefs.GetFloat (GloVar.CustomShipColor3GPrefName, 0.4f);
		}
	}

	public void CheckUnlocked(){
		if (currentSelection>0 && !unlockHandler.IsShipUnlocked (currentSelection)){
			selectButton.gameObject.SetActive (false);
		}
		else{
			selectButton.gameObject.SetActive (true);
		}
	}

	public void SlideC1R(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor1RPrefName, slider_C1R.normalizedValue);
		ReloadModel ();
	}
	public void SlideC1G(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor1GPrefName, slider_C1G.normalizedValue);
		ReloadModel ();
	}
	public void SlideC1B(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor1BPrefName, slider_C1B.normalizedValue);
		ReloadModel ();
	}
	public void SlideC2R(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor2RPrefName, slider_C2R.normalizedValue);
		ReloadModel ();
	}
	public void SlideC2G(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor2GPrefName, slider_C2G.normalizedValue);
		ReloadModel ();
	}
	public void SlideC2B(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor2BPrefName, slider_C2B.normalizedValue);
		ReloadModel ();
	}
	public void SlideC3R(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor3RPrefName, slider_C3R.normalizedValue);
		ReloadModel ();
	}
	public void SlideC3G(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor3GPrefName, slider_C3G.normalizedValue);
		ReloadModel ();
	}
	public void SlideC3B(){
		PlayerPrefs.SetFloat (GloVar.CustomShipColor3BPrefName, slider_C3B.normalizedValue);
		ReloadModel ();
	}
}

