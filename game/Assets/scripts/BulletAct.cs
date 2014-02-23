using UnityEngine;
using System.Collections;

public class BulletAct : MonoBehaviour {

	float lifespan = 3.0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifespan -= Time.deltaTime;
		if (lifespan <= 0) {
			Explode();
		}
	}

	void OnCollisionEnter (Collision collision) {
		print (collision.gameObject.tag);

		if (collision.gameObject.tag == "Enemy") {
			Destroy(gameObject);
			Destroy(collision.gameObject);
		}
	}

	void Explode () {
		Destroy (gameObject);
	}
}
