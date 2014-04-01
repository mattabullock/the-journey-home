using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPathCell : MonoBehaviour {
	public List<GameObject> doors = new List<GameObject>();
	public List<GameObject> inhabitants = new List<GameObject>();
	public int enemiesInside;
	public int playersInside;
	public bool hasSpawner;

	void Awake(){
		enemiesInside = 0;
		playersInside = 0;
		hasSpawner = false;
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "AIPathDoor"){
			doors.Add (c.gameObject);
		}
	}

	public void enter(GameObject o){
		inhabitants.Add(o);
		if(o.tag == "Player"){
			playersInside++;
		}
		else if(o.tag == "Enemy"){
			enemiesInside++;
		}
	}

	public void exit(GameObject o){
		inhabitants.Remove(o);
		if(o.tag == "Player"){
			playersInside--;
		}
		else if(o.tag == "Enemy"){
			enemiesInside--;
		}
	}
}
