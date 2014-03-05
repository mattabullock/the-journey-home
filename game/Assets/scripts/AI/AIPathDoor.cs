using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AIPathDoor : MonoBehaviour {
	public List<GameObject> cells = new List<GameObject>();
	public List<int> doorsToCells = new List<int>();
	public List<GameObject> immediateCells = new List<GameObject>();
	public bool testForCells = true;
	public float waitToTest = 10;
	public int stage = 1;

	void Awake () {
		GameObject[] tmp = GameObject.FindGameObjectsWithTag("AIPathCell");
		foreach(GameObject o in tmp){
			cells.Add(o);
		}
		for(int i = 0; i < cells.Count; i++){
			doorsToCells.Add(int.MaxValue);
		}
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "AIPathCell"){
			immediateCells.Add (c.gameObject);
		}
	}

	void Update () {
		if(testForCells && waitToTest <= 0){
			foreach(GameObject immediateCell in immediateCells){
				for(int i = 0; i < cells.Count; i++){
					if(cells[i] == immediateCell)
						doorsToCells[i] = 1;
				}
			}

			for(stage = 2; stage < cells.Count; stage++){
				for(int i = 0; i < cells.Count; i++){
					if(doorsToCells[i] == stage-1){
						foreach(GameObject checkDoor in cells[i].GetComponent<AIPathCell>().doors){
							if(checkDoor != gameObject){
								foreach(GameObject checkCell in checkDoor.GetComponent<AIPathDoor>().immediateCells){
									for(int j = 0; j < cells.Count; j++){
										if(cells[j] == checkCell && doorsToCells[j] == int.MaxValue)
											doorsToCells[j] = stage;
									}
								}
							}
						}
					}
				}
			}
			testForCells = false;
		}
		else{
			waitToTest -= Time.deltaTime;
		}
	}
}
