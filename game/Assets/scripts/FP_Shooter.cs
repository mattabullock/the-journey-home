using UnityEngine;
using System.Collections;

public class FP_Shooter : MonoBehaviour {

	public GameObject bullet_prefab;
	public Camera theCam;
	public GameObject theGun;
	float bulletPulse = 20f;

	// Use this for initialization
	void Start () {
		theCam = gameObject.transform.FindChild ("Main Camera").gameObject.camera;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")){
			theGun.animation["Default Take"].speed = 10;
			theGun.animation.Play();
			GameObject the_bullet = (GameObject) PhotonNetwork.Instantiate ("bullet", theCam.transform.position + theCam.transform.forward, theCam.transform.rotation, 0);
			the_bullet.rigidbody.AddForce(theCam.transform.forward * bulletPulse, ForceMode.Impulse);
		}
	}
}
