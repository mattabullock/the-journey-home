using UnityEngine;
using System.Collections;

public class Opening : MonoBehaviour {

	public GameObject asteroid;
	public GameObject spaceship;
	public GameObject plane;
	public float timer;
	public float startanimation = 6f;
	public float endanimation = 5.5f;
	bool showtext = true;
	bool play = false;
	bool showEndText = false;

	// Use this for initialization
	void Start () {
		timer = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void FixedUpdate() {
		timer += Time.deltaTime;
		if (timer > startanimation && !play && !showEndText) {
			plane.SetActive(false);
			asteroid.GetComponent<Animator>().SetBool("start", true);
			spaceship.GetComponent<Animator>().SetBool("start", true);
			showtext = false;
			play = true;
			timer = 0;
		}

		if (timer > endanimation && play && !showEndText) {
			asteroid.GetComponent<Animator> ().SetBool ("start", false);
			spaceship.GetComponent<Animator> ().SetBool ("start", false);
			plane.SetActive (true);
			timer = 0;
			showEndText = true;
			play = false;
		}

		if (timer > endanimation && !play && showEndText) {
			Application.LoadLevel("MainMenu");
		}
	}

	void OnGUI() {
		if (showtext) {
			GUI.Box (new Rect (Screen.width / 4, 4 * Screen.height / 10, Screen.width / 2, Screen.height / 10),
			         "All alone in space, \nfour crew members do their best to survive...");
		}

		if (showEndText) {
			GUI.Box (new Rect (Screen.width / 4, 4 * Screen.height / 10, Screen.width / 2, Screen.height / 10),
			         "The Journey Home");
		}
	}
}
