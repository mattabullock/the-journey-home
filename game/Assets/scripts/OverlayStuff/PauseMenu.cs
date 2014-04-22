using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour
{
//	GUISkin newSkin;

	public void thePauseMenu() {
		//layout start
		GUI.BeginGroup(new Rect(0, 0, Screen.width, Screen.height));
		//the menu background box
		GUI.Box(new Rect(0,0, Screen.width, Screen.height), "");
		///////pause menu buttons
		//game resume button
		if(GUI.Button(new Rect(Screen.width/2-(Screen.width/8), Screen.height/2-(2*Screen.height/16), Screen.width/4, Screen.height/16), "Resume")) {
			//resume the game
			Time.timeScale = 1.0f;
			Destroy(this);
			Screen.showCursor = false;
		}

		//main menu return button (level 0)
		if(GUI.Button(new Rect(Screen.width/2-(Screen.width/8), Screen.height/2-(Screen.height/16), Screen.width/4, Screen.height/16), "Main Menu")) {
			Time.timeScale = 1.0f;
			PhotonNetwork.LeaveRoom();
			Application.LoadLevel("MainMenu");	
		}
		//quit button
		if(GUI.Button(new Rect(Screen.width/2-(Screen.width/8), Screen.height/2, Screen.width/4, Screen.height/16), "Quit")) {
			Application.Quit();
		}
		//layout end
		GUI.EndGroup();
	}
	void OnGUI () {
		//load GUI skin
//		GUI.skin = newSkin;
		//show the mouse cursor
		Screen.showCursor = true;
		//run the pause menu script
		thePauseMenu();
		Debug.Log ("hoorah!");
	}
	
}