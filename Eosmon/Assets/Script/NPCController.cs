
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
    [SerializeField] GameController gameController;


    public void Interact(Collider2D collider)
    {
        // Create a temporary reference to the current scene.
        Scene currentScene = SceneManager.GetActiveScene();

        // Retrieve the name of this scene.
        string sceneName = currentScene.name;

        if (sceneName == "Room" || sceneName == "BossRoom")
        {
            GameObject npcBattle = GameObject.Find("NPC");
            if (collider.name == "NPC" || collider.name == "BossNPC")
                gameController.StartBattle(collider.name);
            else
                StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }
        else
        {
            Debug.Log("NPC interact!");
            if (SceneManager.GetActiveScene().name == "InfoRoom")
            {
                dialog.Lines.Add("Welcome!");
                dialog.Lines.Add("Don't worry! I won't be testing you!");
                dialog.Lines.Add("You must be careful with the others though.");
                dialog.Lines.Add("It's finals season, so everyone can be a bit... weird");
                dialog.Lines.Add("Especially those in the LaSt Ro0oM o0n tHe LeFt");
                dialog.Lines.Add("That's it! Have fun!");
            }
            else if (SceneManager.GetActiveScene().name == "ShoolYard")
            {
                //dialog.Lines.Add("Greetings!");
                //dialog.Lines.Add("Welcome to the Fire Phoenix Training-ground");
                //dialog.Lines.Add("Wander around as you may please.");
            }
            StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
        }

    }

}
