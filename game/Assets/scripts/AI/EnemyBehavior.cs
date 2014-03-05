using UnityEngine;
using System.Collections;

public class EnemyBehavior : Photon.MonoBehaviour {
	public GameObject currentCell;
	public NetworkCharacter target;
	public Transform targetTransform;
	public GameObject targetCell;
	public GameObject goalDoor;
	public int shortestPathSoFar;
	float waitToStart = 10;
	float currentMoveSpeed = 5;
	float maxMoveSpeed = 6;
	float minMoveSpeed = 1;
	float speedRecover = 1;
	float speedDamage = 2;
	Vector3 randomizeCourseVector;
	bool randomizedCourse = false;
	bool calculatedNewRandomizeCourseVector = false;
	bool isInstantiated = false;
	bool haveCell = false;
	Vector3 realPosition;
	Quaternion realRotation;
	bool gotFirstUpdate = false;
	GameObject currTarget;
	float damage = .1f;

	public float delay = 2f; 
	float cooldown = 0f;

	void Awake(){
		shortestPathSoFar = int.MaxValue;
		if (GameObject.FindWithTag ("Player") != null) {
			currTarget = FindTarget();
			target = currTarget.GetComponent<NetworkCharacter> ();
			targetTransform = currTarget.transform;
			isInstantiated = true;
		}
		randomizeCourseVector = transform.position;


	}

	void Update(){
		if (photonView.isMine) {
			Move ();
			AttackTarget();
		} else {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);
		}
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

	GameObject FindTarget() {
		return GameObject.FindWithTag ("Player");
	}

	void AttackTarget() {
		if(Vector3.Distance(transform.position, currTarget.transform.position) < 1) {
			currTarget.GetComponent<Health>().GetComponent<PhotonView>().RPC ("TakeDamage",PhotonTargets.All,damage);
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
			transform.position += (transform.position - c.transform.position).normalized * 1f;
		}
	}

	Vector3 FindSpotInCell(){
		if (!haveCell)
			return transform.position;
		return currentCell.transform.position + (currentCell.transform.rotation * new Vector3(
			Random.Range(currentCell.transform.localScale.x*(-0.5F), currentCell.transform.localScale.x*(0.5F)), 0, 
			Random.Range(currentCell.transform.localScale.z*(-0.5F), currentCell.transform.localScale.z*(0.5F))));

	}

	void Move() {
		if (!isInstantiated) {
			if(GameObject.FindWithTag ("Player") != null) {
				isInstantiated = true;
				target = GameObject.FindWithTag ("Player").GetComponent<NetworkCharacter> ();
				targetTransform = target.transform;
			} else { 
				return;
			}
		}
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
}