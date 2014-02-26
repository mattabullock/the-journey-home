using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	float currentHitPoints;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}

//	void OnGUI() {
//		GUILayout.Label(currentHitPoints.ToString());
//	}
	
	[RPC]
	public void TakeDamage(float amt) {
		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			Die();
		}
	}
	
	void Die() {
		if( GetComponent<PhotonView>().instantiationId==0 ) {
			Destroy(gameObject);
		}
		else {
			if( photonView.isMine ) {
				PhotonNetwork.Destroy(gameObject);
				spawn ();
			}
		}
	}

	void spawn() {
		
		SpawnSpot mySpawn = NetworkManager.spawnSpots [Random.Range (0, NetworkManager.spawnSpots.Length)];
		//levelCam.SetActive (false);

		GameObject myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawn.transform.position, mySpawn.transform.rotation, 0);
		((MonoBehaviour) myPlayerGO.GetComponent ("FPSInputController")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("CharacterMotor")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);
		
	}
}
