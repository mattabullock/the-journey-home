using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour {

	public float timer = 0f;
	public float spawnDelay = 30f;
	public int startSize = 10;
	public int waveSize = 3;
	public int currentHitPoints = 0;
	public int maxHealth = 100;

	// Use this for initialization
	void Start () {
		currentHitPoints = maxHealth;
		// if (PhotonNetwork.isMasterClient) {
		// 	for(int i = 0; i < startSize; i++) {
		// 		PhotonNetwork.Instantiate("Test Enemy", transform.position, Quaternion.identity, 0, null);
		// 	}
		// }
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			timer += Time.deltaTime;
			if (timer > spawnDelay) {
				timer = 0;
				for(int i = 0; i < waveSize; i++) {
					PhotonNetwork.Instantiate("Test Enemy", transform.position, Quaternion.identity, 0, null);
				}
			}
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (currentHitPoints);
		} else if(stream.isReading) {
			currentHitPoints = (int) stream.ReceiveNext ();
		}
	}

	[RPC]
	public virtual void TakeDamage(float amt) {
		if (currentHitPoints <= 0) {
			currentHitPoints = 0;
			return;
		}
		
		currentHitPoints -= (int) amt;
		
		if(currentHitPoints <= 0) {
			Die ();
		}
	}

	public virtual void Die() {
		if( PhotonNetwork.isMasterClient ) {
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
