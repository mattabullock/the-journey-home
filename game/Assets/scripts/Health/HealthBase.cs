using UnityEngine;
using System.Collections;

public class HealthBase : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	public float currentHitPoints;
	protected float healthBarLength = 60f;
	
	// Use this for initialization
	protected void Start () {
		currentHitPoints = hitPoints;
	}
	
	[RPC]
	public virtual void TakeDamage(float amt) {
		if (currentHitPoints <= 0) {
			currentHitPoints = 0;
			return;
		}
		
		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			GetComponent<NetworkCharacter>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false;
			Die ();
		}
	}
	
	protected virtual void Die() {
		if( PhotonNetwork.isMasterClient ) {
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
