using UnityEngine;
using System.Collections;

public class Crosshair : MonoBehaviour {

	public Texture2D crosshairTexture;
	Rect position;
	public bool isOn = true;

	// Use this for initialization
	void Start () {
		position = new Rect((Screen.width - crosshairTexture.width) / 2, (Screen.height - 
			crosshairTexture.height) /2, crosshairTexture.width, crosshairTexture.height);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton (1)) {
			isOn = false;
		} else {
			isOn = true;
		}
	}

	void OnGUI() {
		if(isOn) {
			GUI.DrawTexture(position, crosshairTexture);
		}
	}
}