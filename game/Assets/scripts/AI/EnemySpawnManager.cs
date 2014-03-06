using UnityEngine;
using System.Collections;

public class EnemySpawnManager : MonoBehaviour {

	public float timer = 0f;
	public float spawnDelay = 3f;

	// Use this for initialization
	void Start () {
		if (PhotonNetwork.isMasterClient) {
			for(int i = 0; i < 10; i++) {
				PhotonNetwork.Instantiate("Test Enemy", new Vector3(1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (PhotonNetwork.isMasterClient) {
			timer += Time.deltaTime;
			if (timer > spawnDelay) {
				timer = 0;
				PhotonNetwork.Instantiate ("Test Enemy", new Vector3 (1.899121f, 0.5744562f, -3.08994f), Quaternion.identity, 0, null);
			}
		}
	}
}
