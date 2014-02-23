using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("e")) {
			GameObject[] gos;
			
			gos = GameObject.FindGameObjectsWithTag("interactive");

			Debug.Log (gos);

			var fwd = transform.TransformDirection (Vector3.forward);
			RaycastHit hit;

			if (Physics.Raycast (transform.position, fwd, out hit)) {
				print ("There is something in front of the object!");
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
