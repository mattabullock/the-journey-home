using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	public float currentHitPoints;
	public Animator anim;
	public bool dead = false;
	public bool notSpawned = false;
	public float respawnTimer = 0f;
	float respawn = 5f;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}

	void Update() {
		if (dead || notSpawned) {
			respawnTimer += Time.deltaTime;
			if(respawnTimer >= respawn && dead) {
				dead = false;
				notSpawned = true;
				respawnTimer = 0;
				Die();
			} else if(respawnTimer >= respawn && notSpawned) {
				notSpawned = false;
				spawn ();
				respawnTimer = 0;
			}
		}
	}

//	void OnGUI() {
//		GUILayout.Label(currentHitPoints.ToString());
//	}
	
	[RPC]
	public void TakeDamage(float amt) {
		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			anim.SetBool("Dead", true);
			dead = true;
			((MonoBehaviour) gameObject.GetComponent ("MouseLook")).enabled = false;
		}
	}

	void Die() {
		if( GetComponent<PhotonView>().instantiationId==0 ) {
			Destroy(gameObject);
		}
		else {
			if( photonView.isMine ) {
				PhotonNetwork.Destroy(gameObject);
			}
		}
	}

	void spawn() {
		
		SpawnSpot mySpawn = NetworkManager.spawnSpots [Random.Range (0, NetworkManager.spawnSpots.Length)];

		GameObject myPlayerGO = (GameObject) PhotonNetwork.Instantiate ("PlayerController", mySpawn.transform.position, mySpawn.transform.rotation, 0);
		((MonoBehaviour) myPlayerGO.GetComponent ("MouseLook")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Health")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("Interaction")).enabled = true;
		((MonoBehaviour) myPlayerGO.GetComponent ("PlayerShooting")).enabled = true;
		myPlayerGO.transform.FindChild("Main Camera").gameObject.SetActive(true);
		
	}
}
