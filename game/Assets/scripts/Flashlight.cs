using UnityEngine;
using System.Collections;

public class Flashlight : Photon.MonoBehaviour {

	public Light l;
	public Camera cam;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Flashlight")) {
			l.enabled = !l.enabled;
		}
		l.transform.rotation = cam.transform.rotation;
	}

	protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (l.enabled);
		} else if (stream.isReading) {
			l.enabled = (bool)stream.ReceiveNext ();
		}
	}
}
