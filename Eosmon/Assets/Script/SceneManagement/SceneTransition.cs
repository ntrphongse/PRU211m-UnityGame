using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class SceneTransition : MonoBehaviourPunCallbacks
{
    [SerializeField] private int sceneIndex;
    private string sceneName;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var player = collision.gameObject;
            PhotonView view = player.GetComponent<PhotonView>();
            switch (sceneIndex)
            {
                case 1:
                    break;
                case 2:
                    SavedPositionManager.savedPositions = new Vector2(
                         collision.transform.position.x,
                         collision.transform.position.y - 1f
                         );
                    break;
                case 7:
                    SavedPositionManager.savedPositions = new Vector2(
                         collision.transform.position.x,
                         collision.transform.position.y - 1f
                         );
                    break;
                default:
                    SavedPositionManager.savedPositions = new Vector2(
                         collision.transform.position.x,
                         collision.transform.position.y - 1f
                         );
                    break;
            }
            this.sceneName = NameFromIndex(sceneIndex);
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene(sceneIndex);
            }
            else
            {
                if (PhotonNetwork.InRoom)
                {
                    CreateRoom(this.sceneName);
                }
            }
        }
    }

    private static string NameFromIndex(int BuildIndex)
    {
        string path = SceneUtility.GetScenePathByBuildIndex(BuildIndex);
        int slash = path.LastIndexOf('/');
        string name = path.Substring(slash + 1);
        int dot = name.LastIndexOf('.');
        return name.Substring(0, dot);
    }


    public void CreateRoom(string sceneName)
    {
        try
        {
            this.sceneName = sceneName;
            Debug.Log(this.sceneName);
            PhotonNetwork.LeaveRoom();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
        }
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log(this.sceneName);
        PhotonNetwork.JoinLobby();
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

