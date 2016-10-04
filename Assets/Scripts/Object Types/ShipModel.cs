using UnityEngine;
using System.Collections;

public class ShipModel : MonoBehaviour {

	public ShipModel(){
		
	}
	public MeshRenderer meshRenderer;

	public void SetMaterials(ParticleSystem trail){
		ColorGroup colorGroup = ColorHandler.getColorGroup (PlayerPrefs.GetInt(GloVar.ShipModelChoicePrefName, 0));
		meshRenderer.materials [0].SetColor ("_Color", colorGroup.Color1);
		meshRenderer.materials [1].SetColor ("_Color", colorGroup.Color2);
		meshRenderer.materials [2].SetColor ("_Color", colorGroup.Color3);
	}
}