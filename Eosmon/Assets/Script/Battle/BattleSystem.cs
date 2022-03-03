using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum BattleState { Start, PlayerAction, PLayerMove, EnemyMove, Busy }

public class BattleSystem : MonoBehaviour
{
    [SerializeField] BattleUnit player;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemy;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;

    BattleState state;
    int currentAction;
    int currentMove;
    private void Start()
    {
        StartCoroutine(SetupBattle());
    }

    public IEnumerator SetupBattle()
    {
        player.Setup();
        enemy.Setup();
        playerHud.SetData(player.Lecturer);
        enemyHud.SetData(enemy.Lecturer);
        var answers = new List<Answer>();

        answers.Add(new Answer(new AnswerBase("1", "", mobType.Regular, 5)));
        answers.Add(new Answer(new AnswerBase("2", "", mobType.Regular, 5)));
        answers.Add(new Answer(new AnswerBase("3", "", mobType.Regular, 5)));
        answers.Add(new Answer(new AnswerBase("4", "", mobType.Regular, 5)));

        dialogBox.setMoveNames(answers);

        yield return dialogBox.TypeDialog($"The lecturer {enemy.Lecturer.Base.Name} appeared.");
        yield return new WaitForSeconds(1f);

        PlayerAction();
    }

    IEnumerator PerformPlayerMove()
    {
        state = BattleState.Busy;
        Answer answer = new Answer(new AnswerBase("Test", "Test answer", mobType.Regular, 10));
        yield return dialogBox.TypeDialog($"Answer is...{answer.Base.Description}");

        yield return new WaitForSeconds(1f);

        bool isFainted = enemy.Lecturer.TakeDamage(answer, player.Lecturer);
        yield return enemyHud.UpdateKP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog("You are worthy");
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove;
        Answer question = enemy.Lecturer.GetRandomQuestion();
        playerHud.UpdateKP();
        yield return dialogBox.TypeDialog($"Next question is...{question.Base.Description}");
        yield return new WaitForSeconds(1f);
        bool isFainted = player.Lecturer.TakeDamage(question, player.Lecturer);
        yield return playerHud.UpdateKP();

        if (isFainted)
        {
            yield return dialogBox.TypeDialog("You are not worthy");
        }
        else
        {
            PlayerAction();
        }
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

    private void Update()
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
            StartCoroutine(PerformPlayerMove());
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
