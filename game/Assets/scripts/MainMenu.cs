using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

public class MainMenu : Photon.MonoBehaviour {

	public GUISkin styles;
	Vector2 viewPoint = Vector2.one;
	enum t_screen {
		set_name,
		main,
		servers,
		lobby
	};
	t_screen screen = t_screen.main;
	string roomName = "";
	string playerName = "";
	bool isReady = false;
	bool lobbyReady = false;

	// Use this for initialization
	void Start () {
		PhotonNetwork.automaticallySyncScene = true;
		Connect ();
		if ((PhotonNetwork.player.name = playerName = PlayerPrefs.GetString ("Username", "")) == "") {
			screen = t_screen.set_name;
		}
	}
	
	void Connect() {
		PhotonNetwork.ConnectUsingSettings ("v009");
		PhotonNetwork.player.name = PlayerPrefs.GetString ("Username", "");
	}

	void OnGUI() {
		if (screen == t_screen.set_name) {
			SetNameScreen ();
		} else if (screen == t_screen.main) {
			MainScreen ();
		} else if (screen == t_screen.servers) {
			ServerBrowser ();
		} else if (screen == t_screen.lobby) {
			Lobby();
		}
	}

	void SetNameScreen() {
		float ScreenX = Screen.width * 0.25f;
		float ScreenY = Screen.height * 0.2f;
		float areaWidth = Screen.width * 0.5f;
		float areaHeight = Screen.height * 0.5f;
		
		GUILayout.BeginArea (new Rect (ScreenX, ScreenY, areaWidth, areaHeight), styles.window);
		GUILayout.Space (areaHeight/4);
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		GUILayout.Label ("SET YOUR NAME");
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.Space (10);
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		playerName = GUILayout.TextField (playerName, 20, GUILayout.MinWidth(areaWidth/2),  GUILayout.MaxWidth(areaWidth/2));
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.Space (10);
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		playerName = Regex.Replace (playerName, "^ +$", "");
		if (GUILayout.Button ("OK", GUILayout.MinWidth(areaWidth/3), GUILayout.MaxWidth(areaWidth/3)) && playerName != "") {
			PhotonNetwork.player.name = playerName;
			PlayerPrefs.SetString ("Username", playerName);
			screen = t_screen.main;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
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
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button ("Play")) {
			screen = t_screen.servers;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button ("Edit Name")) {
			screen = t_screen.set_name;
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal();
		GUILayout.FlexibleSpace();
		if (GUILayout.Button ("Quit")) {
			Application.Quit();
		}
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
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
		roomName = GUILayout.TextField (roomName, 20, GUILayout.MinWidth(areaWidth/3),  GUILayout.MaxWidth(areaWidth/3));
		roomName = Regex.Replace (roomName, "^ +$", "");
		if (GUILayout.Button("Create Room") && roomName != "") {
			PhotonNetwork.CreateRoom(roomName,true,true,4);
			roomName = "";
		}
		GUILayout.EndHorizontal ();
		
		GUILayout.EndArea ();
	}

	void Lobby() {
		float ScreenX = Screen.width * 0.25f;
		float ScreenY = Screen.height * 0.2f;
		float areaWidth = Screen.width * 0.5f;
		float areaHeight = Screen.height * 0.5f;

		lobbyReady = true;

		GUILayout.BeginArea (new Rect (ScreenX, ScreenY, areaWidth, areaHeight), styles.window);
		GUILayout.BeginHorizontal();
		if (GUILayout.Button ("Leave", GUILayout.MaxWidth(areaWidth/6))) {
			PhotonNetwork.LeaveRoom();
			screen = t_screen.servers;
		}
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace();
		GUILayout.Label ("Room: " + PhotonNetwork.room.name);
		GUILayout.Label ("Players: " + PhotonNetwork.room.playerCount + "/" + PhotonNetwork.room.maxPlayers);
		GUILayout.FlexibleSpace();
		GUILayout.EndHorizontal ();
		GUILayout.BeginHorizontal ();
		GUILayout.FlexibleSpace ();
		GUILayout.Label ("PLAYERS");
		GUILayout.FlexibleSpace ();
		GUILayout.EndHorizontal ();
		foreach (PhotonPlayer p in PhotonNetwork.playerList) {
			GUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace();
			if(p == PhotonNetwork.player) {
				isReady = GUILayout.Toggle (isReady, " Ready?", GUILayout.MinWidth(areaWidth/4), GUILayout.MaxWidth(areaWidth/4));
				PhotonNetwork.player.SetCustomProperties (new ExitGames.Client.Photon.Hashtable () {{"ready", isReady}});
			}
			if(!(bool)p.customProperties["ready"]) {
				lobbyReady = false;
			}
			string ready = "";
			if((bool)p.customProperties["ready"])
				ready = "Ready";
			else
				ready = "Not Ready";
			GUILayout.Label(ready, GUILayout.MinWidth(areaWidth/4), GUILayout.MaxWidth(areaWidth/4));
			GUILayout.Label (p.name, GUILayout.MinWidth(areaWidth/4), GUILayout.MaxWidth(areaWidth/4));
			GUILayout.EndHorizontal ();
		}
		GUILayout.BeginHorizontal ();
		if (PhotonNetwork.isMasterClient) {
			if(lobbyReady) {
				GUILayout.FlexibleSpace();
				if(GUILayout.Button("Start Game", GUILayout.MaxWidth(areaWidth/3))) {
					GetComponent<PhotonView>().RPC ("loadScene", PhotonTargets.AllBuffered);
				}
				GUILayout.FlexibleSpace();
			} else {
				GUILayout.FlexibleSpace();
				GUI.enabled = false;
				if(GUILayout.Button("Start Game", GUILayout.MaxWidth(areaWidth/3))) {
					GetComponent<PhotonView>().RPC ("loadScene", PhotonTargets.All);
				}
				GUI.enabled = true;
				GUILayout.FlexibleSpace();
			}
		}
		GUILayout.EndHorizontal ();
		GUILayout.EndArea ();
	}

	void OnJoinedRoom() {
		screen = t_screen.lobby;
	}

	[RPC]
	void loadScene() {
		PhotonNetwork.LoadLevel ("layout");
	}
		
	void StartGame() {
		PhotonNetwork.room.open = false;
		loadScene ();
	}
}
