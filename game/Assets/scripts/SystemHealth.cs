﻿using UnityEngine;
using System.Collections;

public class SystemHealth : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	float currentHitPoints;
	public float healthBarLength;
	float currHealthBarLength;
	GameObject lights;
	
	// Use this for initialization
	void Start () {
		lights = GameObject.FindGameObjectWithTag ("Lights");
		currentHitPoints = hitPoints;
		currHealthBarLength = healthBarLength;
	}

	void OnGUI(){
		GUI.Box (new Rect(Screen.width - 10 - healthBarLength,10, currHealthBarLength, 20), GUIContent.none);
		GUI.Box (new Rect(Screen.width - 10 - healthBarLength,10, healthBarLength, 20), currentHitPoints + "/" + hitPoints);
	}
	
	[RPC]
	public void repair(float amt) {
		if (currentHitPoints >= hitPoints) {
			return;	
		} else if (currentHitPoints + amt > hitPoints) {
			currentHitPoints = hitPoints;
		} else if (currentHitPoints + amt > 50) {
			currentHitPoints += amt;
			lights.SetActive (true);
		} else {
			currentHitPoints += amt;
		}

		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}

	[RPC]
	public void TakeDamage(float amt) {

		if (currentHitPoints <= 0) {
			return;
		} else if (currentHitPoints - amt <= 0) {
			currentHitPoints = 0;
			photonView.RPC ("SystemDown", PhotonTargets.All);
		} else {
			lights.SetActive(false);
			StartCoroutine (flickerLights());
			currentHitPoints -= amt;
		}	

		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}

	IEnumerator flickerLights() {
		yield return new WaitForSeconds(.2f);
		lights.SetActive (true);
	}
	
	[RPC]
	void SystemDown() {
		GameObject lights = GameObject.FindGameObjectWithTag ("Lights");
		lights.SetActive (false);
		Debug.Log ("The system is down.");
	}

}
