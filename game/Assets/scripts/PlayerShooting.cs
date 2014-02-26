﻿using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	
	public float fireRate = 0.5f;
	float cooldown = 0;
	public float damage = 25f;
	public GameObject decalHitWall;
	float floatInFrontOfWall = 0.0001f;
	
	void Update () {
		cooldown -= Time.deltaTime;
		
		if(Input.GetButton("Fire1") && !Input.GetButton ("Interact")) {
			Fire ();
		}
		
	}
	
	void Fire() {
		if(cooldown > 0) {
			return;
		}
		
		Debug.Log ("Firing our gun!");
		
		RaycastHit hit;

		Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000000);

		Transform hitTransform = hit.transform;

		if (hitTransform != null && hitTransform.tag == "Level Part") {
			Debug.Log("DO THIS");
			Instantiate(decalHitWall,  hit.point + 
			            (hit.normal * floatInFrontOfWall), Quaternion.LookRotation (hit.normal));
//			PhotonNetwork.Instantiate ("wallGunshotDecal", hit.point + 
//					(hit.normal * floatInFrontOfWall), Quaternion.LookRotation (hit.normal), 10);
		}

		if(hitTransform != null) {
			Debug.Log ("We hit: " + hitTransform.name);
						
			Health h = hitTransform.GetComponent<Health>();
			
			while(h == null && hitTransform.parent) {
				hitTransform = hitTransform.parent;
				h = hitTransform.GetComponent<Health>();
			}

			if(h == null) {
				SystemHealth sh = hitTransform.GetComponent<SystemHealth>();
				if(sh != null) {
					PhotonView pv = sh.GetComponent<PhotonView>();
					if(pv==null) {
						Debug.LogError("Freak out!");
					}
					else {
						sh.GetComponent<PhotonView>().RPC ("TakeDamage", PhotonTargets.AllBuffered, damage);
					}
				}
			}
			else if(h != null) {
				PhotonView pv = h.GetComponent<PhotonView>();
				if(pv==null) {
					Debug.LogError("Freak out!");
				}
				else {
					h.GetComponent<PhotonView>().RPC ("TakeDamage", PhotonTargets.AllBuffered, damage);
				}	
			}
			
			
		}
		
		cooldown = fireRate;
	}
	
	Transform FindClosestHitObject(Ray ray, out Vector3 hitPoint) {
		
		RaycastHit[] hits = Physics.RaycastAll(ray);
		
		Transform closestHit = null;
		float distance = 0;
		hitPoint = Vector3.zero;
		
		foreach(RaycastHit hit in hits) {
			if(hit.transform != this.transform && ( closestHit==null || hit.distance < distance ) ) {
				closestHit = hit.transform;
				distance = hit.distance;
				hitPoint = hit.point;
			}
		}

		return closestHit;
		
	}
}
