﻿using UnityEngine;
using System.Collections;

public class Scope : MonoBehaviour {

	public float defaultFOV = 0f;
	public float fovSpeed = 0f;
	public float fovIn = 0f;
	public bool gunAim = false;
	public GameObject playerCam;
	GameObject gunModel;
	string scopeInAnim;
	string scopeOutAnim;
	Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator> ();
		defaultFOV = playerCam.camera.fieldOfView;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(1)){
			anim.SetBool ("Zoom", true);
			gunAim = true;
		}
		if(Input.GetMouseButtonUp(1)){
			anim.SetBool("Zoom", false);
			gunAim = false;
		}

		if(gunAim){
			playerCam.camera.fieldOfView = Mathf.Lerp(playerCam.camera.fieldOfView, fovIn, fovSpeed * Time.deltaTime);
		}
		
		if(!gunAim){
			playerCam.camera.fieldOfView = Mathf.Lerp(playerCam.camera.fieldOfView, defaultFOV, fovSpeed * Time.deltaTime);
		}
	}
}
