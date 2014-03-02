using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public GUISkin styles;
	Vector2 viewPoint = Vector2.one;
	enum t_screen {
		main,
		servers,
		lobby
	};
	t_screen screen = t_screen.main;
	string roomName = "";

	// Use this for initialization
	void Start () {
		PhotonNetwork.automaticallySyncScene = true;
		Connect ();
	}
	
	void Connect() {
		PhotonNetwork.ConnectUsingSettings ("v007");
	}

	void OnGUI() {
		if (screen == t_screen.main) {
			MainScreen ();
		} else if (screen == t_screen.servers) {
			ServerBrowser ();
		} else if (screen == t_screen.lobby) {

		}
	}

	void MainScreen() {

		float ScreenX = Screen.width * 0.25f;
		float ScreenY = Screen.height * 0.2f;
		float areaWidth = Screen.width * 0.5f;
		float areaHeight = Screen.height * 0.5f;
		
		GUILayout.BeginArea (new Rect (ScreenX, ScreenY, areaWidth, areaHeight), styles.window);
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		GUILayout.Label ("THE JOURNEY HOME");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		if (GUILayout.Button ("Play")) {
			screen = t_screen.servers;
		}
		if (GUILayout.Button ("Quit")) {
			Application.Quit();
		}
		GUILayout.EndArea ();
	}

	void ServerBrowser() {
		float ScreenX = Screen.width * 0.25f;
		float ScreenY = Screen.height * 0.2f;
		float areaWidth = Screen.width * 0.5f;
		float areaHeight = Screen.height * 0.5f;
		
		GUILayout.BeginArea (new Rect (ScreenX, ScreenY, areaWidth, areaHeight), styles.window);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button ("Back"))
			screen = t_screen.main;
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		GUILayout.Label("Server", GUILayout.MinWidth(areaWidth/3),  GUILayout.MaxWidth(areaWidth/3));
		GUILayout.Label("Players", GUILayout.MinWidth(areaWidth/6),  GUILayout.MaxWidth(areaWidth/6));
		GUILayout.EndHorizontal();
		viewPoint = GUILayout.BeginScrollView(viewPoint, false, true);
		foreach (RoomInfo room in PhotonNetwork.GetRoomList())
		{
			
			GUILayout.BeginHorizontal();
			GUILayout.Label(room.name, GUILayout.MinWidth(areaWidth/3),  GUILayout.MaxWidth(areaWidth/3));
			GUILayout.Label(room.playerCount + "/" + room.maxPlayers, GUILayout.MinWidth(areaWidth/6),  GUILayout.MaxWidth(areaWidth/3));
			if(GUILayout.Button("Connect", GUILayout.MinWidth(areaWidth/4),  GUILayout.MaxWidth(areaWidth/4))) {
				PhotonNetwork.JoinRoom(room.name);
			}
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView ();
		GUILayout.BeginHorizontal ();
		roomName = GUILayout.TextField (roomName, 20, GUILayout.MinWidth(350),  GUILayout.MaxWidth(350));
		if (GUILayout.Button("Create Room")) {
			PhotonNetwork.CreateRoom(roomName,true,true,4);
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
