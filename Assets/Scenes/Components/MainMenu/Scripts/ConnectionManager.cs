using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : Photon.PunBehaviour//, IPunCallbacks
{
    string _gameVersion = "1";
	bool isConnecting;

	private void Awake()
    {
        PhotonNetwork.autoJoinLobby = false;
        PhotonNetwork.automaticallySyncScene = true;
    }

	private void Start()
	{
		MainMenuManager.Instance.buttonConnect.onClick.AddListener(Connect);
	}

	private void Connect()
	{
		isConnecting = true;

		if (PhotonNetwork.connected)
		{
			PhotonNetwork.JoinRandomRoom();
		}
		else
		{
			PhotonNetwork.ConnectUsingSettings(_gameVersion);
		}
	}
	/// <summary>
	/// When someone joined the room
	/// </summary>
	/// <param name="newPlayer"></param>
	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		base.OnPhotonPlayerConnected(newPlayer);
	}

	/// <summary>
	/// When someone left the room
	/// </summary>
	/// <param name="otherPlayer"></param>
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		base.OnPhotonPlayerDisconnected(otherPlayer);
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);
		MainMenuManager.Instance.textLog.text = "Joining random room ";
		if (isConnecting)
		{
			PhotonNetwork.JoinRandomRoom();
		}
	}


	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		MainMenuManager.Instance.textLog.text = "No avail. rooms, creating ";
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, null);
		
	}

	public override void OnJoinedRoom()
	{
		Debug.Log("DemoAnimator/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.\nFrom here on, your game would be running. For reference, all callbacks are listed in enum: PhotonNetworkingMessage");

		MainMenuManager.Instance.textLog.text = "Joined room ";
		//if (PhotonNetwork.room.PlayerCount == 1)
		//{
			PhotonNetwork.LoadLevel("MultiplayerGame");
		//}
	}

	public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("Error join room  " + codeAndMsg);
	}

	
}
