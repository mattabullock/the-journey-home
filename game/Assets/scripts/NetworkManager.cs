using UnityEngine;
using System.Collections;

public class NetworkManager : Photon.MonoBehaviour {
	
//	public GameObject levelCam;
	public static SpawnSpot[] spawnSpots;
	
	
	// Use this for initialization
	void Start () {
		spawnSpots = GameObject.FindObjectsOfType<SpawnSpot> ();
		Screen.lockCursor = true;
		if (PhotonNetwork.isMasterClient) {
			PhotonNetwork.InstantiateSceneObject ("LightSystem", new Vector3(6.5f, .668f, 15.83f), Quaternion.identity, 0, null);
		}
		spawnPlayer ();
	}
	
	void OnGUI() {
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString ());
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
		((AudioListener) myPlayerGO.transform.FindChild ("Main Camera").gameObject.GetComponent ("AudioListener")).enabled = true;
		
	}
	
}