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

	bool gotFirstUpdate = false;

	public GameObject currentCell;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		gunAnim = gun.GetComponent<Animator> ();
	}

	void OnTriggerStay(Collider c){
		if(c.tag == "AIPathCell"){
			currentCell = c.gameObject;
		}
	}

//	void OnGUI(){
//		GUI.Box (new Rect(Screen.width/2,Screen.height/2, .5f, .5f),GUIContent.none);   	
//	}

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
			stream.SendNext (gameObject.GetComponent<Health>().currentHitPoints);
			stream.SendNext (anim.GetFloat("Speed"));
			stream.SendNext (anim.GetBool("Dead"));
		} else {
			realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext();
			gameObject.GetComponent<Health>().currentHitPoints = (float)stream.ReceiveNext();
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
