using UnityEngine;
using System.Collections;

public class SystemOverlay : MonoBehaviour
{
	SystemBase[] systems;
	public Texture2D cross;
	public Texture2D lights;
	public Texture2D oxygen;
	public Texture2D engine;
	public Texture2D armory;
	public Texture2D engineering;
	public Texture2D cameraTexture;


	void Start() {
		systems = GameObject.FindObjectsOfType<SystemBase> ();
	}
	
	void systemOverlay() {
		//layout start
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		GUI.Box (new Rect (0, 0, Screen.width, Screen.height),"");
		GUILayout.BeginHorizontal();
		if (systems [1].foundBool) {

			GUILayout.BeginArea(new Rect(0,0,Screen.width/2,  Screen.height/4));
			GUILayout.BeginHorizontal ();
			GUILayout.Box (cross);
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Med Bay");
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label ("Health");
			GUILayout.EndHorizontal ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea();
		} else {
			GUILayout.BeginArea(new Rect(0,0,Screen.width/2,  Screen.height/4));
			GUILayout.EndArea ();		
		}
		GUILayout.EndHorizontal();
		GUILayout.EndArea ();
	}
	void OnGUI () {
		systemOverlay();
	}
	
}