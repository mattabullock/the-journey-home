using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

	public AudioClip AssaultBulletSFX;

	[RPC]
	void AssaultBulletFX(Vector3 startPos) {
		Debug.Log ("Boom.");
//		AudioSource.PlayClipAtPoint (AssaultBulletSFX, startPos);
	}

}
