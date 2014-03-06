using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehavior : Photon.MonoBehaviour {
	public GameObject currentCell;
	public GameObject targetCell;

	public GameObject target;
	public Transform targetTransform;

	public GameObject goalDoor;
	public int shortestPathSoFar = int.MaxValue;

	float timer = 0;
	float timeBetweenSphereCasts = 1;
	float waitToStart = 10;
	float currentMoveSpeed = 5;
	float maxMoveSpeed = 6;
	float minMoveSpeed = 1;
	float speedRecover = 1;
	float speedDamage = 2;
	float attackDistance = 2.5f;

	public float Health = 10;
	float damage = .1f;

	Vector3 randomizeCourseVector;
	Vector3 realPosition;
	Vector3 lastPosition;
	Quaternion realRotation;

	bool randomizedCourse = false;
	bool calculatedNewRandomizeCourseVector = false;
	bool isInstantiated = false;
	bool haveCell = false;
	bool gotFirstUpdate = false;

	void Awake(){
		shortestPathSoFar = int.MaxValue;

		randomizeCourseVector = transform.position;
	}

	void Update(){
		timer += Time.deltaTime;

		if (photonView.isMine) {
			if(target == null){
				FindTarget();
			}
			if(target != null){
				if(target.tag != "Player" && target.GetComponent<SystemBase>().currentHitPoints == 0){
					FindTarget();
				}
			}

			if(timer > timeBetweenSphereCasts){
				timer = 0;
				bool anyHit = false;
				List<GameObject> newTargets = new List<GameObject>();

				GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
				foreach(GameObject o in players){
					RaycastHit[] hits = Physics.RaycastAll(transform.position, o.transform.position - transform.position, Vector3.Distance(transform.position, o.transform.position));
					foreach(RaycastHit hit in hits){
						if(hit.transform.tag == "Level Part")
							anyHit = true;
						if(!anyHit)
							newTargets.Add(o);
					}
					if(newTargets.Count > 0){
						float dist = float.MaxValue;
						foreach(GameObject p in newTargets){
							float newDist = Vector3.Distance(transform.position, p.transform.position);
							if(newDist < dist){
								target = p;
								targetTransform = p.transform;
								dist = newDist;
							}
						}
					}
				}
			}
			Move ();
			AttackTarget();
		} 
		else {
			transform.position = Vector3.Lerp (transform.position, realPosition, 10*Time.deltaTime);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 10*Time.deltaTime);
		}

		transform.rotation = Quaternion.LookRotation(transform.position - lastPosition);
		lastPosition = transform.position;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
		} else if(stream.isReading) {
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			
			if(gotFirstUpdate == false) {
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}
		}
	}

	void FindTarget() {
		List<GameObject> targets = new List<GameObject>();

		GameObject[] systems = GameObject.FindGameObjectsWithTag("interactive");
		foreach(GameObject o in systems){
			if(o.GetComponent<SystemBase>().currentHitPoints > 0)
				targets.Add(o);
		}
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject o in players){
			targets.Add(o);
		}

		if(targets.Count == 0){
			target = null;
			return;
		}
		target = targets[0];
		targetTransform = target.transform;
		if(target.tag == "Player"){
			targetCell = target.GetComponent<NetworkCharacter>().currentCell;
		}
		else {
			targetCell = target.GetComponent<SystemBase>().currentCell;
		}
		float dist = Vector3.Distance(target.transform.position, transform.position);
		foreach(GameObject o in targets){
			float tmpDist = Vector3.Distance(o.transform.position, transform.position);
			if(tmpDist < dist){
				target = o;
				targetTransform = target.transform;
				dist = tmpDist;
			}
		}

		isInstantiated = true;
	}

	void AttackTarget() {
		if(target != null){
			if(Vector3.Distance(transform.position, target.transform.position) <= attackDistance) {
				if(target.tag == "Player"){
					target.GetComponent<HealthBase>().GetComponent<PhotonView>().RPC ("TakeDamage",PhotonTargets.All,damage);
				}
				else{
					target.GetComponent<SystemBase>().GetComponent<PhotonView>().RPC ("TakeDamage",PhotonTargets.All,damage);
				}
			}
		}
	}

	void OnTriggerEnter(Collider c){
		if(c.tag == "AIPathCell"){
			currentCell = c.gameObject;
			randomizedCourse = false;
			calculatedNewRandomizeCourseVector = false;
		}
		haveCell = true;
	}

	void OnTriggerStay(Collider c){
		if((c.tag == "Enemy" || c.tag == "Player") && c.gameObject != gameObject){
			if(currentMoveSpeed > minMoveSpeed){
				currentMoveSpeed -= speedDamage;
			}
			transform.position += (transform.position - c.transform.position).normalized;
		}
	}

	Vector3 FindSpotInCell(){
		if (!haveCell)
			return transform.position;
		return currentCell.transform.position + (currentCell.transform.rotation * new Vector3(
			Random.Range(Mathf.Min(currentCell.transform.localScale.x * -0.5f, 3), Mathf.Min(currentCell.transform.localScale.x * 0.5f, 3)),
			0,
			Random.Range(Mathf.Min(currentCell.transform.localScale.z * -0.5f, 3), Mathf.Min(currentCell.transform.localScale.z * 0.5f, 3))));
	}

	void Move() {
		if (!isInstantiated) {
			return;
		}
		if (waitToStart <= 0){
			if(target == null){
				targetCell = currentCell;
				targetTransform = transform;
			}
			else{
				if(target.tag == "Player"){
					targetCell = target.GetComponent<NetworkCharacter>().currentCell;
				}
				else {
					targetCell = target.GetComponent<SystemBase>().currentCell;
				}
			}
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
		else{
			waitToStart -= 1;
		}
		
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
		
		if (targetCell == currentCell){
			if(Vector3.Distance(targetTransform.position, transform.position) > attackDistance){
				transform.position += (targetTransform.position - transform.position + new Vector3(0f,.5f, 0f)).normalized * currentMoveSpeed * Time.deltaTime;
			}
		}
		
		if(currentMoveSpeed < maxMoveSpeed){
			currentMoveSpeed += speedRecover*Time.deltaTime;
		}
		if(currentMoveSpeed > maxMoveSpeed){
			currentMoveSpeed = maxMoveSpeed;
		}
	}
}