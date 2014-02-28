using UnityEngine;
using System.Collections;

public class NetworkManager : MonoBehaviour {
	
	public GameObject levelCam;
	public static SpawnSpot[] spawnSpots;
	
	
	// Use this for initialization
	void Start () {
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
		Screen.lockCursor = true;
		Connect ();
	}

	void Update() {
		//something about updating respawns should go here?!
	}
	
	void Connect() {
		PhotonNetwork.ConnectUsingSettings ("v004");
	}
	
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString ());
	}
	
	void OnJoinedLobby() {
		PhotonNetwork.JoinRandomRoom ();
	}
	
	void OnPhotonRandomJoinFailed() {
		PhotonNetwork.CreateRoom (null);
	}

	void OnCreatedRoom() {
		PhotonNetwork.Instantiate ("LightSystem", new Vector3(6.5f, .668f, 15.83f), Quaternion.identity, 0);
	}
	
	void OnJoinedRoom() {
		spawnPlayer ();
	}
	
	public void spawnPlayer() {
		
		SpawnSpot mySpawn = spawnSpots [Random.Range (0, spawnSpots.Length)];
		
		levelCam.SetActive (false);
		
		GameObject myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawn.transform.position, mySpawn.transform.rotation, 0);
		((MonoBehaviour) myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Health")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Interaction")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
//		((MonoBehaviour) myPlayerGO.GetComponent ("NetworkCharacter")).enabled = true;
		myPlayerGO.transform.FindChild ("Main Camera").gameObject.SetActive (true);
		((AudioListener) myPlayerGO.transform.FindChild ("Main Camera").gameObject.GetComponent ("AudioListener")).enabled = true;
		
	}
	
}