using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle }
public class GameController : MonoBehaviour
{
    [SerializeField] PlayerMovement playerController;
    [SerializeField] BattleSystem battleSystem;
    [SerializeField] Camera worldCam;
    [SerializeField] Camera battleCam;


    GameState state;

    private void Start()
    {
        playerController.OnEncounter += StartBattle;
        battleSystem.OnBattleOver += EndBattle;
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCam.gameObject.SetActive(true);
    }

    void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCam.gameObject.SetActive(false);

        battleSystem.StartBattle();
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else
        {
            battleSystem.HandleUpdate();
        }
    }
}
