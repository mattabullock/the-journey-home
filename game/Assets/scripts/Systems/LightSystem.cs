using UnityEngine;
using System.Collections;

public class LightSystem : SystemBase {

	Transform lights;
	bool changed = false;

	const float flickerOn = 7f;
	const float flickerOff = 8f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		currentHitPoints = 0;
		down = true;
		lights = GameObject.FindGameObjectWithTag ("Lights").transform;
		foreach (Transform child in lights) {
			child.light.color = Color.white * 0f;
		}
	}


	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if(changed) {
			foreach (Transform child in lights) {
				child.light.color = Color.white * (currentHitPoints/threshold);
			}
			changed = false;
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
		} else if (currentHitPoints + amt < threshold) {
			currentHitPoints += amt;
			changed = true;
//			belowThresh = false;
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
//			if(!belowThresh) {
//				StartCoroutine (trigger(flickerOff));
//				belowThresh = true;
//			}
		} else {
//			if(!belowThresh) {
//				StartCoroutine (trigger(flickerOn));
//			}
			currentHitPoints -= amt;
			if(currentHitPoints < threshold) {
				changed = true;
			}
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
