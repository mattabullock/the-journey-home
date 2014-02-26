﻿using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;

	bool gotFirstUpdate = false;

	// Use this for initialization
	void Start () {
	
	}

	void OnGUI(){
		GUI.Box (new Rect(Screen.width/2,Screen.height/2, .5f, .5f),GUIContent.none);   	
	}
	
	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {

		}
		else {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
				//do something
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		} else {
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();

			if(gotFirstUpdate == false) {
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}

		}

	}
}
