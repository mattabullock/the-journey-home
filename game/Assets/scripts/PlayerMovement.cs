using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public GameObject currentCell;

	void OnTriggerStay(Collider c){
		if(c.tag == "AIPathCell"){
			currentCell = c.gameObject;
		}
	}
}
