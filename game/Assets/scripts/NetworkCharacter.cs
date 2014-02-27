using UnityEngine;
using System.Collections;

public class NetworkCharacter : Photon.MonoBehaviour {

	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	float speed = 5.668f;
	float minSpeed = -2f;
	float vertVelocity = 0f;
	float jumpSpeed = 5f;
	Vector3 direction = Vector3.zero;
	CharacterController cc;
	public Animator anim;

	bool gotFirstUpdate = false;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
	}

	void OnGUI(){
		GUI.Box (new Rect(Screen.width/2,Screen.height/2, .5f, .5f),GUIContent.none);   	
	}

	// Update is called once per frame
	void Update () {
		if (photonView.isMine) {
			float yVel = Input.GetAxis ("Vertical") * speed;
			if(yVel < minSpeed)
				yVel = minSpeed;
			direction = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * speed, 0, yVel);
			anim.SetFloat("Speed", yVel);
//			if(direction.magnitude > 1f) {
//				direction = direction.normalized;
//			}
			if (cc.isGrounded) {
				vertVelocity = .000001f;
				if(Input.GetButtonDown("Jump")) {
					vertVelocity = jumpSpeed;
				}
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
		vertVelocity += Physics.gravity.y * Time.deltaTime;
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
