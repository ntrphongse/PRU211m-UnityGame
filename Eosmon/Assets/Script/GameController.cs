using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState { FreeRoam, Battle, Dialog }
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
        DialogManager.Instance.OnShowDialog += () => { state = GameState.Dialog; };
        DialogManager.Instance.OnCloseDialog += () => { if (state == GameState.Dialog) state = GameState.FreeRoam; };
    }

    void EndBattle(bool won)
    {
        state = GameState.FreeRoam;
        battleSystem.gameObject.SetActive(false);
        worldCam.gameObject.SetActive(true);
        battleCam.gameObject.SetActive(false);

    }

    public void StartBattle()
    {
        state = GameState.Battle;
        battleSystem.gameObject.SetActive(true);
        worldCam.gameObject.SetActive(false);
        battleCam.gameObject.SetActive(true);

        battleSystem.StartBattle();
    }

    private void Update()
    {
        if (state == GameState.FreeRoam)
        {
            playerController.HandleUpdate();
        }
        else if (state == GameState.Dialog)
        {
            DialogManager.Instance.HandleUpdate();
        }
        else
        {
            battleSystem.HandleUpdate();
        }
    }
}
