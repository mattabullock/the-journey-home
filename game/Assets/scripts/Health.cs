using UnityEngine;
using System.Collections;

public class Health : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	public float currentHitPoints;
	public Animator anim;
	float healthBarLength = 60f;

	// Use this for initialization
	void Start () {
		currentHitPoints = hitPoints;
	}

	void OnGUI() {
		GUI.Box (new Rect(Screen.width - 10 - healthBarLength, 70, healthBarLength, 20), Mathf.Floor(currentHitPoints) + "/" + hitPoints);
	}
	
	[RPC]
	public void TakeDamage(float amt) {
		if (currentHitPoints <= 0) {
			currentHitPoints = 0;
			return;
		}

		currentHitPoints -= amt;
		
		if(currentHitPoints <= 0) {
			anim.SetBool("Dead", true);
			GetComponent<NetworkCharacter>().enabled = false;
			GetComponent<MouseLook>().enabled = false;
			GetComponentInChildren<MouseLook>().enabled = false;
			StartCoroutine(dieAnim ());
		}
	}

	IEnumerator dieAnim() {
		yield return new WaitForSeconds (5);
		Die ();
		if (photonView.isMine) {
			SpawnManager.dead = true;
		}
	}

	void Die() {
		if( photonView.isMine ) {
			PhotonNetwork.Destroy(gameObject);
		}
	}
}
