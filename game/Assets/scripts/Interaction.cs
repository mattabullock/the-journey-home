using UnityEngine;
using System.Collections;

public class Interaction : MonoBehaviour {
	
	public float delay = .1f; 
	public float cooldown = 0f;
	public float repairDistance = 5f;

	// Update is called once per frame
	void Update () {
		cooldown -= Time.deltaTime;
		if (Input.GetButton ("Interact") && !Input.GetButton ("Shoot")) {
			interact();
		}
	}

	void interact() {

		if(cooldown > 0) {
			return;
		}

		RaycastHit hit;
		Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000);

		if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit, 10000) && hit.transform.gameObject.tag == "interactive") {
			var go = hit.point;
			var position = transform.position;
			var dist = go - position;
			var absDist = dist.sqrMagnitude;
			
			if(absDist < repairDistance) {
				var component = hit.transform.GetComponent<SystemBase>();
				component.GetComponent<PhotonView>().RPC ("repair", PhotonTargets.All, 1f);
			}
		}

		cooldown = delay;
	}
}
	