using UnityEngine;
using System.Collections;

public class Pipe : MonoBehaviour {
	public float pipeRadius;
	public int pipeSegmentCount;
	Texture2D pipeTexture;

	public float minCurveRadius, maxCurveRadius;
	public int minCurveSegmentCount, maxCurveSegmentCount;

	private float curveRadius;

	private Vector2[] uv;

	public float CurveRadius {
		get {
			return curveRadius;
		}
	}

	private int curveSegmentCount;

	public int CurveSegmentCount {
		get {
			return curveSegmentCount;
		}
	}

	private Mesh mesh;
	private Vector3[] vertices;
	private int[] triangles;
	public float ringDistance;
	private float curveAngle;

	public float CurveAngle {
		get {
			return curveAngle;
		}
	}

	private float relativeRotation;

	public float RelativeRotation {
		get {
			return relativeRotation;
		}
	}
	public PipeItemGenerator[] normalModeGenerators;
	public PipeItemGenerator[] spiralModeGenerators;

	private PipeItemGenerator[] generators;

	public PipeItemGenerator slalomGenerator;
	public PipeItemGenerator hyperSpeedModeGenerator;

	private void Awake(){
		GetComponent<MeshFilter> ().mesh = mesh = new Mesh ();
		mesh.name = "Pipe";
		generators = new PipeItemGenerator[1];
		generators [0] = normalModeGenerators [0];
	}
	public void Generate (int sectionNr, bool withItems = true)
	{
		GameMode gameMode = (GameMode) PlayerPrefs.GetInt (GloVar.GameModePrefName, 0);
		curveRadius = Random.Range (minCurveRadius, maxCurveRadius);
		curveSegmentCount = Random.Range (minCurveSegmentCount, maxCurveSegmentCount + 1);
		mesh.Clear ();
		SetVertices ();
		SetUV ();
		SetTriangles ();
		CreateTexture (sectionNr, withItems, gameMode);
		if (gameMode == GameMode.Normal_SlowStart || gameMode == GameMode.Normal_FastStart || gameMode == GameMode.ThreeStrikes){
			SetGenerators (sectionNr);
		}
		else if (gameMode == GameMode.Slalom){
			SetSlalomGenerator();
		}
		else if (gameMode == GameMode.Spiral){
			SetSpiralGenerators ();
		}
		else if (gameMode == GameMode.HyperSpeed){
			SetHyperSpeedGenerators ();
		}
		this.gameObject.GetComponent<Renderer> ().material.mainTexture = pipeTexture;
		mesh.RecalculateNormals ();
		for (int i = 0; i < transform.childCount; i++) {
			Destroy (transform.GetChild (i).gameObject);
		}
		if (withItems){
			generators [Random.Range (0, generators.Length)].GenerateItems (this);
		}
	}
	private void SetSlalomGenerator(){
		generators = new PipeItemGenerator[1];
		generators [0] = slalomGenerator;
	}
	private void SetSpiralGenerators(){
		generators = spiralModeGenerators;
	}
	private void SetHyperSpeedGenerators(){
		generators = new PipeItemGenerator[1];
		generators[0] = hyperSpeedModeGenerator;
	}
	private void SetGenerators(int sectionNr){
		if (normalModeGenerators.Length>generators.Length && sectionNr !=0){
			generators = new PipeItemGenerator[sectionNr];
			for (int i = 0; i < sectionNr; i++) {
				generators [i] = normalModeGenerators [i];
			}
		}
	}
	private void CreateTexture(int sectionNr, bool withItems, GameMode gameMode){
		pipeTexture = new Texture2D (32, 32, TextureFormat.ARGB32, false);
		Color newColor;
		switch(gameMode){
		case GameMode.Normal_SlowStart:
		case GameMode.Normal_FastStart:
		case GameMode.ThreeStrikes:
			/* SET LINES TO WHITE */
			for (int j = 0; j < 32; j++) {
				pipeTexture.SetPixel (15, j, Color.white);
				pipeTexture.SetPixel (16, j, Color.white);
			}
			for (int i = 0; i < 32; i++) {
				pipeTexture.SetPixel (i, 15, Color.white);
				pipeTexture.SetPixel (i, 16, Color.white);
			}

			if (!withItems) {
				newColor = Color.black;
			} else {
				newColor = new Color (Random.Range (0.1f, 0.85f), Random.Range (0.1f, 0.85f), Random.Range (0.1f, 0.85f));
			}

			/* ASSIGN RANDOM COLORS TO SQUARES */
			for (int i = 0; i < 15; i++) {
				for (int j = 0; j < 15; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
				for (int j = 17; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			for (int i = 17; i < 32; i++) {
				for (int j = 0; j < 15; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
				for (int j = 17; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			break;

		case GameMode.Slalom:
			/* SET LINES TO DARK */
			for (int j = 0; j < 32; j++) {
				pipeTexture.SetPixel (15, j, new Color(0, 0, 0.3f));
				pipeTexture.SetPixel (16, j, new Color(0, 0, 0.3f));
			}
			for (int i = 0; i < 32; i++){
				pipeTexture.SetPixel (i, 15, new Color(0, 0, 0.3f));
				pipeTexture.SetPixel (i, 16, new Color(0, 0, 0.3f));
			}

			/* CREATE FILL COLOR */
			if (!withItems){
				newColor = new Color(0.8f, 0.8f, 1.0f);
			}
			else{
				float blue = Random.Range (0.55f, 1f);
				float others = Random.Range (0.25f, blue-0.2f);
				newColor = new Color(others,others,blue);
			}

			/* ASSIGN RANDOM COLORS TO SQUARES */
			for (int i = 0; i < 15; i++) {
				for (int j = 0; j < 15; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
				for (int j = 17; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			for (int i = 17; i < 32; i++) {
				for (int j = 0; j < 15; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
				for (int j = 17; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			break;

		case GameMode.Spiral:
			/* SET LINES TO BLACK */
			if (!withItems){
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (15, j, Color.white);
					pipeTexture.SetPixel (16, j, Color.white);
				}
				for (int i = 0; i < 32; i++){
					pipeTexture.SetPixel (i, 15, Color.white);
					pipeTexture.SetPixel (i, 16, Color.white);
				}
			}
			else{
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (15, j, Color.black);
					pipeTexture.SetPixel (16, j, Color.black);
				}
				for (int i = 0; i < 32; i++){
					pipeTexture.SetPixel (i, 15, Color.black);
					pipeTexture.SetPixel (i, 16, Color.black);
				}
			}


			/* CREATE FILL COLOR */
			if (!withItems){
				newColor = new Color(0.1f, 0.1f, 0.1f);
			}
			else{
				float colorFloat = Random.Range (0.5f, 0.85f);
				newColor = new Color(Random.Range(colorFloat-0.1f, colorFloat+0.1f),Random.Range(colorFloat-0.1f, colorFloat+0.1f),Random.Range(colorFloat-0.1f, colorFloat+0.1f));
			}

			/* ASSIGN RANDOM COLORS TO SQUARES */
			for (int i = 0; i < 15; i++) {
				for (int j = 0; j < 15; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
				for (int j = 17; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			for (int i = 17; i < 32; i++) {
				for (int j = 0; j < 15; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
				for (int j = 17; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			break;

		case GameMode.HyperSpeed:
			if (!withItems) {
				Color tempColor = Color.black;
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (0, j, tempColor);
					pipeTexture.SetPixel (1, j, tempColor);
				}
				for (int i = 2; i < 9; i++) {
					pipeTexture.SetPixel (i, 0, tempColor);
					pipeTexture.SetPixel (i, 1, tempColor);
				}

				/* SET MIDDLE LINE + BRIDGE */
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (8, j, tempColor);
					pipeTexture.SetPixel (9, j, tempColor);
				}
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (16, j, tempColor);
					pipeTexture.SetPixel (17, j, tempColor);
				}
				for (int i = 10; i < 16; i++) {
					pipeTexture.SetPixel (i, 16, tempColor);
					pipeTexture.SetPixel (i, 17, tempColor);
				}
			} else {
				/* SET LEFTMOST LINE + BRIDGE */
				Color tempColor = GetHyperRandomLine ();
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (0, j, tempColor);
					pipeTexture.SetPixel (1, j, tempColor);
				}
				for (int i = 2; i < 9; i++) {
					pipeTexture.SetPixel (i, 0, tempColor);
					pipeTexture.SetPixel (i, 1, tempColor);
				}

				/* SET MIDDLE LINE + BRIDGE */
				tempColor = GetHyperRandomLine ();
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (8, j, tempColor);
					pipeTexture.SetPixel (9, j, tempColor);
				}

				tempColor = GetHyperRandomLine ();
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (16, j, tempColor);
					pipeTexture.SetPixel (17, j, tempColor);
				}
				for (int i = 10; i < 16; i++) {
					pipeTexture.SetPixel (i, 16, tempColor);
					pipeTexture.SetPixel (i, 17, tempColor);
				}
			}

			/* CREATE FILL COLOR */
			if (!withItems){
				newColor = Color.white;
			}
			else{
				float colorFloat = Random.Range (0.0f, 0.1f);
				newColor = new Color(colorFloat,colorFloat,colorFloat);
			}

			/* ASSIGN RANDOM COLORS TO SQUARES */
			for (int i = 2; i < 8; i++) {
				for (int j = 2; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			for (int i = 10; i < 16; i++) {
				for (int j = 0; j < 16; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
				for (int j = 18; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			for (int i = 18; i < 32; i++) {
				for (int j = 0; j < 32; j++) {
					pipeTexture.SetPixel (i, j, newColor);
				}
			}
			break;
		default:
			goto case GameMode.Normal_FastStart;
		}


		pipeTexture.Apply ();
	}
	private Color GetHyperRandomLine(){
		float red = 0, green = 0, blue = 0;
		float lowL = 0f, lowU = 0.4f, highL = 0.7f, highU = 1.0f;
		switch(Random.Range(1,6)) {
		case (1):
			red = Random.Range (lowL, lowU);
			green = Random.Range (highL, highU);
			blue = Random.Range (highL, highU);
			break;
		case (2):
			red = Random.Range (highL, highU);
			green = Random.Range (lowL, lowU);
			blue = Random.Range (highL, highU);
			break;
		case (3):
			red = Random.Range (lowL, lowU);
			green = Random.Range (lowL, lowU);
			blue = Random.Range (highL, highU);
			break;
		case (4):
			red = Random.Range (lowL, lowU);
			green = Random.Range (lowL, lowU);
			blue = Random.Range (highL, highU);
			break;
		case (5):
			red = Random.Range (highL, highU);
			green = Random.Range (lowL, lowU);
			blue = Random.Range (highL, highU);
			break;
		case (6):
			red = Random.Range (lowL, lowU);
			green = Random.Range (highL, highU);
			blue = Random.Range (highL, highU);
			break;
		}
		return new Color (red, green, blue);
	}
	private void SetUV(){
		uv = new Vector2[vertices.Length];
		for (int i = 0; i<vertices.Length;i+=4){
			uv [i] = Vector2.zero;
			uv [i + 1] = Vector2.right;
			uv [i + 2] = Vector2.up;
			uv [i + 3] = Vector2.one;
		}
		mesh.uv = uv;
	}
	private void SetVertices(){
		vertices = new Vector3[pipeSegmentCount * curveSegmentCount * 4];
		float uStep = ringDistance / curveRadius;
		curveAngle = uStep * curveSegmentCount * (360f / (2f * Mathf.PI));
		CreateFirstQuadring (uStep);
		int iDelta = pipeSegmentCount * 4;
		for (int u = 2, i = iDelta; u <= curveSegmentCount; u++, i += iDelta) {
			CreateQuadRing (u * uStep, i);
		}
		mesh.vertices = vertices;
	}
	public void AlignWith(Pipe pipe){
		relativeRotation = Random.Range (0, curveSegmentCount) * 360f/pipeSegmentCount;

		transform.SetParent (pipe.transform, false);
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.Euler (0f, 0f, -pipe.curveAngle);
		transform.Translate (0f, pipe.curveRadius, 0f);
		transform.Rotate (relativeRotation, 0f, 0f);
		transform.Translate (0f, -curveRadius, 0f);
		transform.SetParent (pipe.transform.parent);
		transform.localScale = Vector3.one;
	}
	private void CreateFirstQuadring (float u) {
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;

		Vector3 vertexA = GetPointsOnTorus (0f, 0f);
		Vector3 vertexB = GetPointsOnTorus (u, 0f);
		for (int v = 1, i = 0; v <= pipeSegmentCount; v++, i +=4) {
			vertices [i] = vertexA;
			vertices[i + 1] = vertexA = GetPointsOnTorus (0f, v * vStep);
			vertices[i + 2] = vertexB;
			vertices[i + 3] = vertexB = GetPointsOnTorus (u, v * vStep);
			
		}
	}
	private void CreateQuadRing (float u, int i) {
		float vStep = (2f * Mathf.PI) / pipeSegmentCount;
		int ringOffset = pipeSegmentCount * 4;

		Vector3 vertex = GetPointsOnTorus (u, 0f);
		for (int v = 1; v <= pipeSegmentCount; v++, i +=4) {
			vertices [i] = vertices [i - ringOffset + 2];
			vertices [i + 1] = vertices [i - ringOffset + 3];
			vertices[i + 2] = vertex;
			vertices [i + 3] = vertex = GetPointsOnTorus (u, v * vStep);

		}
	}
	private void SetTriangles(){
		triangles = new int[pipeSegmentCount * curveSegmentCount * 6];
		for (int t = 0, i = 0; t < triangles.Length; t+=6, i += 4) {
			triangles [t] = i;
			triangles [t + 1] = triangles [t + 4] = i + 2;
			triangles [t + 2] = triangles [t + 3] = i + 1;
			triangles [t + 5] = i + 3;
		}
		mesh.triangles = triangles;
	}
	private Vector3 GetPointsOnTorus(float u, float v){
		Vector3 p;
		float r = (curveRadius + pipeRadius * Mathf.Cos (v));
		p.x = r * Mathf.Sin (u);
		p.y = r * Mathf.Cos (u);
		p.z = pipeRadius * Mathf.Sin (v);
		return p;
	}
}
