using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPlayers : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject Spawn()
    {
        var x = -0.5f;
        var y = -0.25f;
        var z = 0f;
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name.Contains("Room"))
        {
            x = -1f;
            y = -3f;
        }
        if (PhotonNetwork.IsConnected)
        {
            var player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity);
            player.transform.position = new Vector3(x, y, z);
            return player;
        }
        else
        {
            var player = Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            player.transform.position = new Vector3(x, y, z);
            return player;
        }
    }
}
