using UnityEngine;
using System.Collections;

//Note this line, if it is left out, the script won't know that the class 'Path' exists and it will throw compiler errors
//This line should always be present at the top of scripts which use %Pathfinding
using Pathfinding;
public class AstarAI : MonoBehaviour
{
	//The point to move to
	private Vector3 targetPosition;
	public GameObject target;
	private GameObject tempTarget;
	private float clockTick;
	private Seeker seeker;
	private	GameObject[] systems;
	private	GameObject[] players;
	private CharacterController controller;
	private bool newPath;
	private GameObject player;
	private Transform playerTrans;
	private Vector3 playerLoc;
	private bool exploring;
	private bool systemTarget;
	private float closest;
	private int count;
	private bool playerFound;
	private GameObject[] targets;
	private bool targetAquired;
	private bool targetLocked;

	//The calculated path
	public Path path;

	//The AI's speed per second
	public float speed = 100;

	//The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;

	//The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	public void Start ()
	{
		systems = GameObject.FindGameObjectsWithTag ("SystemSpawn");
		players = GameObject.FindGameObjectsWithTag ("Player");
		targets = new GameObject[24];
		count = 0;
		foreach (GameObject o in systems) {
			targets [count] = o;
			count++;
		}
		foreach (GameObject o in players) {
				targets [count] = o;
				count++;
		}
		count = 0;
		closest = 100000f;
		targetPosition = new Vector3 (0, 0, 0);
		target = null;
		systemTarget = false;
		clockTick = 0;
		exploring = true;
		targetAquired = false;
		seeker = GetComponent<Seeker> ();
		controller = GetComponent<CharacterController> ();
		decision ();
	}

	public void pathCalc ()
	{
		//Start a new path to the targetPosition, return the result to the OnPathComplete function
		seeker.StartPath (transform.position, targetPosition, OnPathComplete);
	}

	public void OnPathComplete (Path p)
	{
		Debug.Log ("Yey, we got a path back. Did it have an error? " + p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;	
			newPath = true;
		}
	}

	public void FixedUpdate ()
	{
		if (path == null) {
			return;
		}

		if (exploring) {
			if ((path.vectorPath.Count - currentWaypoint) < 2) {
				if (systemTarget) {
					foreach (GameObject o in systems) {
						if (o.transform.position.magnitude == target.transform.position.magnitude) {
							Debug.Log("found " + target); 
							o.GetComponent<SystemSpawn> ().enemyFound = true;
						}
					}
				}
			}
			if (newPath) {
				if (clockTick == 10) {
					decision ();
				} else {
					clockTick++;
				}
			}
		} 
		else {
			if ((path.vectorPath.Count - currentWaypoint) < 1) {
				if (targetAquired) {
					//attack the target
					//if the target hits 0 health targetAquired goes false.
				} 
				else {
					if (newPath) {
						if (clockTick == 10) {
							decision ();
						} else {
							clockTick++;
						}
					}
				}							
			}
		}
		if (currentWaypoint < path.vectorPath.Count) {
			//Direction to the next waypoint
			//Debug.Log ("" + path.vectorPath.Count + " and : " + currentWaypoint);
			Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
			dir *= speed * Time.fixedDeltaTime;
			controller.SimpleMove (dir);

			//Check if we are close enough to the next waypoint
			//If we are, proceed to follow the next waypoint
			if (Vector3.Distance (transform.position, path.vectorPath [currentWaypoint]) < nextWaypointDistance) {
					currentWaypoint++;
					return;
			}
		}
	}

	public void decision (){
		newPath = false;
		clockTick = 0;
		count = 0;
		if (exploring) {
			closest = 10000;
			foreach (GameObject o in systems) {
				SystemSpawn g = o.GetComponent<SystemSpawn> ();
				if (g.enemyFound) {
					Debug.Log("here");
					count++;
					if (count>4){
						exploring = false;
						Debug.Log("GOING AGGRESSIVE!!!!!!!");
					}
				}
				else {
					if ((Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude)) < closest) {
						Debug.Log ("" + o + " is closest, it is " + (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude)) + " units away.");
						closest = (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude));
						targetPosition = o.transform.position;
						target = o;
						systemTarget = true;
					}
				}
			}
			foreach (GameObject o in players) {
				if ((Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude)) < closest) {
					Debug.Log ("" + o + " is closest, it is " + (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude)) + " units away.");
					closest = (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude));
					targetPosition = o.transform.position;
					target = o;
					systemTarget = false;
					playerFound = true;
				}
			}
			Debug.Log ("" + target + " is closest");
			targetPosition = target.transform.position;
			pathCalc ();
		} 
		else {


			if (targetAquired) {
			} 
			else {
				int Thetarget = 0;		
				if (playerFound) {
					Thetarget = Random.Range (0, 23);
					if (Thetarget < 20){
						while (!targets[Thetarget].GetComponent<SystemSpawn>().enemyFound || targets[Thetarget].GetComponent<SystemSpawn>()) {
							Thetarget = Random.Range (0, 19);
							//if Thetarget has 0 health already, choose a new target
						}

				} 
				else {
					Thetarget = Random.Range (0, count - 1);
					//While Thetarget has 0 health, choose a new target.
				}

				if (Thetarget > 19) {
					systemTarget = false;
				}
				targetAquired = true;
				target = targets [Thetarget];
				pathCalc ();
			}	
		}
	}
} 