using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public enum BattleState { Start, PlayerAction, PLayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    public TextAsset jsonFile;

    [SerializeField] BattleUnit player;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemy;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] QuestionBase q;
    [SerializeField] GameController gameController;

    public event Action<bool> OnBattleOver;
    public event Action<bool> OnWiningGame;


    BattleState state;
    int currentAction;
    int currentMove;
    public void StartBattle()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        OnWiningGame(false);

        player.Setup();
        enemy.Setup();
        playerHud.SetData(player.Lecturer);
        enemyHud.SetData(enemy.Lecturer);

        yield return dialogBox.TypeDialog($"The lecturer {enemy.Lecturer.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);
        q = enemy.Lecturer.GetRandomQuestion(jsonFile);
        yield return dialogBox.TypeDialog($"First question is...{q.Question}");
        yield return new WaitForSeconds(3f);
        dialogBox.setMoveNames(q.SuggestedAnswers);

        PlayerAction();
    }

    IEnumerator PerformPlayerMove(int currentMove)
    {
        bool correct = false;
        switch (q.CorrectAnswer)
        {
            case "A": if (currentMove + 1 == 1) correct = true; break;
            case "B": if (currentMove + 1 == 2) correct = true; break;
            case "C": if (currentMove + 1 == 3) correct = true; break;
            case "D": if (currentMove + 1 == 4) correct = true; break;
        }
        state = BattleState.Busy;
        yield return dialogBox.TypeDialog($"Answer is...{q.CorrectAnswer}");
        yield return new WaitForSeconds(1f);

        bool isLecturerFainted = false;
        bool isPlayerFainted = false;
        if (correct)
        {
            player.PlayAttacAnimation();
            enemy.PlayHitAnimation();
            isLecturerFainted = enemy.Lecturer.TakeDamage(q, player.Lecturer);
            yield return enemyHud.UpdateKP();

        }
        else
        {
            enemy.PlayAttacAnimation();
            player.PlayHitAnimation();
            isPlayerFainted = player.Lecturer.TakeDamage(q, player.Lecturer);
            yield return playerHud.UpdateKP();

        }
        if (isLecturerFainted)
        {
            yield return dialogBox.TypeDialog("You are worthy");
            enemy.PLayFaintAnimation();
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
        }
        else if (isPlayerFainted)
        {
            yield return dialogBox.TypeDialog("You are not worthy");
            player.PLayFaintAnimation();
            yield return new WaitForSeconds(2f);
            OnBattleOver(false);
            OnWiningGame(false);
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        q = enemy.Lecturer.GetRandomQuestion(jsonFile);
        yield return dialogBox.TypeDialog($"Next question is...{q.Question}");
        yield return new WaitForSeconds(3f);
        dialogBox.setMoveNames(q.SuggestedAnswers);

        PlayerAction();
    }

    void PlayerAction()
    {
        state = BattleState.PlayerAction;
        StartCoroutine(dialogBox.TypeDialog("Choose an action: "));
        dialogBox.EnabledActionSelector(true);
    }

    void PlayerMove()
    {
        state = BattleState.PLayerMove;

        dialogBox.EnabledActionSelector(false);
        dialogBox.EnableDialogText(false);
        dialogBox.EnabledMoveSelector(true);
    }

    public void HandleUpdate()
    {
        if (state == BattleState.PlayerAction)
        {
            HandleActionSelection();
        }
        else if (state == BattleState.PLayerMove)
        {
            HandleMoveSelection();
        }
    }

    void HandleMoveSelection()
    {
        var movesCount = 4;
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (currentMove < movesCount - 1)
            {
                ++currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (currentMove > 0)
            {
                --currentMove;
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentMove < movesCount - 1)
            {
                currentMove += 2;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentMove > 1)
            {
                currentMove -= 2;
            }
        }
        dialogBox.UpdateMoveSelection(currentMove);

        if (Input.GetKeyDown(KeyCode.Z))
        {
            dialogBox.EnabledMoveSelector(false);
            dialogBox.EnableDialogText(true);
            StartCoroutine(PerformPlayerMove(currentMove));
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            dialogBox.SetDialog(q.Question);
            dialogBox.EnabledMoveSelector(false);
            dialogBox.EnableDialogText(true);
        }
        if (Input.GetKeyUp(KeyCode.X))
        {
            dialogBox.EnableDialogText(false);
            dialogBox.EnabledMoveSelector(true);
        }
    }


    void HandleActionSelection()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (currentAction < 1)
            {
                ++currentAction;
            }
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (currentAction > 0)
            {
                --currentAction;
            }
        }
        dialogBox.UpdateActionSelection(currentAction);
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (currentAction == 0)
            {
                PlayerMove();
            }
            else if (currentAction == 1)
            {
                //Run
            }

        }
    }
}
