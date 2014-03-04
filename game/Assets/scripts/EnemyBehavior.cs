﻿using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {
	public GameObject currentCell;
	public PlayerMovement target;
	public Transform targetTransform;
	public GameObject targetCell;
	public GameObject goalDoor;
	public int shortestPathSoFar;
	float waitToStart = 5;
	float currentMoveSpeed = 5;
	float maxMoveSpeed = 6;
	float minMoveSpeed = 1;
	float speedRecover = 1;
	float speedDamage = 2;
	Vector3 randomizeCourseVector;
	bool randomizedCourse = false;
	bool calculatedNewRandomizeCourseVector = false;

	void Awake(){
		shortestPathSoFar = int.MaxValue;
		target = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
		targetTransform = GameObject.FindWithTag("Player").transform;
		waitToStart = 5;
		randomizeCourseVector = transform.position;
	}

	void Update(){
		if (waitToStart <= 0){
			targetCell = target.currentCell;
			foreach(GameObject doorCheckingNow in currentCell.GetComponent<AIPathCell>().doors){
				for(int i = 0; i < doorCheckingNow.GetComponent<AIPathDoor>().cells.Count; i++){
					if(doorCheckingNow.GetComponent<AIPathDoor>().cells[i] == targetCell){
						if(doorCheckingNow.GetComponent<AIPathDoor>().doorsToCells[i] < shortestPathSoFar){
							goalDoor = doorCheckingNow;
							shortestPathSoFar = doorCheckingNow.GetComponent<AIPathDoor>().doorsToCells[i];
						}
					}
				}
			}
			shortestPathSoFar = int.MaxValue;
		}
		waitToStart -= 1;
		
		if (!calculatedNewRandomizeCourseVector){
			randomizeCourseVector = FindSpotInCell();
			calculatedNewRandomizeCourseVector = true;
		}

		if(currentCell != targetCell || targetCell == null){
			if(randomizedCourse){
				transform.position += (goalDoor.transform.position - transform.position).normalized * currentMoveSpeed * Time.deltaTime;
			}
			if (!randomizedCourse){
				transform.position += (randomizeCourseVector - transform.position).normalized * currentMoveSpeed * Time.deltaTime;
				if (Vector3.Distance(transform.position, randomizeCourseVector) < transform.localScale.x){
					if (goalDoor){
						randomizedCourse = true;
					}
					if (goalDoor == null){
						calculatedNewRandomizeCourseVector = false;
					}
				}
			}
		}

		if (targetCell == currentCell)
			transform.position += (targetTransform.position - transform.position).normalized * currentMoveSpeed * Time.deltaTime;

		if(currentMoveSpeed < maxMoveSpeed){
			currentMoveSpeed += speedRecover*Time.deltaTime;
		}
		if(currentMoveSpeed > maxMoveSpeed){
			currentMoveSpeed = maxMoveSpeed;
		}
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "AIPathCell"){
			currentCell = c.gameObject;
			randomizedCourse = false;
			calculatedNewRandomizeCourseVector = false;
		}
	}

	void OnTriggerStay(Collider c){
		if((c.tag == "Enemy" || c.tag == "Player") && c.gameObject != gameObject){
			if(currentMoveSpeed > minMoveSpeed){
				currentMoveSpeed -= speedDamage;
			}
			transform.position += (transform.position - c.transform.position).normalized * 0.1f;
		}
	}

	Vector3 FindSpotInCell(){
		return currentCell.transform.position + (currentCell.transform.rotation * new Vector3(
			Random.Range(currentCell.transform.localScale.x*(-0.5F), currentCell.transform.localScale.x*(0.5F)), 0, 
			Random.Range(currentCell.transform.localScale.z*(-0.5F), currentCell.transform.localScale.z*(0.5F))));

	}
}
