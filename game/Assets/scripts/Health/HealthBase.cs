﻿using UnityEngine;
using System.Collections;

public class HealthBase : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	public float currentHitPoints;
	public float healthBarLength = 60f;
	
	// Use this for initialization
	protected virtual void Start () {
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
//			GetComponent<MouseLook>().enabled = false;
			foreach(MouseLook m in GetComponentsInChildren<MouseLook>()) {
				m.enabled = false;
			}
//			transform.FindChild("Main Camera").gameObject.GetComponent<MouseLook>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false;
			Die ();
		}
	}
	
	public virtual void Die() {
		if( photonView.isMine ) {
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
