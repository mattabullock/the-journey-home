using UnityEngine;
using System.Collections;

public class OxygenSystem : SystemBase {

	float timer = 0f;
	public float delay = .5f;

	public float oxygenSupply;
	public float maxOxygenSupply = 100f;
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		oxygenSupply = maxOxygenSupply;
	}
	
	
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		timer += Time.deltaTime;
		if (down && timer >= delay) {
			timer = 0;
			oxygenSupply--;
			if(oxygenSupply <= 0 && !SpawnManager.isGameOver) {
				SpawnManager.isGameOver = true;
			}
		}
		if(oxygenSupply < 0) {
			oxygenSupply = 0;
		}
	}
	
	protected override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {
			stream.SendNext(oxygenSupply);
		} else if (stream.isReading) {
			oxygenSupply = (float) stream.ReceiveNext();
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
				down = true;
				belowThresh = true;
			}
		} else {
			if(!belowThresh) {
				trigger ();
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
