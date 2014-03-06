using UnityEngine;
using System.Collections;

public class EngineSystem : SystemBase {

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		currentHitPoints = 0;
	}
	
	
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if(currentHitPoints >= 100 && !SpawnManager.win) {
			SpawnManager.win = true;
		}
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
		} else {
			currentHitPoints -= amt;
			trigger ();
		}	
		
		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}
	
	[RPC]
	protected void trigger() {
		Debug.Log ("System oh no!!");
	}
	
	public EngineSystem getEngineSystem() {
		return this;
	}
	
	public bool isDown() {
		return down;
	}
	
}

