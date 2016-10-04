using UnityEngine;
using System.Collections;

public class ColorGroup{

	private Color color1;
	public Color Color1 {
		get {
			return color1;
		}
	}

	private Color color2;
	public Color Color2 {
		get {
			return color2;
		}
	}

	private Color color3;
	public Color Color3 {
		get {
			return color3;
		}
	}

	public ColorGroup (Color color1, Color color2, Color color3){
		this.color1 = color1;
		this.color2 = color2;
		this.color3 = color3;
	}
}