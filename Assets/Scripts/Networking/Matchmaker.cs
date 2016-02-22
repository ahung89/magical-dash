using UnityEngine;
using System.Collections;

public class Matchmaker : Photon.PunBehaviour {

	void Start () {
        PhotonNetwork.ConnectUsingSettings("0.1");	
	}

    public override void OnJoinedLobby ()
    {
        Debug.Log("Joining random room");
        PhotonNetwork.JoinRandomRoom();
    }

    public void OnPhotonRandomJoinFailed ()
    {
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnJoinedRoom ()
    {
        if (PhotonNetwork.room.playerCount > 1)
        {
            InitGame();
        }
    }

    // Called when a remote player joins the room.
    public override void OnPhotonPlayerConnected (PhotonPlayer newPlayer)
    {
        InitGame();
    }

    void InitGame()
    {
        if (!GameSettings.Instance.GameStarted)
        {
            GameSettings.Instance.StartGame();
        }
    }
}
