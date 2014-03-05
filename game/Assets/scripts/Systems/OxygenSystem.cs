using UnityEngine;
using System.Collections;

public class OxygenSystem : SystemBase {
	
	GameObject lights;
	
	const float flickerOn = 7f;
	const float flickerOff = 8f;

	float timer = 0f;
	public float delay = 3f;

	public float oxygenSupply = 100f;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
	}
	
	
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		timer += Time.deltaTime;
		if (down && timer >= delay) {
			GetComponent<PhotonView>().RPC ("depleteOxygen", PhotonTargets.All);
		}
	}
	
	protected override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			
		} else if (stream.isReading) {
			
		}
	}

	[RPC]
	public void depleteOxygen() {
		timer = 0;
		oxygenSupply--;
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
				down = true;
				belowThresh = true;
			}
		} else {
			if(!belowThresh) {

			}
			currentHitPoints -= amt;
		}	
		
		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}

	[RPC]
	protected void trigger() {
		Debug.Log ("Oxygen oh no!!");
	}
	
	public OxygenSystem getOxygenSystem() {
		return this;
	}
	
	public bool isDown() {
		return down;
	}
	
}
