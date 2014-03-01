﻿using UnityEngine;
using System.Collections;

public class HealthBay : SystemBase {
	
	GameObject lights;
	
	const float flickerOn = 7f;
	const float flickerOff = 8f;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
	}
	
	protected override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {

		} else if (stream.isReading) {

		}
	}
	
	protected override void OnGUI() {
		GUI.Box (new Rect(Screen.width - 10 - healthBarLength,10, currHealthBarLength, 20), GUIContent.none);
		GUI.Box (new Rect(Screen.width - 10 - healthBarLength,10, healthBarLength, 20), currentHitPoints + "/" + hitPoints);
	}
	
	[RPC]
	protected override void repair (float amt) {
		if (currentHitPoints >= hitPoints) {
			return;	
		} else if (currentHitPoints + amt > hitPoints) {
			currentHitPoints = hitPoints;
		} else if (currentHitPoints + amt > threshold && down) {
			currentHitPoints += amt;
			belowThresh = false;
			trigger();
		} else {
			currentHitPoints += amt;
		}
		
		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}
	
	[RPC]
	protected override void TakeDamage(float amt) {
		
		if (currentHitPoints <= 0) {
			return;
		} else if (currentHitPoints - amt <= 0) {
			currentHitPoints = 0;
			if(!belowThresh) {
				trigger();
				belowThresh = true;
			}
		} else {
			if(!belowThresh) {
				trigger();
			}
			currentHitPoints -= amt;
		}	
		
		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}
	
	protected void trigger() {
		Debug.Log ("Something is happening to the med bay!");
	}
	
}
