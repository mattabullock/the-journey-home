using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	public float currentHitPoints;
	public Animator anim;
	public bool dead = false;
	public bool spawned = true;
	public float respawnTimer = 0f;
	float respawn = 5f;
	float healthBarLength = 60f;
	HealthBay hBay;

	// Use this for initialization
	void Start () {
		hBay = GameObject.FindObjectOfType<HealthBay> ();
		currentHitPoints = hitPoints;
	}

	void Update() {
//		if (dead) {
//			if(!hBay.isDown()) {
//				respawnTimer += Time.deltaTime;
//			} else {
//				respawnTimer = 0f;
//			}
//			if(respawnTimer >= respawn) {
//				dead = false;
//				anim.SetBool("Dead", false);
//				respawnTimer = 0;
//				if (photonView.isMine) {
//					GameObject.FindGameObjectWithTag ("RespawnCam").SetActive (false);
//				}
//				spawn ();
//			}
//		}
	}

	void OnGUI() {
		GUI.Box (new Rect(Screen.width - 10 - healthBarLength, 70, healthBarLength, 20), currentHitPoints + "/" + hitPoints);
	}
	
	[RPC]
	public void TakeDamage(float amt) {
		if (currentHitPoints <= 0) {
			currentHitPoints = 0;
			return;
		}

		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			anim.SetBool("Dead", true);
			StartCoroutine(dieAnim ());
		}
	}

	IEnumerator dieAnim() {
		yield return new WaitForSeconds (5);
		dead = true;
		Die();
		NetworkManager.spawned = false;
//		spawn ();
//		if (photonView.isMine) {
//			GameObject.FindGameObjectWithTag ("RespawnCam").SetActive (true);
//		}
	}

	void Die() {
		if( photonView.isMine ) {
			PhotonNetwork.Destroy(gameObject);
		}
		//		gameObject.SetActive(false);
	}

	void spawn() {
		SpawnSpot mySpawn = NetworkManager.spawnSpots [Random.Range (0, NetworkManager.spawnSpots.Length)];

		GetComponent<Transform>().position = mySpawn.transform.position;
		GetComponent<Transform> ().rotation = mySpawn.transform.rotation;
		gameObject.SetActive(true);
	}
}
