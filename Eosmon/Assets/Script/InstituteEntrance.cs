using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class InstituteEntrance : MonoBehaviourPunCallbacks
{
    public void CreateRoom()
    {
        Debug.Log("CreateRoom");
        try
        {
            PhotonNetwork.CreateRoom("ShoolYard");
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        JoinRoom();
    }

    public void JoinRoom()
    {
        PhotonNetwork.JoinRoom("ShoolYard");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("ShoolYard");
    }
}
