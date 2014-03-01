using UnityEngine;
using System.Collections;

public class LightSystem : SystemBase {

	GameObject lights;

	const float flickerOn = 7f;
	const float flickerOff = 8f;

	// Use this for initialization
	protected override void Start () {
		base.Start ();
		lights = GameObject.FindGameObjectWithTag ("Lights");
	}


	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();
		if (down && lights.activeSelf) {
			lights.SetActive (false);
		} else if(!down && !lights.activeSelf) {
			lights.SetActive(true);
		}
	}

	protected override void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		base.OnPhotonSerializeView (stream, info);
		if (stream.isWriting) {

		} else if (stream.isReading) {

		}
	}

//	protected override void OnGUI() {
//		GUI.Box (new Rect(Screen.width - 10 - healthBarLength,10, currHealthBarLength, 20), GUIContent.none);
//		GUI.Box (new Rect(Screen.width - 10 - healthBarLength,10, healthBarLength, 20), currentHitPoints + "/" + hitPoints);
//	}

	[RPC]
	protected override void repair (float amt) {
		if (currentHitPoints >= hitPoints) {
			return;	
		} else if (currentHitPoints + amt > hitPoints) {
			currentHitPoints = hitPoints;
		} else if (currentHitPoints + amt > threshold && down) {
			currentHitPoints += amt;
			belowThresh = false;
			StartCoroutine (trigger(flickerOn));
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
				StartCoroutine (trigger(flickerOff));
				belowThresh = true;
			}
		} else {
			if(!belowThresh) {
				StartCoroutine (trigger(flickerOn));
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
