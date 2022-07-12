using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using System;

public class SceneTransition : MonoBehaviourPunCallbacks
{
    [SerializeField] private int sceneIndex;
    public LoadingIngame loadingIngame;

    public void ConnectScene(string sceneName)
    {
        PlayerPrefs.SetString("sceneName", sceneName);
        if (!PhotonNetwork.IsConnected)
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            if (PhotonNetwork.InRoom)
            {
                PhotonNetwork.LoadLevel("LoadingIngame");
            }
        }
    }
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
            this.ConnectScene(NameFromIndex(sceneIndex));
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
}

