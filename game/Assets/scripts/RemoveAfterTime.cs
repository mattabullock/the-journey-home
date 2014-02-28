using UnityEngine;
using System.Collections;

public class RemoveAfterTime : MonoBehaviour {

	public float removeTime = 20f;

	// Use this for initialization
	void Start () {
		StartCoroutine (Remove ());
	}

	IEnumerator Remove() {
		yield return new WaitForSeconds (removeTime);
		Destroy (gameObject);
	}
	
}
