using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour {

	public float timer = 0f;
	public float spawnDelay = 200f;
	public int waveSize = 10;

	// Use this for initialization
	void Start () {
		if (PhotonNetwork.isMasterClient) {
			for(int i = 0; i < waveSize; i++) {
				PhotonNetwork.Instantiate("Test Enemy", transform.position, Quaternion.identity, 0, null);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			timer += Time.deltaTime;
			if (timer > spawnDelay) {
				timer = 0;
				for(int i = 0; i < 5; i++) {
					PhotonNetwork.Instantiate("Test Enemy", transform.position, Quaternion.identity, 0, null);
				}
			}
		}
	}
}
