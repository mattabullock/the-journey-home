﻿using UnityEngine;
using System.Collections;

public class PlayerHealth : HealthBase {

	public Animator anim;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}

	protected void OnGUI() {
		GUI.Box (new Rect (Screen.width - healthBarLength - 10, 10, healthBarLength, 20), Mathf.Floor (currentHitPoints) + "/" + hitPoints);
	}
	
	[RPC]
	public override void TakeDamage(float amt) {
		if (currentHitPoints <= 0) {
			currentHitPoints = 0;
			return;
		}

		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			anim.SetBool("Dead", true);
			GetComponent<NetworkCharacter>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false;
			StartCoroutine(dieAnim ());
		}
	}

	IEnumerator dieAnim() {
		yield return new WaitForSeconds (5);
		Die ();
		if (photonView.isMine) {
			SpawnManager.dead = true;
		}
	}
	
}
