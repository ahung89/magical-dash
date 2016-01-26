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
    }
}
