using UnityEngine;
using System.Collections;

public class SystemHealth : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	float currentHitPoints;
	public float healthBarLength;
	float currHealthBarLength;
	
	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
		currHealthBarLength = healthBarLength;
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
			photonView.RPC ("SystemDown", PhotonTargets.All);
		} else {
			currentHitPoints -= amt;
		}	

		currHealthBarLength = healthBarLength * (currentHitPoints / hitPoints);
	}

	[RPC]
	void SystemDown() {
		Debug.Log ("The system is down.");
	}

}
