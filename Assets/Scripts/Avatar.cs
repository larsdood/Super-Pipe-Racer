using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;

public class Avatar : MonoBehaviour {

	public ShipModel shipModel;
	private GameObject go_ShipModel;

	public Camera camera;
	private ColorCorrectionCurves colCorCur;

	public float maxRotation = 20f;
	public float rotationStep = 30f;
	private float cameraFastScale = 2f;
	public ParticleSystem trail, burst;

	private Player player;

	private float currentRotation;
	private bool invincible;

	private void Awake(){
		invincible = false;
		go_ShipModel = shipModel.gameObject;
		shipModel.SetMaterials (trail);
		player = transform.root.GetComponent<Player>();
		player.SetAvatar (this);
		currentRotation = 0;
		colCorCur = camera.GetComponent<ColorCorrectionCurves> ();
		colCorCur.saturation = 0;
		GameMode gameMode = (GameMode)PlayerPrefs.GetInt (GloVar.GameModePrefName, 0);
		if (gameMode == GameMode.Normal_SlowStart){
			cameraFastScale = 1f;
			camera.transform.Translate (-2.5f, 0.5f, 0, Space.World);
		}
		else{
			
			cameraFastScale = 2f;
			camera.transform.Translate (-2.5f, 0.5f, 0, Space.World);
		}
	}

	private void MoveCamera(){
		camera.transform.Translate (0.5f * Time.deltaTime*cameraFastScale, -0.09f* Time.deltaTime*cameraFastScale, 0, Space.World);
		colCorCur.saturation += 0.003f * cameraFastScale;

	}

	public float deathCountDown = -1f;



	private void OnTriggerEnter(Collider collider){
		if (deathCountDown<0f && !invincible){
			player.Dying ();
			trail.Stop();
			burst.Emit (burst.maxParticles);
			deathCountDown = burst.startLifetime;
		}
	}
	public void Ressurrect(){
		invincible = true;
		colCorCur.saturation = 1f;
		colCorCur.enabled = false;
		ToggleDisplayShip ();
		trail.Play ();

		Invoke ("ToggleDisplayShip", 0.3f); //disable
		Invoke ("ToggleDisplayShip", 0.6f); //enable
		Invoke ("ToggleDisplayShip", 0.9f); //disable
		Invoke ("ToggleDisplayShip", 1.2f); //enable
		Invoke ("ToggleDisplayShip", 1.5f); //disable
		Invoke ("ToggleDisplayShip", 1.8f); //enable
		Invoke ("ToggleInvincible", 2.0f);
	}
	private void ToggleInvincible(){
		invincible = !invincible;
	}

	public void ToggleDisplayShip(){
		go_ShipModel.SetActive (!go_ShipModel.activeSelf);
	}


	private void Update(){
		if (camera.transform.position.x < -1.25f)
			MoveCamera ();
		else if (camera.transform.localPosition.x > -1.25f){
			colCorCur.saturation = 1f;
			colCorCur.enabled = false;
			Vector3 temp = camera.transform.localPosition;
			temp.x = -1.25f;
			camera.transform.localPosition = temp;
			player.Controllable ();
			player.ShowSectionOneHud ();
			player.SetSection (1);
		}
		if (deathCountDown >= 0f){
			deathCountDown -= Time.deltaTime;
			player.SpeedDown (Time.deltaTime);
			colCorCur.enabled = true;
			if (colCorCur.saturation>0f){
				colCorCur.saturation -= 0.015f;
			}
			if (colCorCur.saturation>3f){
				colCorCur.saturation = 0;
			}
			if (deathCountDown<=0f){
				deathCountDown = -1f;
				go_ShipModel.SetActive (false);
				player.Die();
			}
		}
	}

	public void SetRotation(float direction){
		
		currentRotation = go_ShipModel.transform.localRotation.eulerAngles.z;


		if (direction < -0.05){
			if (currentRotation > 180 - maxRotation)
				go_ShipModel.transform.Rotate (0, 0, -rotationStep);
		}
		else if (direction > 0.05){
			if (currentRotation < 180 + maxRotation)
				go_ShipModel.transform.Rotate (0, 0, rotationStep);
		}
		else {
			if (currentRotation > 182)
				go_ShipModel.transform.Rotate (0, 0, -rotationStep);
			else if (currentRotation < 178) {
				go_ShipModel.transform.Rotate (0, 0, rotationStep);
			}
		}

		SetTrailPosition ();
	}
	public void SetTrailPosition(){
		Vector3 trailPosition = trail.transform.localPosition;
		currentRotation = currentRotation - 180;
		trail.transform.localPosition = new Vector3(trailPosition.x, trailPosition.y, currentRotation/250);
	}
	public void TogglePauseVisualEffects(bool enable){
		if (enable){
			colCorCur.enabled = true;
			colCorCur.saturation = 0f;

			trail.Pause ();
		}
		else{
			colCorCur.saturation = 1f;
			colCorCur.enabled = false;
			trail.Play ();
		}
	}
}