using UnityEngine;
using System.Collections;

public class SystemHealth : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	float currentHitPoints;
	public float healthBarLength;
	float currHealthBarLength;
	GameObject lights;
	bool down;
	bool belowThresh = false;
	Transform t;
	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	public float threshold = 50f;

	const float flickerOn = 7f;
	const float flickerOff = 8f;
	
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform> ();
		lights = GameObject.FindGameObjectWithTag ("Lights");
		currentHitPoints = hitPoints;
		currHealthBarLength = healthBarLength;
	}

	void Update() {
		if (down && lights.activeSelf) {
			lights.SetActive (false);
		} else if(!down && !lights.activeSelf) {
			lights.SetActive(true);
		}
		
		if (!photonView.isMine) {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (t.position);
			stream.SendNext (t.rotation);
			stream.SendNext (down);
			stream.SendNext (currentHitPoints);
		} else if(stream.isReading) {
			realPosition = (Vector3) stream.ReceiveNext();
			realRotation = (Quaternion) stream.ReceiveNext();
			down = (bool) stream.ReceiveNext();
			currentHitPoints = (float) stream.ReceiveNext();
		}
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
		} else if (currentHitPoints + amt > threshold && down) {
			currentHitPoints += amt;
			belowThresh = false;
			StartCoroutine (flickerLights(flickerOn));
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
			if(!belowThresh) {
				StartCoroutine (flickerLights(flickerOff));
				belowThresh = true;
			}
		} else {
			if(!belowThresh) {
				StartCoroutine (flickerLights(flickerOn));
			}
			currentHitPoints -= amt;
		}	

		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}

	IEnumerator flickerLights(float times) {
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
