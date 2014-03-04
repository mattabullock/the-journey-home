using UnityEngine;
using System.Collections;

public class SpawnManager : Photon.MonoBehaviour {

	public static SpawnSpot[] spawnSpots;
	public static bool spawned = true;
	bool firstSpawn = true;
	HealthBay hBay;
	public static bool dead = false;
	float respawnTimer = 0f;
	public float respawn = 5f;
	public Camera respawnCam;
	bool respawnCamEnabled = false;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.InstantiateSceneObject ("LightSystem", new Vector3(40.48185f, 0.51f, 19.66377f), Quaternion.identity, 0, null);
			PhotonNetwork.InstantiateSceneObject ("HealthBay", new Vector3(40.51247f, 2.01f, 60.22619f), Quaternion.identity, 0, null);
			PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
			PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
			PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
			PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
		}
		hBay = GameObject.FindObjectOfType<HealthBay> ();
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
	}

	void Update() {
		if (hBay == null) {
			hBay = GameObject.FindObjectOfType<HealthBay> ();
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

	public void spawnPlayer() {
		SpawnSpot mySpawn = spawnSpots [Random.Range (0, spawnSpots.Length)];
		
		GameObject myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawn.transform.position, mySpawn.transform.rotation, 0);
		((MonoBehaviour) myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Health")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Interaction")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
		myPlayerGO.transform.FindChild ("Main Camera").FindChild("Gun Camera").gameObject.SetActive (true);

	}
	
}