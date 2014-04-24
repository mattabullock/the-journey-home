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
	private	SystemBase[] systems;
	private	GameObject[] players;
	private CharacterController controller;
	private bool newPath;
	private GameObject player;
	private Transform playerTrans;
	private Vector3 playerLoc;
	private bool systemTarget;
	private float closest;
	private float count;

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
		systems = FindObjectsOfType<SystemBase>();
		players = GameObject.FindGameObjectsWithTag ("Player");
		count = 0f;
		closest = 100000f;
		targetPosition = new Vector3 (0, 0, 0);
		target = null;
		systemTarget = false;
		clockTick = 0;
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


		if ((path.vectorPath.Count - currentWaypoint) < 2) {
			if (systemTarget) {
				if (count>1) {
					target.GetComponent<SystemBase> ().currentHitPoints = target.GetComponent<SystemBase> ().currentHitPoints - 1;
					count=0;
				} 
				else {
					count+=Time.deltaTime;
				}
			}
			else{
				if (count>1) {
					target.GetComponent<PlayerHealth> ().currentHitPoints = target.GetComponent<PlayerHealth> ().currentHitPoints - 1;
					count=0;
				} 
				else {
					count+=Time.deltaTime;
				}
			}
		}

		if (newPath) {
			if (clockTick >= 2) {
				decision ();
			} else {
				clockTick+=Time.deltaTime;
			}
		}
		Debug.Log ("" + path.vectorPath.Count + " and : " + currentWaypoint);

		if (currentWaypoint < path.vectorPath.Count) {
			//Direction to the next waypoint
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
		else {
		}
	}

	public void decision (){
		newPath = false;
		clockTick = 0;
		count = 0;
		closest = 10000000;
		foreach (SystemBase o in systems) {
			if (o.currentHitPoints==0) {
				Debug.Log("health is 0");
			} 
			else {
				if ((Mathf.Abs (o.gameObject.transform.position.magnitude - gameObject.transform.position.magnitude)) < closest) {
					Debug.Log ("" + o + " is closest, it is " + (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude)) + " units away.");
					closest = (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude));
					target = o.gameObject;
					systemTarget = true;
				}
			}
		}
		foreach (GameObject o in players) {
			if ((Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude)) < closest) {
				Debug.Log ("" + o + " is closest, it is " + (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude)) + " units away.");
				closest = (Mathf.Abs (o.transform.position.magnitude - gameObject.transform.position.magnitude));
				target = o;
				systemTarget = false;
			}
		}
		Debug.Log ("" + target + " is closest");
		targetPosition = target.transform.position;
		pathCalc ();
				
	}
}
