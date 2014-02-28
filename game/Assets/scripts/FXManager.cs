﻿using UnityEngine;
using System.Collections;

public class FXManager : MonoBehaviour {

	public AudioClip AssaultBulletSFX;

	[RPC]
	void AssaultBulletFX(Vector3 startPos) {
		Debug.Log ("BOOM");
		AudioSource.PlayClipAtPoint (AssaultBulletSFX, startPos);
	}

}
