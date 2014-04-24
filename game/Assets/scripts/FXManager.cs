using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

	public AudioClip AssaultBulletSFX;
	public AudioClip AlienAttackSFX;
	public float volume = .2f;

	[RPC]
	void AssaultBulletFX(Vector3 startPos) {
//		Debug.Log ("Boom.");
		AudioSource.PlayClipAtPoint (AssaultBulletSFX, startPos, volume);
	}

	[RPC]
	void AlienAttackFX(Vector3 startPos) {
		AudioSource.PlayClipAtPoint (AlienAttackSFX, startPos, volume);
	}

}
