using UnityEngine;
using System.Collections;

public class InteractedWith : MonoBehaviour {

	[RPC]
	public void repair() {
		Debug.Log ("is this working");
		//go.rigidbody.AddTorque(0,15,0);
	}

}
