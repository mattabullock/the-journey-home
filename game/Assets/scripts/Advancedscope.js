#pragma strict
var defaultFOV : int = 0;
var fovSpeed : int = 0;
var fovIn : int = 0;

var gunAim : boolean = false;
var playerCam : GameObject;

var gunModel : GameObject;
var scopeInAnim : String;
var scopeOutAnim : String;

function Start () {
	defaultFOV = playerCam.camera.fieldOfView;
}

function Update () {
	if(Input.GetMouseButtonDown(1)){
		animation.Play(scopeInAnim);
		gunAim = true;
	}
	if(Input.GetMouseButtonUp(1)){
		animation.Play(scopeOutAnim);
		gunAim = false;
	}
	
	if(gunAim){
		playerCam.camera.fieldOfView = Mathf.Lerp(playerCam.camera.fieldOfView, fovIn, fovSpeed * Time.deltaTime);
	}
	
	if(!gunAim){
		playerCam.camera.fieldOfView = Mathf.Lerp(playerCam.camera.fieldOfView, defaultFOV, fovSpeed * Time.deltaTime);
	}
}