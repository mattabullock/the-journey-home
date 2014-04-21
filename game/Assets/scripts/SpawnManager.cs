using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : Photon.MonoBehaviour {

	public static SpawnSpot[] spawnSpots;
	public static List<SystemSpawn> systemSpawns;
	public static bool spawned = true;
	bool firstSpawn = true;
	HealthBay hBay;
	OxygenSystem oSys;
	public static bool dead = false;
	float respawnTimer = 0f;
	float newSpawnerTimer = 0f;
	float newSpawner = 10f;
	int enemiesNeededForNewSpawner = 4;
	public float respawn = 5f;
	public Camera respawnCam;
	bool respawnCamEnabled = false;
	public static bool isGameOver = false;
	public static bool win = false;
	public static float repairDelay = 0.1f;
	public static bool mapEnemy = false;
	public static bool maskChanged = false;
	int noEnemyMask;
	int enemyMask;

	string[] systems = new string[]
		{
			"OxygenSystem",
			"LightSystem",
			"HealthBay",
			"alienspawn",
			"EngineeringBay",
			"CameraSystem",
			"EngineSystem",
			
		};

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		SystemSpawn[] systemSpawnArray = GameObject.FindObjectsOfType<SystemSpawn>();
		systemSpawns = new List<SystemSpawn>(systemSpawnArray);
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


		if (mapEnemy && maskChanged) {
			Camera cam = GameObject.FindGameObjectWithTag ("overlaymapcam").camera;
			cam.cullingMask |= 1 << LayerMask.NameToLayer("Enemies");
			maskChanged = false;
		} else if(!mapEnemy && maskChanged) {
			Camera cam = GameObject.FindGameObjectWithTag ("overlaymapcam").camera;
			cam.cullingMask &= ~(1 << LayerMask.NameToLayer("Enemies"));
			maskChanged = false;
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
		if(newSpawnerTimer >= newSpawner){
			GameObject[] rooms = GameObject.FindGameObjectsWithTag("AIPathCell");
			foreach(GameObject o in rooms){
				AIPathCell c = o.gameObject.GetComponent<AIPathCell>();
				if(c.playersInside == 0 && c.enemiesInside >= enemiesNeededForNewSpawner && !c.hasSpawner){
					PhotonNetwork.InstantiateSceneObject ("alienspawn", o.transform.position + new Vector3(2f,0f,2f), new Quaternion(-90,0,0,0), 0, null);
				}
			}

			newSpawnerTimer = 0f;
		}
		else{
			newSpawnerTimer += Time.deltaTime;
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
		int index = Random.Range (0, systemSpawns.Count);
		SystemSpawn spawn = systemSpawns[index];
		systemSpawns.RemoveAt(index);
		foreach(string s in systems) {
			PhotonNetwork.InstantiateSceneObject (s, spawn.transform.position, spawn.transform.rotation, 0, null);
			if(systemSpawns.Count == 0) {
				break;
			}
			index = Random.Range (0, systemSpawns.Count);
			spawn = systemSpawns[index];
			systemSpawns.RemoveAt(index);
		}
	}

	public void spawnPlayer() {
		SpawnSpot mySpawn = spawnSpots [Random.Range (0, spawnSpots.Length)];
		Debug.Log (mySpawn);
		
		GameObject myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawn.transform.position, mySpawn.transform.rotation, 0);
		((MonoBehaviour) myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerHealth")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Interaction")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		((MonoBehaviour)myPlayerGO.transform.FindChild ("Flashlight").gameObject.GetComponent ("Flashlight")).enabled = true;
		myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
		myPlayerGO.transform.FindChild ("Main Camera").FindChild("Gun Camera").gameObject.SetActive (true);
		myPlayerGO.transform.FindChild ("GunHolder").gameObject.SetActive (true);
		myPlayerGO.transform.FindChild ("Minimap").gameObject.SetActive (true);
		myPlayerGO.transform.FindChild ("minimapMarker").FindChild ("Cone").gameObject.renderer.material.color = Color.green;

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