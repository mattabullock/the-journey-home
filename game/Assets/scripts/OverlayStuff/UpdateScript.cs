using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateScript: MonoBehaviour {

	bool menuScreen = false;
	PauseMenu sysO;
	public SystemOverlay overlay;
	List<SystemBase> systems;


	// Use this for initialization
	void Start () {
		systems = new List<SystemBase>(GameObject.FindObjectsOfType<SystemBase> ());
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Escape") && !menuScreen) {
			//pause the game
			menuScreen = true;
			Time.timeScale = 0;
			//show the pause menu
			sysO = gameObject.AddComponent<PauseMenu> (); 
			sysO.enabled = true;
		} else if (Input.GetButtonDown ("Escape") && menuScreen) {
			Time.timeScale = 1.0f;
			Destroy(sysO);
			Screen.showCursor = false;
			menuScreen = false;
		}

		if (Input.GetButtonDown ("Overlay")) {
			overlay.enabled = true;
		} else if(Input.GetButtonUp ("Overlay")) {
			overlay.enabled = false;
		}

		//finding systems stuff
		SystemBase toDelete = null;
		foreach (SystemBase go in systems) {
			var dist = go.transform.position - transform.position;
			var absDist = dist.sqrMagnitude;
			
			if(absDist < 50) {
				toDelete = go;
				go.GetComponent<PhotonView>().RPC( "found", PhotonTargets.AllBuffered, null);
			}
		}
		if (toDelete != null) {
			systems.Remove (toDelete);
		}

	}
}
