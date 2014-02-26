﻿using UnityEngine;
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
	
	void Connect() {
		PhotonNetwork.ConnectUsingSettings ("v003");
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
	
	void OnJoinedRoom() {
		spawnPlayer ();
	}
	
	public void spawnPlayer() {
		
		SpawnSpot mySpawn = spawnSpots [Random.Range (0, spawnSpots.Length)];
		
		levelCam.SetActive (false);
		
		GameObject myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawn.transform.position, mySpawn.transform.rotation, 0);
		((MonoBehaviour) myPlayerGO.GetComponent ("FPSInputController")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("CharacterMotor")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Interaction")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);
		
	}
	
}