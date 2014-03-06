using UnityEngine;
using System.Collections;

public class SpawnManager : Photon.MonoBehaviour {

	public static SpawnSpot[] spawnSpots;
	public static bool spawned = true;
	bool firstSpawn = true;
	HealthBay hBay;
	OxygenSystem oSys;
	public static bool dead = false;
	float respawnTimer = 0f;
	public float respawn = 5f;
	public Camera respawnCam;
	bool respawnCamEnabled = false;
	public static bool isGameOver = false;
	public static bool win = false;

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
		if (isGameOver) {
			StartCoroutine(gameOver());
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
		}
	}

	void OnGUI() {

		if (!respawnCamEnabled) {
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
			if(win) {
				GUI.Box (new Rect(Screen.width/2, Screen.height/2, 100, 30), "YOU WIN!");
				StartCoroutine(winFunction());
			}
		} else if(isGameOver) {
			GUILayout.Label("GAME OVER.");
		}else {
			GUILayout.Label("You will respawn in " + Mathf.Ceil(respawn - respawnTimer) + " seconds.");
		}

	}

	void spawnStuff() {
		PhotonNetwork.InstantiateSceneObject ("LightSystem", new Vector3(19.18432f, 0.5094447f, -20.10653f), Quaternion.identity, 0, null);
		PhotonNetwork.InstantiateSceneObject ("HealthBay", new Vector3(40.51247f, 2.01f, 60.22619f), Quaternion.identity, 0, null);
		PhotonNetwork.InstantiateSceneObject ("OxygenSystem", new Vector3(39.28933f, .5f, 19.38437f), Quaternion.identity, 0, null);
		PhotonNetwork.InstantiateSceneObject ("EngineSystem", new Vector3 (0.9676633f, 1.648776f, 39.82158f), Quaternion.identity, 0, null);
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
		myPlayerGO.transform.FindChild ("Minimap").gameObject.SetActive (true);

	}

	IEnumerator winFunction() {
		yield return new WaitForSeconds (5);
		Application.Quit ();
	}

	IEnumerator gameOver() {
		yield return new WaitForSeconds (5);
		respawnCam.gameObject.SetActive(true);
		foreach(PlayerHealth p in FindObjectsOfType<PlayerHealth>()) {
			p.Die();
		}
		yield return new WaitForSeconds (10);
		Application.Quit ();
	}
	
}