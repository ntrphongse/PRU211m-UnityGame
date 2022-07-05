using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System;

public class InstituteEntrance : MonoBehaviourPunCallbacks
{
    public void CreateRoom()
    {
        try
        {
            PhotonNetwork.CreateRoom("FPT_HCM");
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
        PhotonNetwork.JoinRoom("FPT_HCM");
    }
    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("ShoolYard");
    }
}
