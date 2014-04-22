using UnityEngine;
using System.Collections;

public class UpdateScript: MonoBehaviour {

	bool menuScreen = false;
	SystemOverlay sysO;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Escape") && !menuScreen) {
			Debug.Log ("escape!");
			//pause the game
			menuScreen = true;
			Time.timeScale = 0;
			//show the pause menu
			sysO = gameObject.AddComponent<SystemOverlay> (); 
			sysO.enabled = true;
		} else if (Input.GetButtonDown ("Escape") && menuScreen) {
			Time.timeScale = 1.0f;
			Destroy(sysO);
			Screen.showCursor = false;
			menuScreen = false;
		}
	}
}
