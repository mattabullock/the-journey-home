using UnityEngine;
using System.Collections;

public class GUIText : MonoBehaviour {
	const float MAX_OXYGEN = 30;
	const float MAX_TIMER = 30;

	float oxygen;
	float timer;
	bool oxygenProduction;
	bool isDead;
	CharacterController cc;

	// Use this for initialization
	void Start () {
		oxygen = MAX_OXYGEN;
		timer = MAX_TIMER;
		oxygenProduction = true;
		cc = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("e")){
			if(oxygenProduction){
				oxygenProduction = false;
			}
			else{
				oxygenProduction = true;
			}
		}

		if(oxygenProduction){
			oxygen += Time.deltaTime;
			if(oxygen > MAX_OXYGEN){
				oxygen = MAX_OXYGEN;
			}
			timer = MAX_TIMER;
		}
		else{
			oxygen -= Time.deltaTime;
			if(oxygen < 0){
				oxygen = 0;
				timer -= Time.deltaTime;
				if(timer <= 0){
					timer = 0;
				}
			}
		}

		if(timer == 0){
			Vector2 offset = new Vector2(0,0);
			guiText.pixelOffset = offset;
			guiText.text = "You have died.";
			cc.enabled = false;

		}
		else if(oxygen > 0){
			guiText.text = "Current Oxygen: "+oxygen.ToString("00");
		}
		else{
			if(timer > 0){
				guiText.text = "No oxygen.\nYou have "+timer.ToString("00.0")+" seconds until death.";
			}
			else{
				guiText.text = "You have suffocated to death.";
			}
		}
	}
}
