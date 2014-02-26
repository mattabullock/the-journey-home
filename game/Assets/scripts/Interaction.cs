using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact")) {

			var fwd = transform.TransformDirection (Vector3.forward);
			RaycastHit hit;

			Physics.Raycast (transform.position, fwd, out hit);

			if (hit.transform.gameObject.tag == "interactive") {
				var go = hit.transform.gameObject;
				var position = transform.position;
				var dist = go.transform.position - position;
				var absDist = dist.sqrMagnitude;

					if(absDist < 50) {
						Debug.Log ("lolwut");
						
						var component = hit.transform.GetComponent<InteractedWith>();
						component.GetComponent<PhotonView>().RPC ("repair", PhotonTargets.All, null);
					}
			}
		}

	}
}
