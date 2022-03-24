using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;

public enum BattleState { Start, PlayerAction, PLayerMove, EnemyMove, Busy, BossFight }

public class BattleSystem : MonoBehaviour
{
    public static readonly string CorrectAnswersDuringBoss = "CorrectAnswersDuringBoss";
    public TextAsset learning;
    public TextAsset questionEasy;

    [SerializeField] BattleUnit player;
    [SerializeField] BattleUnit hero;
    [SerializeField] BattleHud playerHud;
    [SerializeField] BattleUnit enemy;
    [SerializeField] BattleUnit boss;
    [SerializeField] BattleHud enemyHud;
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] QuestionBase q;
    [SerializeField] TImer timer;
    [SerializeField] AudioSource bossMusic;
    [SerializeField] AudioSource comeBackMusic;
    [SerializeField] Image mainMenu;
    [SerializeField] Text zText;

    [SerializeField] CanvasGroup _bossCanvasGroup;
    [SerializeField] Camera battleCam;




    private bool IsBossFight;
    private bool IsComeback;
    private int noOfFaints;



    public event Action<bool> OnBattleOver;
    public event Action<bool> OnWiningGame;


    BattleState state;
    int currentAction;
    int currentMove;
    public void StartBattle(string npcName)
    {
        if (mainMenu != null)
        {
            mainMenu.enabled = false;
        }
        if (zText != null)
        {
            zText.enabled = false;
        }
        noOfFaints = 0;
        PlayerPrefs.SetInt(CorrectAnswersDuringBoss, 0);
        StartCoroutine(SetupBattle(npcName));
    }

    public IEnumerator SetupBattle(string npcName)
    {
        if (GameObject.Find("RoomBGMUSIC") != null)
        {
            GameObject obj = GameObject.Find("RoomBGMUSIC");
            obj.SetActive(false);
        }
        GameObject.Find("score").SetActive(false);
        GameObject.Find("Press [ Z ]").SetActive(false);
        GameObject.Find("ImgToMainMenuInRoom").SetActive(false);


        IsBossFight = false;
        IsComeback = false;
        player.Setup("Student");
        enemy.Setup(npcName);
        playerHud.SetData(player.Lecturer);
        enemyHud.SetData(enemy.Lecturer);

        yield return dialogBox.TypeDialog($"The lecturer {npcName} appeared.");
        yield return new WaitForSeconds(1f);
        if (GlobalVariables.difficulty == 0)
        {
            q = enemy.Lecturer.GetRandomQuestion(questionEasy);
        }
        else if (GlobalVariables.difficulty == 1)
        {
            q = enemy.Lecturer.GetRandomQuestion(learning);
        }

        if (npcName == "BossNPC")
        {
            yield return dialogBox.TypeDialog($"You shouldn't have come here.");
            yield return new WaitForSeconds(2f);
            yield return dialogBox.TypeDialog($"Did you not hear what the others were saying?");
            yield return new WaitForSeconds(2f);
            yield return dialogBox.TypeDialog($"...........................");
            yield return new WaitForSeconds(2f);
            yield return dialogBox.TypeDialog($"Ughhhhh..... Fine.");
            yield return new WaitForSeconds(2f);
        }
        yield return dialogBox.TypeDialog($"First question is...\n{q.Question}");
        yield return new WaitForSeconds(2f);
        if (npcName == "BossNPC")
        {
            bossMusic.Play();
            enemy.PlayHitAnimation();
            yield return dialogBox.TypeDialog("Aaaaaaaaaaaagh....");
            enemy.Lecturer.TakeDamage(q, player.Lecturer, 100000);
            yield return enemyHud.UpdateKP();
            yield return dialogBox.TypeDialog(".................??? \n No, this cannot be!?!?");
            yield return new WaitForSeconds(2f);
            yield return dialogBox.TypeDialog("You fool! You have doomed us all!!!");
            yield return new WaitForSeconds(2f);
            enemy.PLayFaintAnimation();

            IsBossFight = true;
            boss.Setup("01101100011010000110101101110000");
            enemyHud.SetData(boss.Lecturer);
            yield return dialogBox.TypeDialog($"The lecturer 01101100011010000110101101110000 appeared.");
            yield return new WaitForSeconds(15f); 
            if (GlobalVariables.difficulty == 0)
            {
                q = enemy.Lecturer.GetRandomQuestion(questionEasy);
            }
            else if (GlobalVariables.difficulty == 1)
            {
                q = enemy.Lecturer.GetRandomQuestion(learning);
            }
            yield return dialogBox.TypeDialog($"First question is...\n{q.Question}");
            yield return new WaitForSeconds(2f);
        }

        dialogBox.setMoveNames(q.SuggestedAnswers);
        PlayerAction();
    }

    IEnumerator PerformPlayerMove(int currentMove)
    {

        bool correct = false;
        timer.StopTimer();
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
            if (IsComeback)
            {
                hero.PlayAttacAnimation();
            }
            else
            {
                player.PlayAttacAnimation();
            }
            if (IsBossFight)
            {
                boss.PlayHitAnimation();
                if (IsComeback)
                {
                    isLecturerFainted = boss.Lecturer.TakeDamage(q, player.Lecturer, 10);
                }
                else
                {
                    isLecturerFainted = boss.Lecturer.TakeDamage(q, player.Lecturer, 1);
                    PlayerPrefs.SetInt(CorrectAnswersDuringBoss, 1);
                }
            }
            else
            {
                enemy.PlayHitAnimation();
                isLecturerFainted = enemy.Lecturer.TakeDamage(q, player.Lecturer, 10);
            }
            yield return enemyHud.UpdateKP();
        }
        else
        {
            if (IsBossFight)
            {
                boss.PlayAttacAnimation();
            }
            else
            {
                enemy.PlayAttacAnimation();
            }
            if (IsComeback)
            {
                hero.PlayHitAnimation();
            }
            else
            {
                player.PlayHitAnimation();
            }
            if (IsBossFight)
            {
                isPlayerFainted = player.Lecturer.TakeDamage(q, player.Lecturer, 20);
            }
            else
            {
                isPlayerFainted = player.Lecturer.TakeDamage(q, player.Lecturer, 10);
            }
            yield return playerHud.UpdateKP();

        }
        if (isLecturerFainted)
        {
            if (IsComeback)
            {
                battleCam.DOOrthoSize(3.5f, 0.5f);
                hero.PlayFinalMovementAnimation();
                yield return new WaitForSeconds(2f);
                battleCam.DOOrthoSize(6.3f, 0.5f);
                yield return new WaitForSeconds(2.5f);
                boss.LoopHitAnimation();
                yield return new WaitForSeconds(8f);
            }
            yield return dialogBox.TypeDialog("You are worthy...");
            if (IsBossFight)
            {
                boss.PLayFaintAnimation();
            }
            else
            {
                enemy.PLayFaintAnimation();
            }
            yield return new WaitForSeconds(2f);
            OnBattleOver(true);
            if (IsBossFight)
            {
                OnWiningGame(true);
            }
        }
        else if (isPlayerFainted)
        {
            if (IsBossFight)
            {
                if (noOfFaints == 0)
                {
                    noOfFaints += 1;
                    if (PlayerPrefs.GetInt(CorrectAnswersDuringBoss) > 0)
                    {
                        yield return dialogBox.TypeDialog("................");
                        yield return new WaitForSeconds(2f);
                        boss.PlayAttacAnimation();
                        player.PlayHitAnimation();
                        bossMusic.Stop();

                        yield return dialogBox.TypeDialog("................");
                        yield return new WaitForSeconds(2f);
                        boss.PlayAttacAnimation();
                        player.PlayHitAnimation();
                        player.Lecturer.TakeDamage(q, player.Lecturer, 20);
                        yield return dialogBox.TypeDialog("wHy Won'T yUou dIeeeeeeeeeeeeeeeeeeeee??????????????");
                        yield return new WaitForSeconds(2f);
                        boss.PlayAttacAnimation();
                        player.PlayHitAnimation();
                        player.Lecturer.TakeDamage(q, player.Lecturer, 20);
                        yield return new WaitForSeconds(2f);
                        boss.PlayAttacAnimation();
                        player.PlayHitAnimation();
                        player.Lecturer.TakeDamage(q, player.Lecturer, 20);
                        yield return new WaitForSeconds(2f);

                        yield return new WaitForSeconds(2f);
                        comeBackMusic.Play();
                        comeBackMusic.DOFade(0.5f, 5f);


                        dialogBox.SetColor(Color.blue);
                        yield return dialogBox.TypeDialog("*You could have RUN....");
                        yield return new WaitForSeconds(2f);
                        yield return dialogBox.TypeDialog("*But you PERSISTED");
                        yield return new WaitForSeconds(2f);
                        yield return dialogBox.TypeDialog("*You have one more chance left... \n Do not waste it...");
                        IsComeback = true;
                        player.PlayHealAnimation();
                        player.Lecturer.TakeDamage(q, player.Lecturer, -50);
                        yield return playerHud.UpdateKP();
                        yield return new WaitForSeconds(2f);

                        _bossCanvasGroup.DOFade(1, 1f);
                        yield return new WaitForSeconds(1f);
                        player.PLayFaintAnimation();
                        hero.Setup("Hero");
                        yield return new WaitForSeconds(2f);
                        _bossCanvasGroup.DOFade(0, 2f);
                        yield return new WaitForSeconds(2f);

                        StartCoroutine(EnemyMove());
                    }
                    else
                    {
                        yield return dialogBox.TypeDialog("gOod bYeeeeeee");
                        hero.PLayFaintAnimation();
                        yield return new WaitForSeconds(2f);
                        OnBattleOver(false);
                        OnWiningGame(false);
                    }
                }
                else
                {
                    yield return dialogBox.TypeDialog("gOod bYeeeeeee");
                    player.PLayFaintAnimation();
                    yield return new WaitForSeconds(2f);
                    OnBattleOver(false);
                    OnWiningGame(false);
                }
            }
            else
            {
                yield return dialogBox.TypeDialog("You are not worthy");
                player.PLayFaintAnimation();
                yield return new WaitForSeconds(2f);
                OnBattleOver(false);
                OnWiningGame(false);
            }
        }
        else
        {
            StartCoroutine(EnemyMove());
        }
    }

    IEnumerator EnemyMove()
    {
        state = BattleState.EnemyMove; 
        if (GlobalVariables.difficulty == 0)
        {
            q = enemy.Lecturer.GetRandomQuestion(questionEasy);
        }
        else if (GlobalVariables.difficulty == 1)
        {
            q = enemy.Lecturer.GetRandomQuestion(learning);
        }
        yield return dialogBox.TypeDialog($"Next question is...\n{q.Question}");
        yield return new WaitForSeconds(2f);
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
        timer.StartTimer();
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
            if (timer.GetTime() < 7)
            {
                timer.timerText.color = Color.red;
            }
            else
            {
                timer.timerText.color = Color.white;
            }
            if (timer.GetTime() == 0)
            {
                dialogBox.EnabledMoveSelector(false);
                dialogBox.EnableDialogText(true);
                StartCoroutine(PerformPlayerMove(2));
            }
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

    IEnumerator HandleRun()
    {
        if (IsBossFight)
        {
            dialogBox.EnabledActionSelector(false);
            yield return dialogBox.TypeDialog($"yOu aRe nOt GoInG AnYWhEreeeeeeeeeeeeeeeeeeeeeeeeeeee");
            yield return new WaitForSeconds(1f);
            if (IsComeback)
            {
                hero.PlayHitAnimation();
            }
            else
            {
                player.PlayHitAnimation();
            }
            bool isPlayerFainted = player.Lecturer.TakeDamage(q, player.Lecturer, 20);
            yield return playerHud.UpdateKP();
            dialogBox.EnabledActionSelector(true);
            if (isPlayerFainted)
            {
                yield return dialogBox.TypeDialog("gOod bYeeeeeee");
                player.PLayFaintAnimation();
                yield return new WaitForSeconds(2f);
                OnBattleOver(false);
                OnWiningGame(false);
            }
        }
        else
        {
            yield return dialogBox.TypeDialog($"You coward...");
            yield return new WaitForSeconds(1f);
            OnBattleOver(true);
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
                StartCoroutine(HandleRun());
            }

        }
    }
}
