using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Died, Finished, Ended }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCam;
    [SerializeField] Camera battleCam;
    [SerializeField] AudioSource music;

    GameState state;

    private void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "InfoRoom")
        {
            if (music != null)
            {
                music.Play();
            }
        }
        else
        {
            if (music != null)
            {
                music.Stop();
            }
        }
        battleSystem.OnWiningGame += EndGame;
        battleSystem.OnBattleOver += EndBattle;
        if (DialogManager.Instance != null)
        {
            DialogManager.Instance.OnShowDialog += () => { state = GameState.Dialog; };
            DialogManager.Instance.OnCloseDialog += () => { if (state == GameState.Dialog) state = GameState.FreeRoam; };
        }
    }

    void EndGame(bool won)
    {
        if (won)
        {
            SceneManager.LoadScene("Victory", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
        }
    }

    void EndBattle(bool won)
    {
        if (won)
        {
            music.Stop();
            state = GameState.FreeRoam;
            battleSystem.gameObject.SetActive(false);
            worldCam.gameObject.SetActive(true);
            battleCam.gameObject.SetActive(false);
        }
    }

    public void ExitGame()
    {
        state = GameState.Ended;
        SceneManager.LoadScene("Menu");
    }

    public void RestartGame()
    {
        state = GameState.Ended;
        SceneManager.LoadScene("ShoolYard");
    }

    public void StartBattle(string npcName)
    {
        if (npcName != "BossNPC")
        {
            music.Play();
        }
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCam.gameObject.SetActive(false);
        battleCam.gameObject.SetActive(true);
        battleSystem.StartBattle(npcName);
    }

    public void MuteMusic()
    {
        if (music.mute)
        {
            music.mute = false;
        }
        else
        {
            music.mute = true;
        }
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            worldCam.gameObject.SetActive(true);
            battleCam.gameObject.SetActive(false);
            playerController.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else if (state == GameState.Died)
        {
        }
        else if (state == GameState.Ended)
        {
            battleSystem.gameObject.SetActive(false);
            worldCam.gameObject.SetActive(true);
            battleCam.gameObject.SetActive(false);
        }
        else
        {
            battleSystem.HandleUpdate();
        }
    }
}
