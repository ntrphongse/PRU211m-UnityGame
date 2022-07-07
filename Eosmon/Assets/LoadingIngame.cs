using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class LoadingIngame : MonoBehaviourPunCallbacks
{
    public string sceneName;
    private void Start()
    {
        if (PlayerPrefs.GetString("sceneName") != null)
        {
            this.sceneName = PlayerPrefs.GetString("sceneName");
            CreateRoom();
        }
    }
    public void CreateRoom()
    {
        try
        {
            PhotonNetwork.LeaveRoom();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public override void OnConnectedToMaster()
    {
        if (this.sceneName != null)
        {
            Debug.Log(this.sceneName);
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnJoinedLobby()
    {
        if (this.sceneName != null)
        {
            PhotonNetwork.CreateRoom(this.sceneName);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        JoinRoom();
    }

    public void JoinRoom()
    {
        if (this.sceneName != null)
        {
            PhotonNetwork.JoinRoom(this.sceneName);
        }
    }
    public override void OnJoinedRoom()
    {
        if (this.sceneName != null)
        {
            PhotonNetwork.LoadLevel(this.sceneName);
        }
    }
}
