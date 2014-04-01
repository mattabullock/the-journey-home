using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	
	public float fireRate = 0.1f;
	float cooldown = 0;
	public float damage = 25f;
	public GameObject decalHitWall;
	float floatInFrontOfWall = 0.0001f;
	public float ammo;
	public float maxAmmo = 20;
	float reloadCooldown = 3f;
	FXManager fx;
	public GameObject gun;
	Animator gunAnim;
	bool reloading = false;

	public GameObject muzzleFlash;

	public int NavMeshLayer;
	public int NavMeshMask;
	public int FinalMask;
	Transform bulletSpawn;

	void Start() {
		gunAnim = gun.GetComponent<Animator> ();
		fx = GameObject.FindObjectOfType<FXManager>();

		NavMeshLayer = 9;
		NavMeshMask = 1 << NavMeshLayer;
		FinalMask = ~NavMeshMask;
		ammo = maxAmmo;
		bulletSpawn = transform.FindChild("Main Camera").FindChild("Gun Camera").FindChild("M4A1").FindChild("BulletSpawn");
	}

	void Update () {
		cooldown -= Time.deltaTime;

		if(cooldown <= 0) {
			if(reloading) {
				ammo = maxAmmo;
				reloading = false;
			}
			if(Input.GetButton("Shoot") && !Input.GetButton ("Interact") && ammo > 0 && !reloading) {
				Fire ();
				gunAnim.SetTrigger("Shoot");
				GameObject holdMuzzleFlash;
				if(muzzleFlash != null) {
					holdMuzzleFlash = (GameObject)Instantiate(muzzleFlash, bulletSpawn.position, bulletSpawn.rotation);
					if(holdMuzzleFlash) {
						holdMuzzleFlash.transform.parent = transform;
					}
				}

			} else if ((Input.GetButton ("Shoot") && !Input.GetButton ("Interact") && ammo <= maxAmmo) || (Input.GetButton ("Reload") && ammo < maxAmmo)){
				reload();
			}
		}
		
	}
	
	void Fire() {
		fx.GetComponent<PhotonView>().RPC ("AssaultBulletFX", PhotonTargets.All, Camera.main.transform.position);
		RaycastHit hit;

		Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 1000000, FinalMask);

		Transform hitTransform = hit.transform;

		if (hitTransform != null && hitTransform.tag == "Level Part") {
			Instantiate(decalHitWall,  hit.point + 
			            (hit.normal * floatInFrontOfWall), Quaternion.LookRotation (hit.normal));
		}

		if(hitTransform != null) {
			if(hitTransform.tag == "interactive" || hitTransform.tag == "Enemy") {
				HealthBase h = hitTransform.GetComponent<HealthBase>();
				
				while(h == null && hitTransform.parent) {
					hitTransform = hitTransform.parent;
					h = hitTransform.GetComponent<HealthBase>();
				}

				if(h == null) {
					SystemBase sh = hitTransform.GetComponent<SystemBase>();
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
			
		}
		ammo--;
		cooldown = fireRate;
	}

	void reload() {
		reloading = true;
		cooldown = reloadCooldown;
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
