using UnityEngine;
using System.Collections;

public class GunBase : MonoBehaviour {

	public float maxAmmo = 20;
	public float reloadCooldown = 3f;
	public float fireRate = 0.1f;
	public float damage = 25f;
	public float ammo;


	void Start () {
		ammo = maxAmmo;
	} 

}
