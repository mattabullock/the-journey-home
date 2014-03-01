using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUISkin styles;
	Vector2 viewPoint = Vector2.one;

	// Use this for initialization
	void Start () {
		PhotonNetwork.automaticallySyncScene = true;
		Connect ();
	}
	
	void Update() {
		//something about updating respawns should go here?!
	}
	
	void Connect() {
		PhotonNetwork.ConnectUsingSettings ("v006");
	}

	void OnGUI() {

		float ScreenX = Screen.width * 0.25f;
		float ScreenY = Screen.height * 0.2f;
		float areaWidth = Screen.width * 0.5f;
		float areaHeight = Screen.height * 0.5f;

		GUILayout.BeginArea (new Rect (ScreenX, ScreenY, areaWidth, areaHeight), styles.window);
		GUILayout.BeginHorizontal();
		GUILayout.Label("Server");
		GUILayout.Label("Players");
		GUILayout.EndHorizontal();
		viewPoint = GUILayout.BeginScrollView(viewPoint, false, true);
		foreach (RoomInfo room in PhotonNetwork.GetRoomList())
		{

			GUILayout.BeginHorizontal();
			GUILayout.Label(room.name);
			GUILayout.Label(room.playerCount + "/" + room.maxPlayers);
			if(GUILayout.Button("Connect")) {
				PhotonNetwork.JoinRoom(room.name);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView ();
		GUILayout.BeginHorizontal ();
		GUILayout.TextField ("");
		if (GUILayout.Button("Create Room")) {
			PhotonNetwork.CreateRoom("name",true,true,4);
		}
		GUILayout.EndHorizontal ();

		GUILayout.EndArea ();
		
	}

	void loadScene() {
		PhotonNetwork.LoadLevel ("testscene");
	}

	void OnJoinedRoom() {
		loadScene ();
	}
}
