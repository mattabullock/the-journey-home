using UnityEngine;
using System.Collections;

public class HealthBay : SystemBase {
	
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
	
	[RPC]
	protected override void repair (float amt) {
		if (currentHitPoints >= hitPoints) {
			return;	
		} else if (currentHitPoints + amt > hitPoints) {
			currentHitPoints = hitPoints;
		} else if (currentHitPoints + amt > threshold && down) {
			down = false;
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
				down = true;
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

	public HealthBay getHealthBay() {
		return this;
	}

	public bool isDown() {
		return down;
	}
	
}
