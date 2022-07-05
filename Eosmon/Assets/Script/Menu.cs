using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    IEnumerator WaitASecond(string type)
    {
        yield return new WaitForSeconds(1f);
        GameObject obj = GameObject.Find("BackgroundAudioMenu");
        switch (type)
        {
            case "play":
                Destroy(obj);
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case "multiplayerPlay":
                Destroy(obj);
                SceneManager.LoadScene("Loading");
                break;
            case "main_menu":
                SceneManager.LoadScene(0);
                break;
            case "room":
                SceneManager.LoadScene("Room");
                break;
            case "yard":
                SceneManager.LoadScene("ShoolYard");
                break;
            case "options":
                DontDestroyOnLoad(obj);
                SceneManager.LoadScene("Options");
                break;
            case "exit":
                Application.Quit();
                break;
        }
    }

    public void Play()
    {
        StartCoroutine(WaitASecond("play"));
    }

    public void MultiplayerPlay()
    {
        StartCoroutine(WaitASecond("multiplayerPlay"));
    }

    public void Options()
    {
        StartCoroutine(WaitASecond("options"));
    }

    public void BackToRoom()
    {
        StartCoroutine(WaitASecond("room"));
    }

    public void BackToYard()
    {
        StartCoroutine(WaitASecond("yard"));
    }

    public void BackToMainMenu()
    {
        StartCoroutine(WaitASecond("main_menu"));
    }

    public void Quit()
    {
        StartCoroutine(WaitASecond("exit"));
    }
}
