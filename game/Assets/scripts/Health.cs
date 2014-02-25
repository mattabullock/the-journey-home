using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	float currentHitPoints;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}

	void OnGUI() {
		GUILayout.Label(currentHitPoints.ToString());
	}
	
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
			}
//			if( PhotonNetwork.isMasterClient ) {
//				PhotonNetwork.Destroy(gameObject);
//			}
		}
	}
}
