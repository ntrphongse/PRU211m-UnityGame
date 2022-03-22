using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    IEnumerator WaitASecond(string type)
    {
        yield return new WaitForSeconds(1f);
        switch (type)
        {
            case "play":
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                break;
            case "main_menu":
                SceneManager.LoadScene(0);
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

    public void BackToMainMenu()
    {
        StartCoroutine(WaitASecond("main_menu"));
    }

    public void Quit()
    {
        StartCoroutine(WaitASecond("exit"));
    }
}
