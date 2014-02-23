using UnityEngine;
using System.Collections;

public class FP_Shooter : MonoBehaviour {

	public GameObject bullet_prefab;
	public Camera theCam;
	public GameObject theGun;
	float bulletPulse = 20f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Fire1")){
			theGun.animation["Default Take"].speed = 10;
			theGun.animation.Play();
			GameObject the_bullet = (GameObject) Instantiate (bullet_prefab, theCam.transform.position + theCam.transform.forward, theCam.transform.rotation);
			the_bullet.rigidbody.AddForce(theCam.transform.forward * bulletPulse, ForceMode.Impulse);
		}
	}
}
