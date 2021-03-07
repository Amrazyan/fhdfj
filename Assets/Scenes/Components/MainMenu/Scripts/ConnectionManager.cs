using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionManager : Photon.PunBehaviour//, IPunCallbacks
{
    string _gameVersion = "1";
	bool isConnecting;

	public static ConnectionManager instance = null;

	public event Action OnAnotherPlayerConnected;
	public event Action OnAnotherPlayerLeft;

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			PhotonNetwork.autoJoinLobby = false;
			PhotonNetwork.automaticallySyncScene = true;
			OnAnotherPlayerConnected = OnStart;

			DontDestroyOnLoad(gameObject);
			return;
		}
		if (instance == this) return;
		Destroy(gameObject);
		
	}

	private void OnStart()
	{
		Debug.Log("Loading scene");
		PhotonNetwork.LoadLevel("MultiplayerGame");
		
	}

	public void Connect()
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
		if (MainMenuManager.Instance)
		{
			MainMenuManager.Instance.textLog.text = "Waiting for a game";
		}
	}
	/// <summary>
	/// When someone joined the room
	/// </summary>
	/// <param name="newPlayer"></param>
	public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
	{
		base.OnPhotonPlayerConnected(newPlayer);
		Debug.Log("Player connected + " + newPlayer.NickName);
		OnAnotherPlayerConnected?.Invoke();
	}

	/// <summary>
	/// When someone left the room
	/// </summary>
	/// <param name="otherPlayer"></param>
	public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
	{
		base.OnPhotonPlayerDisconnected(otherPlayer);
		Debug.Log("Player left + " + otherPlayer.NickName);
		OnAnotherPlayerLeft?.Invoke();
	}

	public override void OnConnectedToMaster()
	{
		Debug.Log("Region:" + PhotonNetwork.networkingPeer.CloudRegion);
		//MainMenuManager.Instance?.textLog?.text = "Joining random room ";
		if (isConnecting)
		{
			PhotonNetwork.JoinRandomRoom();
		}
	}


	public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
	{
		//MainMenuManager.Instance.textLog.text = "No avail. rooms, creating ";
		PhotonNetwork.CreateRoom(null, new RoomOptions() { MaxPlayers = 2 }, null);
		
	}

	public override void OnJoinedRoom()
	{
		Debug.Log(PhotonNetwork.room.PlayerCount);

		//MainMenuManager.Instance.textLog.text = "Joined room \n waiting another player";
		if (PhotonNetwork.room.PlayerCount > 1)
		{
			PhotonNetwork.LoadLevel("MultiplayerGame");
		}
	}

	public override void OnPhotonJoinRoomFailed(object[] codeAndMsg)
	{
		Debug.Log("Error join room  " + codeAndMsg);
	}

	
}
