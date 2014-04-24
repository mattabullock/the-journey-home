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

	public Texture2D progressBarEmpty;
	public Texture2D progressBarFull;


	void Start() {
		progressBarFull = new Texture2D (1, 1);
		progressBarEmpty = new Texture2D (1, 1);
		progressBarEmpty.SetPixel (0, 0, Color.red);
		progressBarFull.SetPixel (0, 0, Color.red);
		systems = GameObject.FindObjectsOfType<SystemBase> ();
	}
	
	void systemOverlay() {
		//layout start
		GUILayout.BeginArea (new Rect (0, 0, Screen.width, Screen.height));
		GUI.Box (new Rect (0, 0, Screen.width, Screen.height),"");
		GUILayout.BeginHorizontal();
		if (systems [0].foundBool) {

			GUILayout.BeginArea(new Rect(0,0,Screen.width/2,  Screen.height/4));
			GUILayout.BeginHorizontal ();
			GUILayout.Box (systems [0].image);
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[0].name);
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[0].currentHitPoints.ToString() + "%");
			GUILayout.EndHorizontal ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea();
		} else {
			GUILayout.BeginArea(new Rect(0,0,Screen.width/2,  Screen.height/4));
			GUILayout.EndArea ();		
		}

		if (systems [1].foundBool) {
			
			GUILayout.BeginArea(new Rect(Screen.width/2,0,Screen.width/2,  Screen.height/4));
			GUILayout.BeginHorizontal ();
			GUILayout.Box (systems [1].image);
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[1].name);
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[1].currentHitPoints.ToString() + "%");
			GUILayout.EndHorizontal ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea();
		} else {
			GUILayout.BeginArea(new Rect(Screen.width/2,0,Screen.width/2,  Screen.height/4));
			GUILayout.EndArea ();		
		}

		if (systems [2].foundBool) {
			
			GUILayout.BeginArea(new Rect(0,Screen.height/4,Screen.width/2,  Screen.height/4));
			GUILayout.BeginHorizontal ();
			GUILayout.Box (systems [2].image);
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[2].name);
			GUILayout.EndHorizontal ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[2].currentHitPoints.ToString() + "%");
			GUILayout.EndHorizontal ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea();
		} else {
			GUILayout.BeginArea(new Rect(0,Screen.height/4,Screen.width/2,  Screen.height/4));
			GUILayout.EndArea ();		
		}

		if (systems [3].foundBool) {
			
			GUILayout.BeginArea(new Rect(Screen.width/2,Screen.height/4,Screen.width/2,  Screen.height/4));
			GUILayout.BeginHorizontal ();
			GUILayout.Box (systems [3].image);
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[3].name);
			GUILayout.EndHorizontal ();

//			GUI.BeginGroup (new Rect(Screen.width - Screen.width/4,Screen.height/4,Screen.width/4,Screen.height/4));
//				GUI.DrawTexture (new Rect (0,0,Screen.width/4,Screen.height/4),progressBarEmpty);
//				GUI.BeginGroup (new Rect (0, 0, Screen.width/4 * systems [3].currentHitPoints/(float)100.0, Screen.height/4));
//			Debug.Log("we are here");
//					GUI.DrawTexture (new Rect (0,0,Screen.width/4,Screen.height/4),progressBarFull);
//				GUI.EndGroup();
//			GUI.EndGroup ();
			GUILayout.BeginHorizontal ();
			GUILayout.Label (systems[3].currentHitPoints.ToString() + "%");
			GUILayout.EndHorizontal ();
			GUILayout.EndHorizontal ();
			GUILayout.EndArea();
		} else {
			GUILayout.BeginArea(new Rect(Screen.width/2,Screen.height/4,Screen.width/2,  Screen.height/4));
			GUILayout.EndArea ();		
		}
//
//		if (systems [4].foundBool) {
//			
//			GUILayout.BeginArea(new Rect(0,Screen.height/2,Screen.width/2,  Screen.height/4));
//			GUILayout.BeginHorizontal ();
//			GUILayout.Box (systems [4].image);
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label (systems[4].name);
//			GUILayout.EndHorizontal ();
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Health");
//			GUILayout.EndHorizontal ();
//			GUILayout.EndHorizontal ();
//			GUILayout.EndArea();
//		} else {
//			GUILayout.BeginArea(new Rect(0,Screen.height/2,Screen.width/2,  Screen.height/4));
//			GUILayout.EndArea ();		
//		}
//
//		if (systems [5].foundBool) {
//			
//			GUILayout.BeginArea(new Rect(Screen.width/2,Screen.height/2,Screen.width/2,  Screen.height/4));
//			GUILayout.BeginHorizontal ();
//			GUILayout.Box (systems [5].image);
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Med Bay");
//			GUILayout.EndHorizontal ();
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Health");
//			GUILayout.EndHorizontal ();
//			GUILayout.EndHorizontal ();
//			GUILayout.EndArea();
//		} else {
//			GUILayout.BeginArea(new Rect(Screen.width/2,Screen.height/2,Screen.width/2,  Screen.height/4));
//			GUILayout.EndArea ();		
//		}
//
//		if (systems [6].foundBool) {
//			
//			GUILayout.BeginArea(new Rect(0,Screen.height - Screen.height/4,Screen.width/2,  Screen.height/4));
//			GUILayout.BeginHorizontal ();
//			GUILayout.Box (systems [6].image);
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Med Bay");
//			GUILayout.EndHorizontal ();
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Health");
//			GUILayout.EndHorizontal ();
//			GUILayout.EndHorizontal ();
//			GUILayout.EndArea();
//		} else {
//			GUILayout.BeginArea(new Rect(0,Screen.height - Screen.height/4,Screen.width/2,  Screen.height/4));
//			GUILayout.EndArea ();		
//		}
//
//		if (systems [7].foundBool) {
//			
//			GUILayout.BeginArea(new Rect(Screen.width/2,Screen.height - Screen.height/4,Screen.width/2,  Screen.height/4));
//			GUILayout.BeginHorizontal ();
//			GUILayout.Box (systems [7].image);
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Med Bay");
//			GUILayout.EndHorizontal ();
//			GUILayout.BeginHorizontal ();
//			GUILayout.Label ("Health");
//			GUILayout.EndHorizontal ();
//			GUILayout.EndHorizontal ();
//			GUILayout.EndArea();
//		} else {
//			GUILayout.BeginArea(new Rect(Screen.width/2,Screen.height - Screen.height/4,Screen.width/2,  Screen.height/4));
//			GUILayout.EndArea ();		
//		}

		GUILayout.EndHorizontal();
		GUILayout.EndArea ();
	}
	void OnGUI () {
		systemOverlay();
	}
	
}