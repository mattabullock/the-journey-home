using UnityEngine;
using System.Collections;

public class SystemBase : Photon.MonoBehaviour {
	
	public float hitPoints = 100f;
	public float currentHitPoints;
	public float healthBarLength = 200f;
	protected float currHealthBarLength;
	protected bool down;
	protected bool foundBool = false;
	protected bool belowThresh = false;
	protected Transform t;
	protected Vector3 realPosition = Vector3.zero;
	protected Quaternion realRotation = Quaternion.identity;
	public float threshold = 50f;
	public GameObject currentCell;

	
	// Use this for initialization
	protected virtual void Start () {
		t = GetComponent<Transform> ();
		currentHitPoints = hitPoints;
		currHealthBarLength = healthBarLength;
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "AIPathCell"){
			currentCell = c.gameObject;
		}
	}
	
	protected virtual void Update() {
		if (!photonView.isMine) {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);
		}
	}
	
	protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (t.position);
			stream.SendNext (t.rotation);
			stream.SendNext (currentHitPoints);
			stream.SendNext (down);
		} else if(stream.isReading) {
			realPosition = (Vector3) stream.ReceiveNext();
			realRotation = (Quaternion) stream.ReceiveNext();
			currentHitPoints = (float)stream.ReceiveNext ();
			down = (bool)stream.ReceiveNext ();
		}
	}

	[RPC]
	protected virtual void repair(float amt) {
		
	}
	
	[RPC]
	protected virtual void TakeDamage(float amt) {

	}

	[RPC]
	public void found() {

		if (foundBool) {
			// do nothing
		} else {
			Transform t = transform.FindChild("indicator");
			Transform tChild = t.FindChild("Plane");
			t.gameObject.layer = 9; // layer 9 should be NavMesh
			tChild.gameObject.layer = 9;
		}
	}	
	

	
}