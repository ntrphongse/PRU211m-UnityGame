
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameController gameController;

    public void Interact()
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;
        
        if (sceneName == "Room")
        {
            gameController.StartBattle();
        }
        else
        {
            Debug.Log("NPC interact!");
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }

    }
}
