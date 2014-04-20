using UnityEngine;
using System.Collections;

public class Flashlight : MonoBehaviour {

	public Light l;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Flashlight")) {
			l.enabled = !l.enabled;
		}
	}
}
