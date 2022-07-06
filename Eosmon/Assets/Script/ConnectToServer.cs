using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class ConnectToServer : MonoBehaviourPunCallbacks
{
    public bool isFirstTimeJoin;
    private void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            isFirstTimeJoin = true;
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("oowowow");
        Debug.Log(isFirstTimeJoin);
        if (isFirstTimeJoin)
        {
            isFirstTimeJoin = false;
            PhotonNetwork.LoadLevel("Lobby");
        }
    }

}
