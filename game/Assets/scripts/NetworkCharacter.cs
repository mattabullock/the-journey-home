using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	public float maxSpeed = 5.668f;
	float minSpeed = -2f;
	float vertVelocity = 0f;
	public float jumpSpeed = 5f;
	public float sprintMultiplier = 2f;
	float sprintTimer = 0f;
	public float maxSprint = 5f;
	bool sprinting = false;

	Vector3 direction = Vector3.zero;
	CharacterController cc;
	public Animator anim;
	public GameObject gun;
	Animator gunAnim;
	OxygenSystem oSys;

	PlayerHealth pHealth;

	bool gotFirstUpdate = false;

	public GameObject currentCell;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		gunAnim = gun.GetComponent<Animator> ();
		pHealth = GetComponent<PlayerHealth> ();
		oSys = GameObject.FindObjectOfType<OxygenSystem> ();
	}

	void OnTriggerStay(Collider c){
		if(c.tag == "AIPathCell"){
			currentCell = c.gameObject;
		}
	}

	void OnGUI(){
		GUI.Box (new Rect (Screen.width - pHealth.healthBarLength - 10, 10, 
		                   	pHealth.currentHitPoints*pHealth.healthBarLength/pHealth.hitPoints, 20), 
		         			Mathf.Floor (pHealth.currentHitPoints) + "/" + pHealth.hitPoints);

		GUI.Box (new Rect (Screen.width - pHealth.healthBarLength - 10, 40, 
		                   	oSys.currentHitPoints*pHealth.healthBarLength/oSys.hitPoints, 20), 
		         			Mathf.Floor (oSys.currentHitPoints) + "/" + oSys.hitPoints);

		GameObject[] systems = GameObject.FindGameObjectsWithTag("interactive");
		foreach(GameObject g in systems) {
			Vector3 transformPoint = g.transform.position;
			transformPoint.y += 2.2f;
			Vector3 groundPoint = transformPoint;
			groundPoint.y = 0;
			Vector3 pGroundPoint = transform.position;
			pGroundPoint.y = 0;
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(transformPoint);
			screenPoint.x -= Screen.width/35;
			if (screenPoint.z > 0f && (pGroundPoint - groundPoint).magnitude < 10f 
			    && (pGroundPoint - groundPoint).magnitude > 0) {
				float currHP = g.GetComponent<SystemBase>().currentHitPoints;
				float maxHP = g.GetComponent<SystemBase>().hitPoints;
				GUI.Box(new Rect(screenPoint.x, Screen.height - screenPoint.y, 60, 20), Mathf.Floor(currHP) + "/" + maxHP);
			}
		}

		GameObject[] gos = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject g in gos) {
			Vector3 transformPoint = g.transform.position;
			transformPoint.y += 2.2f; //puts it above their heads
			Vector3 groundPoint = transformPoint;
			groundPoint.y = 0;
			Vector3 pGroundPoint = transform.position;
			pGroundPoint.y = 0;
			Vector3 screenPoint = Camera.main.WorldToScreenPoint(transformPoint);
			screenPoint.x -= Screen.width/35; //mostly centers it
			if (screenPoint.z > 0f && (pGroundPoint - groundPoint).magnitude < 10f 
			    && (pGroundPoint - groundPoint).magnitude > 0) {
				float currHP = g.GetComponent<HealthBase>().currentHitPoints;
				float maxHP = g.GetComponent<HealthBase>().hitPoints;
				GUI.Box(new Rect(screenPoint.x, Screen.height - screenPoint.y, 60, 20), Mathf.Floor(currHP) + "/" + maxHP);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			if(Input.GetButtonDown("Sprint") && !Input.GetButton ("Scope"))
				sprinting = true;
			else if(Input.GetButtonUp ("Sprint") || Input.GetButtonDown ("Scope"))
				sprinting = false;
			float speed = maxSpeed;
			float yVel = Input.GetAxis ("Vertical") * speed;
			if (sprinting && sprintTimer > 0 && yVel > 0) {
				sprintTimer -= Time.deltaTime;
			}
			else if(sprintTimer < maxSprint) {
				sprinting = false;
				sprintTimer += Time.deltaTime;
			}
			if(yVel < minSpeed)
				yVel = minSpeed;
			else if(yVel > 0 && sprinting && sprintTimer > 0)
				yVel = maxSpeed * sprintMultiplier;
			direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * speed, 0, yVel);
			anim.SetFloat("Speed", yVel);
			if(gunAnim == null) {
				gunAnim = gun.GetComponent<Animator>();
			}
			gunAnim.SetFloat ("Speed", yVel);
			//normalize vector??
			if(cc.isGrounded && Input.GetButton("Jump")) {
				vertVelocity = jumpSpeed;
			}
		}
		else {
			transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);
		}
	}

	//Called once per physics loop, do movement here
	void FixedUpdate() {
		Vector3 distance = direction * Time.deltaTime;
		if(cc.isGrounded && vertVelocity < 0) {
			vertVelocity = Physics.gravity.y * Time.deltaTime;
		} else {
			vertVelocity += Physics.gravity.y * Time.deltaTime;
		}
		distance.y = vertVelocity * Time.deltaTime;
		cc.Move (distance);
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);
			stream.SendNext (gameObject.GetComponent<HealthBase>().currentHitPoints);
			stream.SendNext (anim.GetFloat("Speed"));
			stream.SendNext (anim.GetBool("Dead"));
		} else {
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			gameObject.GetComponent<HealthBase>().currentHitPoints = (float)stream.ReceiveNext();
			anim.SetFloat ("Speed", (float)stream.ReceiveNext());
			anim.SetBool ("Dead", (bool)stream.ReceiveNext());

			if(gotFirstUpdate == false) {
				transform.position = realPosition;
				transform.rotation = realRotation;
				gotFirstUpdate = true;
			}

		}

	}
}
