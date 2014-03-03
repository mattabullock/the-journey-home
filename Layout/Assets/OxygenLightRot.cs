using UnityEngine;
using System.Collections;

public class OxygenLightRot : MonoBehaviour {

	float rotateSpeed = 180f;
	// Use this for initialization
	void Start () {
	
	}
	
	void Update() {

		transform.Rotate (rotateSpeed * Time.deltaTime, 0, 0) ;
	}
}
