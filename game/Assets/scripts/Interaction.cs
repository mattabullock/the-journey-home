using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Interact")) {
			GameObject[] gos;
			
			gos = GameObject.FindGameObjectsWithTag("interactive");

			var fwd = transform.TransformDirection (Vector3.forward);
			RaycastHit hit;

			Physics.Raycast (transform.position, fwd, out hit);

			if (hit.transform.gameObject.tag == "interactive") {
				//print ("There is something in front of the object!");
				foreach(GameObject go in gos){
					var position = transform.position;
					var dist = go.transform.position - position;
					var absDist = dist.sqrMagnitude;

					if(absDist < 50) {
						go.rigidbody.AddTorque(0,15,0);
					}
				}
			}
		}

	}
}
