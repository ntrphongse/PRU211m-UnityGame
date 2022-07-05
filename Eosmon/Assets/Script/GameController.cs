using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState { FreeRoam, Battle, Dialog, Died, Finished, Ended }
public class GameController : MonoBehaviour
{
    [SerializeField] SpawnPlayers spawnPlayers;
    [SerializeField] PlayerMovement playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCam;
    [SerializeField] Follow follow;
    [SerializeField] Camera battleCam;
    [SerializeField] AudioSource music;
    [SerializeField] AudioSource roomMusic;
    [SerializeField] Text scoreText;
    GameState state;
    private void Start()
    {
        if (scoreText != null)
            scoreText.text = "Your Score: " + GlobalVariables.totalScore;
        Scene currentScene = SceneManager.GetActiveScene();
        if (spawnPlayers != null)
        {
            var player = spawnPlayers.Spawn();
            playerController = player.GetComponent<PlayerMovement>();
            follow.player = player.transform;
        }
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
            GlobalVariables.totalScore += 5000;
            if (scoreText != null)
                scoreText.text = "Your Score: " + GlobalVariables.totalScore;
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
            GlobalVariables.totalScore += 50;
            music.Stop();
            state = GameState.FreeRoam;
            battleSystem.gameObject.SetActive(false);
            worldCam.gameObject.SetActive(true);
            battleCam.gameObject.SetActive(false);
            if (scoreText != null)
                scoreText.text = "Your Score: " + GlobalVariables.totalScore;
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
