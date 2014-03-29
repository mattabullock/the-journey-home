using UnityEngine;
using System.Collections;

public class EngineeringBay : SystemBase {
	
	
	// Use this for initialization
	protected override void Start () {
		base.Start ();
		down = true;
		currentHitPoints = 0;
	}
	
	
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (!down && SpawnManager.repairDelay == .1f) {
			Debug.Log ("repair faster!");
			SpawnManager.repairDelay = .07f;
		} else if(down && SpawnManager.repairDelay == .07f) {
			Debug.Log ("repair slower!");
			SpawnManager.repairDelay = .1f;
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
		} else if (currentHitPoints + amt > threshold && down) {
			down = false;
			currentHitPoints += amt;
			belowThresh = false;
			//			StartCoroutine (trigger(flickerOn));
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
				//				StartCoroutine (trigger(flickerOff));
				belowThresh = true;
			}
		} else {
			if(!belowThresh) {
				//				StartCoroutine (trigger(flickerOn));
			}
			currentHitPoints -= amt;
		}	
		
		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}
	
	protected IEnumerator trigger(float times) {
		bool curr = false;
		float i = 0f;
		float start = times / 100f;
		while (i < times) {
			yield return new WaitForSeconds (start - (i * .01f));
			down = curr;
			curr = !curr;
			i++;
		}
	}
	
}
