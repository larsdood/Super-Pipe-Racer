using UnityEngine;
using System.Collections;

public class ShipObject {

	private ColorGroup colorGroup;
	public ColorGroup ColorGroup {	get {	return colorGroup;	}	}

	private string shipLangKey;

	public string ShipLangKey {
		get {
			return shipLangKey;
		}
	}

	public ShipObject(string shipKey, ColorGroup colorGroup){
		this.shipLangKey = shipKey;
		this.colorGroup = colorGroup;
	}
		
	public ShipObject(int i){
		this.colorGroup = ColorHandler.getColorGroup (i);
		shipLangKey = "ship" + i;
	}
}
