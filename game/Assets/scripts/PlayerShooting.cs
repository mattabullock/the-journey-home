using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {
	
	float cooldown = 0;
	public GameObject decalHitWall;
	float floatInFrontOfWall = 0.0001f;

	FXManager fx;
	Transform gun;
	GunBase gunBase;
	Animator gunAnim;
	bool reloading = false;
	public AudioClip reloadSound;

	public GameObject muzzleFlash;

	public AudioSource audioSource;

	public int NavMeshLayer;
	public int NavMeshMask;
	public int FinalMask;
	Transform bulletSpawn;

	void Start() {
		fx = GameObject.FindObjectOfType<FXManager>();
		audioSource = GetComponent<AudioSource> ();
		gun = transform.FindChild ("Main Camera").FindChild ("Gun Camera").FindChild ("M4A1");
		gunBase = gun.GetComponent<GunBase>();
		gunAnim = gun.GetComponent<Animator> ();
		NavMeshLayer = 9;
		NavMeshMask = 1 << NavMeshLayer;
		FinalMask = ~NavMeshMask;
		bulletSpawn = gun.FindChild("BulletSpawn");
	}

	void Update () {

		if (Input.GetButton ("SecondaryWeapon") && gun.tag != "SecondaryWeapon") {
			gun.gameObject.SetActive(false);
			gun = transform.FindChild ("Main Camera").FindChild ("Gun Camera").FindChild ("G36-C");
			gunBase = gun.GetComponent<GunBase>();
			gunAnim = gun.GetComponent<Animator> ();
			bulletSpawn = gun.FindChild("BulletSpawn");
			gun.gameObject.SetActive(true);

		} else if (Input.GetButton ("PrimaryWeapon") && gun.tag != "PrimaryWeapon") {
			gun.gameObject.SetActive(false);
			gun = transform.FindChild ("Main Camera").FindChild ("Gun Camera").FindChild ("M4A1");
			gunBase = gun.GetComponent<GunBase>();
			gunAnim = gun.GetComponent<Animator> ();
			bulletSpawn = gun.FindChild("BulletSpawn");
			gun.gameObject.SetActive(true);
		}

		cooldown -= Time.deltaTime;

		if(cooldown <= 0) {
			if(reloading) {
				gunBase.ammo = gunBase.maxAmmo;
				reloading = false;
			}
			if(Input.GetButton("Shoot") && !Input.GetButton ("Interact") && gunBase.ammo > 0 && !reloading) {
				Fire ();
				gunAnim.SetTrigger("Shoot");
				GameObject holdMuzzleFlash;
				if(muzzleFlash != null) {
					holdMuzzleFlash = (GameObject)Instantiate(muzzleFlash, bulletSpawn.position, bulletSpawn.rotation);
					if(holdMuzzleFlash) {
						holdMuzzleFlash.transform.parent = transform;
					}
				}

			} else if ((Input.GetButton ("Shoot") && !Input.GetButton ("Interact") && gunBase.ammo <= gunBase.maxAmmo) || (Input.GetButton ("Reload") && gunBase.ammo < gunBase.maxAmmo)){
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
							pv.RPC ("TakeDamage", PhotonTargets.AllBuffered, gunBase.damage);
						}
					}
				}
				else if(h != null) {
					PhotonView pv = h.GetComponent<PhotonView>();
					if(pv==null) {
						Debug.LogError("Freak out!");
					}
					else {
						pv.RPC ("TakeDamage", PhotonTargets.AllBuffered, gunBase.damage);
					}	
				}
			} else if(hitTransform.tag == "AlienSpawnPoint") {
				EnemySpawnManager esm = hitTransform.GetComponent<EnemySpawnManager>();
				PhotonView pv = esm.GetComponent<PhotonView>();
				if(pv==null) {
					Debug.LogError("Freak out!");
				}
				else {
					pv.RPC ("TakeDamage", PhotonTargets.AllBuffered, gunBase.damage);
				}	
			}
		}
		gunBase.ammo--;
		cooldown = gunBase.fireRate;
	}

	void reload() {
		if(reloadSound) {
			audioSource.PlayOneShot(reloadSound, 1f);
		}
		reloading = true;
		cooldown = gunBase.reloadCooldown;
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

	public GunBase getGun() {
		return gunBase;
	}
}
