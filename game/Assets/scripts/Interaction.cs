using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Interaction : MonoBehaviour {
	
	public float cooldown = 0f;
	public float repairDistance = 5f;
	List<SystemBase> systems;

	void Start () {
		systems = new List<SystemBase>(GameObject.FindObjectsOfType<SystemBase> ());
	}

	// Update is called once per frame
	void Update () {
		cooldown -= Time.deltaTime;
		if (Input.GetButton ("Interact") && !Input.GetButton ("Shoot")) {
			interact();
		}

		if (Input.GetButton ("Map")) {
			GameObject.FindGameObjectWithTag ("overlaymapcam").camera.depth = 5;
 		} else {
			GameObject.FindGameObjectWithTag ("overlaymapcam").camera.depth = -1;
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

	void interact() {

		if(cooldown > .1f) {
			Debug.Log ("cooldown");
			return;
		}
		Debug.Log ("asdfasdfasfa");
		RaycastHit hit;
		Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000);

		Debug.Log ("hit: " + hit);

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000) && hit.transform.gameObject.tag == "interactive") {
			var go = hit.point;
			var position = transform.position;
			var dist = go - position;
			var absDist = dist.sqrMagnitude;

			if(absDist < repairDistance) {
				Debug.Log ("repairing");
				var component = hit.transform.GetComponent<SystemBase>();
				component.GetComponent<PhotonView>().RPC ("repair", PhotonTargets.All, .2f);

				//GUI.Box (new Rect (Screen.width/2, Screen.height/2, 100, 25), "Repairing: "+ component.currentHitPoints + "%");
			}
		}

		cooldown = SpawnManager.repairDelay;
	}

	protected void OnGUI(){
		if (Input.GetButton ("Interact") && !Input.GetButton ("Shoot")) {

			RaycastHit hit;
			Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000);
			
			if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000) && hit.transform.gameObject.tag == "interactive") {
				var go = hit.point;
				var position = transform.position;
				var dist = go - position;
				var absDist = dist.sqrMagnitude;
				
				if(absDist < repairDistance) {
					var component = hit.transform.GetComponent<SystemBase>();
					
					GUI.Box (new Rect (Screen.width/2, Screen.height/2, 100, 25), "Repairing: "+ (int)component.currentHitPoints + "%");
				}
			}
			
			cooldown = SpawnManager.repairDelay;
		}
	}
}
	