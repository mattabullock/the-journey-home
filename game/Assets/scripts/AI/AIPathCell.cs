using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPathCell : MonoBehaviour {
	public List<GameObject> doors = new List<GameObject>();

	void OnTriggerEnter(Collider c){
		if(c.tag == "AIPathDoor"){
			doors.Add (c.gameObject);
		}
	}
}
