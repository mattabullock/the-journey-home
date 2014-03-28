using UnityEngine;
using System.Collections;

public class SpawnManager : Photon.MonoBehaviour {

	public static SpawnSpot[] spawnSpots;
	public static bool spawned = true;
	public static bool mapEnemy = false;
	bool firstSpawn = true;
	HealthBay hBay;
	OxygenSystem oSys;
	public static bool dead = false;
	float respawnTimer = 0f;
	public float respawn = 5f;
	public Camera respawnCam;
	bool respawnCamEnabled = false;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		if (PhotonNetwork.isMasterClient) {
			spawnStuff ();
		}

		hBay = GameObject.FindObjectOfType<HealthBay> ();
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
	}

	void Update() {

		if (mapEnemy) {
			Object[] tempList = Resources.FindObjectsOfTypeAll (typeof(GameObject));
			foreach (GameObject obj in tempList) {
				if (obj.name.Equals ("Map")) {
					Camera gObj = obj.camera;
					gObj.cullingMask = (gObj.cullingMask) | (1 << LayerMask.NameToLayer("Enemy"));
				}
			}
		}

		if (hBay == null) {
			hBay = GameObject.FindObjectOfType<HealthBay> ();
		}
		if (oSys == null) {
			oSys = GameObject.FindObjectOfType<OxygenSystem> ();
		}
		if (dead) {
			if(!respawnCamEnabled) {
				respawnCam.gameObject.SetActive(true);
				respawnCamEnabled = true;
			}
			if(!hBay.isDown()) {
				respawnTimer += Time.deltaTime;
			} else {
				respawnTimer = 0f;
			}
			if(respawnTimer >= respawn) {
				dead = false;
				spawned = false;
				respawnTimer = 0;
			}
		}
		if (!spawned) {
			spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
			if (spawnSpots.Length != 0) {
				spawned = true;
				respawnCamEnabled = false;
				respawnCam.gameObject.SetActive(false);
				spawnPlayer ();
			}
		} else if (firstSpawn) {
			spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
			if (spawnSpots.Length != 0) {
				firstSpawn = false;
				spawned = true;
				spawnPlayer ();
			}
			if (PhotonNetwork.isMasterClient) {
				PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
				PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
				PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
				PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
			}
		}
	}

	void OnGUI() {
		if (!respawnCamEnabled) {
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		} else {
			GUILayout.Label("You will respawn in " + Mathf.Ceil(respawn - respawnTimer) + " seconds.");
		}
	}

	void spawnStuff() {
		PhotonNetwork.InstantiateSceneObject ("LightSystem", new Vector3(19.18432f, 0.5094447f, -20.10653f), Quaternion.identity, 0, null);
		PhotonNetwork.InstantiateSceneObject ("HealthBay", new Vector3(40.51247f, 2.01f, 60.22619f), Quaternion.identity, 0, null);
		PhotonNetwork.InstantiateSceneObject ("OxygenSystem", new Vector3(39.28933f, .5f, 19.38437f), Quaternion.identity, 0, null);
	}

	public void spawnPlayer() {
		SpawnSpot mySpawn = spawnSpots [Random.Range (0, spawnSpots.Length)];
		
		GameObject myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawn.transform.position, mySpawn.transform.rotation, 0);
		((MonoBehaviour) myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerHealth")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Interaction")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
		myPlayerGO.transform.FindChild ("Main Camera").FindChild("Gun Camera").gameObject.SetActive (true);
	//	myPlayerGO.transform.FindChild ("Map Cam").gameObject.SetActive (true);
	//	myPlayerGO.transform.FindChild ("Map Cam").gameObject.transform.rotation = Quaternion.Euler (90, 90, 0);
		myPlayerGO.transform.FindChild ("Minimap").gameObject.SetActive (true);

	}
	
}