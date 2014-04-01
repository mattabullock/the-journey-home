using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyBehavior : Photon.MonoBehaviour {
	public GameObject currentCell;
	public GameObject targetCell;
	public GameObject target;
	public GameObject goalDoor;
	public int shortestPathSoFar = int.MaxValue;

	public Transform targetTransform;
	Vector3 realPosition;
	Vector3 lastPosition;
	Vector3 currentVelocity;
	Quaternion realRotation;

	public float Health = 10f;
	float damage = 5f;
	float attackTimer;
	float timeBetweenAttacks = 1f;
	float attackDistance = 2.5f;
	float maxVelocity = 0.1f;
	float maxForce = 0.1f;
	float mass = 30f;

	float timer = 0;
	float timeBetweenSphereCasts = 1;
	float waitToStart = 10;
	bool isInstantiated = false;
	bool haveCell = false;
	bool gotFirstUpdate = false;
	bool go = false;

	void Awake(){
		shortestPathSoFar = int.MaxValue;
		currentVelocity = new Vector3(0f,0f,0f);
		attackTimer = 0f;
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

		if(transform.position - lastPosition != Vector3.zero)
			transform.rotation = Quaternion.LookRotation(transform.position - lastPosition);
		else{
			Vector3 tmp = targetTransform.position - transform.position;
			tmp.y = 0;
			transform.rotation = Quaternion.LookRotation(tmp);
		}
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

	void OnTriggerEnter(Collider c){
		if(c.tag == "AIPathCell"){
			currentCell = c.gameObject;
			c.gameObject.GetComponent<AIPathCell>().enter(this.gameObject);
		}
		haveCell = true;
	}

	void OnTriggerExit(Collider c){
		if(c.tag == "AIPathCell"){
			c.gameObject.GetComponent<AIPathCell>().exit(this.gameObject);
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
				if(attackTimer >= timeBetweenAttacks){
					if(target.tag == "Player"){
						target.GetComponent<HealthBase>().GetComponent<PhotonView>().RPC ("TakeDamage",PhotonTargets.All,damage);
					}
					else{
						target.GetComponent<SystemBase>().GetComponent<PhotonView>().RPC ("TakeDamage",PhotonTargets.All,damage);
					}
					attackTimer = 0f;
				}
				else{
					attackTimer += Time.deltaTime;
				}
			}
		}
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
							go = true;
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
		
		if(go){
			if(currentCell != targetCell){
				steer(goalDoor.transform.position);
			}
			else if (targetCell == currentCell){
				if(Vector3.Distance(transform.position, targetTransform.position) > attackDistance){
					steer(targetTransform.position);
				}
			}
		}
	}

	void steer(Vector3 pos){
		Vector3 desiredVelocity = (pos - transform.position);
		if(targetCell == currentCell){
			desiredVelocity.y = 0f;
		}
		desiredVelocity = desiredVelocity.normalized * maxVelocity;

		Vector3 steering = desiredVelocity - currentVelocity;

		if(steering.magnitude > maxForce){
			steering = steering.normalized * maxForce;
		}
		steering = steering / mass;

		currentVelocity = currentVelocity + steering;
		if(currentVelocity.magnitude > maxVelocity){
			currentVelocity = currentVelocity.normalized * maxVelocity;
		}

		RaycastHit r = new RaycastHit();
		if(Physics.Raycast(transform.position, currentVelocity, out r, attackDistance, ~(1 << 9))){
			if(r.collider.gameObject.tag != "Player"){
				Vector3 avoidance = r.normal;
				if(targetCell == currentCell){
					avoidance.y = 0f;
				}
				avoidance = avoidance.normalized;

				float scalarProjection = Vector3.Dot(avoidance, currentVelocity);
				Vector3 avoidanceVelocity = avoidance * scalarProjection;
				avoidanceVelocity = currentVelocity - avoidanceVelocity;

				currentVelocity = Vector3.Lerp(currentVelocity, avoidanceVelocity, r.distance/attackDistance).normalized * maxVelocity * Time.deltaTime;
			}
		}
		 
		transform.position = transform.position + currentVelocity;
	}
}