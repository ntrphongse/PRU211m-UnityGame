using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] GameController gameController;
    public float moveSpeed;
    public LayerMask solidObjects;
    public LayerMask interactableLayer;

    public const string FacingUp1 = "character_24";
    public const string FacingUp2 = "character_25";
    public const string FacingUp3 = "character_26";
    public const string FacingUp4 = "character_27";

    public const string FacingRight1 = "character_13";
    public const string FacingRight2 = "character_14";
    public const string FacingRight3 = "character_15";
    public const string FacingRight4 = "character_16";

    public const string FacingDown1 = "character_0";
    public const string FacingDown2 = "character_1";
    public const string FacingDown3 = "character_2";
    public const string FacingDown4 = "character_3";

    public const string FacingLeft1 = "character_35";
    public const string FacingLeft2 = "character_36";
    public const string FacingLeft3 = "character_37";
    public const string FacingLeft4 = "character_38";


    public bool isInFightRoom;
    public bool isFighting = false;
    public bool isDoneFighting = false;
    public bool isDancing;
    public bool isReadyToFight = false;
    public bool isChallenger = false;
    public bool isCheckEnd = true;


    public bool isWaiting;

    int currentAction;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public event Action<string> OnEncounter;

    PhotonView view;

    public Player player;

    [SerializeField] public SceneTransition transition;
    [SerializeField] public Text TakeSidetext;
    [SerializeField] public AudioSource pvpMusic;
    [SerializeField] public AudioSource victoryMusic;
    [SerializeField] public SpriteRenderer sr;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        sr = GetComponent<SpriteRenderer>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        player = PhotonNetwork.LocalPlayer;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { "isChoosing", false } });
        transition = GameObject.Find("SceneTransition")?.GetComponent<SceneTransition>();
        TakeSidetext = GameObject.Find("Take your side")?.GetComponent<Text>();
        pvpMusic = GameObject.Find("MusicBackground")?.GetComponent<AudioSource>();
        victoryMusic = GameObject.Find("BgSound")?.GetComponent<AudioSource>();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "beginFight", true } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isFighting", false } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isDoneFighting", false } });
    }
    public void HandleUpdate()
    {
        isInFightRoom = SceneManager.GetActiveScene().name == "FightRoom";
        bool isChoosing = false;
        bool isLeftRoom = false;
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isChoosing"))
            {
                isChoosing = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isChoosing"];
            }
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isLeftRoom"))
            {
                isLeftRoom = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isLeftRoom"];
            }
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isReadyToFight"))
            {
                isReadyToFight = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isReadyToFight"];
            }
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isFighting"))
            {
                isFighting = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isFighting"];
            }
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isChallenger"))
            {
                isChallenger = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isChallenger"];
            }
        }
        Debug.Log("Done: " + isDoneFighting);
        Debug.Log("ischeck: " + isCheckEnd);
        Debug.Log("inroom: " + isInFightRoom);

        if (isLeftRoom)
        {
            view.RPC("HasLeftRoom", RpcTarget.All);
        }
        if (animator != null)
        {
            if (SceneManager.GetActiveScene().name == "GameOver")
            {
                animator.SetBool("IsDead", true);
            }
            else
            {
                if (!isMoving)
                {
                    if (isChoosing)
                    {
                        if (dialogBox == null)
                        {
                            dialogBox = gameController.ToggleChallengeDialogBox(true);
                        }
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
                                view.RPC("Fight", RpcTarget.All);
                            }
                            else if (currentAction == 1)
                            {
                                view.RPC("BeDeclined", RpcTarget.All);
                            }
                        }
                    }
                    else if (isDoneFighting && isCheckEnd && isInFightRoom)
                    {
                        isCheckEnd = false;
                        isDoneFighting = false;
                        view.RPC("EndFight", RpcTarget.All);
                    }
                    else if (isInFightRoom && isReadyToFight && !isFighting)
                    {
                        var beginFight = (bool)player.CustomProperties["beginFight"];
                        foreach (var player in PhotonNetwork.PlayerList)
                        {
                            if (!(bool)player.CustomProperties["isReadyToFight"])
                            {
                                beginFight = false;
                            }
                        }
                        if (beginFight)
                        {
                            view.RPC("BeginFight", RpcTarget.All);
                        }
                    }

                    else
                    {
                        if (!PhotonNetwork.IsConnected || (PhotonNetwork.IsConnected && view.IsMine) || isFighting)
                        {
                            input.x = Input.GetAxisRaw("Horizontal");
                            input.y = Input.GetAxisRaw("Vertical");
                            if (input.x != 0) input.y = 0;
                            if (input != Vector2.zero)
                            {
                                animator.SetFloat("moveX", input.x);
                                animator.SetFloat("moveY", input.y);

                                var targetPos = transform.position;
                                targetPos.x += input.x;
                                targetPos.y += input.y;
                                if (IsWalkable(targetPos))
                                {
                                    StartCoroutine(Move(targetPos));
                                }
                            }
                        }
                        animator.SetBool("isMoving", isMoving);
                        if (Input.GetKeyDown(KeyCode.Z) && !isInFightRoom) Interact();
                        if (isInFightRoom && !isReadyToFight) view.RPC("CheckReady", RpcTarget.All);
                        if (isFighting && isInFightRoom)
                        {
                            if (Input.GetKeyDown(KeyCode.Z)) Attack();
                            view.RPC("CheckEndFight", RpcTarget.All);
                            var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
                            var interactPos = transform.position + facingDir;
                            var collider = Physics2D.OverlapCircle(interactPos, 0.5f, interactableLayer);
                            if (collider != null)
                            {
                                if (collider.gameObject.name.Contains("Player"))
                                {
                                    view.RPC("DoDamage", RpcTarget.All);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);

        if (collider != null)
        {
            if (collider.gameObject.name.Contains("Player"))
            {
                if (PhotonNetwork.IsConnected && SceneManager.GetActiveScene().name == "Room")
                {
                    dialogBox = gameController.ToggleChallengeDialogBox(true);

                    //view.RPC("BeChallenged", PhotonNetwork.CurrentRoom.GetPlayer(collider.GetComponent<PlayerMovement>().player.ActorNumber));
                    view.RPC("BeChallenged", RpcTarget.All);
                }
            }
            else
            {
                collider.GetComponent<Interactable>()?.Interact(collider);
            }
        }
    }

    public void HasLeftRoom()
    {
        dialogBox.EnabledActionSelector(false);
        dialogBox = gameController.ToggleChallengeDialogBox(false);
    }

    [PunRPC]
    public IEnumerator EndFight()
    {
        isDoneFighting = false;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isDoneFighting", false } });
        victoryMusic.Play();
        pvpMusic.Stop();
        TakeSidetext.text = "";
        if (dialogBox == null)
        {
            dialogBox = gameController.ToggleChallengeDialogBox(true);
        }
        dialogBox.EnableDialogText(true);
        StartCoroutine(dialogBox.TypeDialog("The battle has ended!!!"));
        yield return new WaitForSeconds(2f);
        StartCoroutine(dialogBox.TypeDialog("You will now be returned to the study hall! "));
        yield return new WaitForSeconds(2f);
        transition.ConnectScene("Room");
    }

    [PunRPC]
    void Attack()
    {
        if (view.IsMine)
        {
            var originalPos = sr.transform.localPosition;
            var seq = DOTween.Sequence();
            switch (sr.sprite.name)
            {
                case FacingDown1:
                case FacingDown2:
                case FacingDown3:
                case FacingDown4: seq.Append(sr.transform.DOLocalMoveY(originalPos.y - 1.5f, 0.25f)); break;
                case FacingUp1:
                case FacingUp2:
                case FacingUp3:
                case FacingUp4: seq.Append(sr.transform.DOLocalMoveY(originalPos.y + 1.5f, 0.25f)); break;
                case FacingRight1:
                case FacingRight2:
                case FacingRight3:
                case FacingRight4: seq.Append(sr.transform.DOLocalMoveX(originalPos.x + 1.5f, 0.25f)); break;
                case FacingLeft1:
                case FacingLeft2:
                case FacingLeft3:
                case FacingLeft4: seq.Append(sr.transform.DOLocalMoveX(originalPos.x - 1.5f, 0.25f)); break;
            }
        }
    }

    [PunRPC]
    public void DoDamage()
    {
        if (!view.IsMine) return;
        var originalPos = sr.transform.localPosition;
        var seq = DOTween.Sequence();
        switch (sr.sprite.name)
        {
            case FacingDown1:
            case FacingDown2:
            case FacingDown3:
            case FacingDown4: seq.Append(sr.transform.DOLocalMoveY(originalPos.y + 2f, 0.25f)); break;
            case FacingUp1:
            case FacingUp2:
            case FacingUp3:
            case FacingUp4: seq.Append(sr.transform.DOLocalMoveY(originalPos.y - 2f, 0.25f)); break;
            case FacingRight1:
            case FacingRight2:
            case FacingRight3:
            case FacingRight4: seq.Append(sr.transform.DOLocalMoveX(originalPos.x - 2f, 0.25f)); break;
            case FacingLeft1:
            case FacingLeft2:
            case FacingLeft3:
            case FacingLeft4: seq.Append(sr.transform.DOLocalMoveX(originalPos.x + 2f, 0.25f)); break;
        }
    }

    IEnumerator Move(Vector3 targetPos)
    {
        isMoving = true;
        while ((targetPos - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }
    private bool IsWalkable(Vector3 targetPos)
    {
        return !(Physics2D.OverlapCircle(targetPos, 0.03f, solidObjects | interactableLayer) != null);
    }

    [PunRPC]
    public void BeginFight()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "beginFight", false } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isReadyToFight", false } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isFighting", true } });
        TakeSidetext.text = "Push the opponent out of the field to win!";
        pvpMusic.Play();
    }

    [PunRPC]
    public IEnumerator CheckEndFight()
    {
        if (transform.localPosition.y >= 4 || transform.localPosition.y <= -6 || transform.localPosition.x >= 8 || transform.localPosition.x <= -7)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isFighting", false } });
            TakeSidetext.text = "The struggle has ended!!!";
            yield return new WaitForSeconds(1f);
            view.RPC("SetEndFight", RpcTarget.All);
        }
    }

    [PunRPC]
    public void SetEndFight()
    {
        isDoneFighting = true;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isDoneFighting", false } });
    }

    [PunRPC]
    public void CheckReady()
    {
        if (view.IsMine)
        {
            if ((new Vector3(-4, 0, 0) == transform.position) || (new Vector3(5, 0, 0) == transform.position))
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isReadyToFight", true } });
            }
        }
        else
        {
            if ((new Vector3(-4, 0, 0) == transform.position) || (new Vector3(5, 0, 0) == transform.position))
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isReadyToFight", true } });
            }
        }
    }

    [PunRPC]
    public void Fight()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isChoosing", false } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isReadyToFight", false } });
        transition.ConnectScene("FightRoom");
    }

    [PunRPC]
    public void ExitFight()
    {
        transition.ConnectScene("ShoolYard");
    }


    public void Challenge()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isChoosing", true } });
        dialogBox.EnableDialogText(true);
        StartCoroutine(dialogBox.TypeDialog("Invitation sent! Awaiting response from the opponent... "));
    }
    [PunRPC]
    public IEnumerator BeDeclined()
    {
        if (dialogBox == null)
        {
            dialogBox = gameController.ToggleChallengeDialogBox(true);
        }
        if (view.IsMine)
        {
            StartCoroutine(dialogBox.TypeDialog("You have declined the invitation"));
            yield return new WaitForSeconds(2f);
        }
        else
        {
            StartCoroutine(dialogBox.TypeDialog("Your opponent has declined the invitation... "));
            yield return new WaitForSeconds(2f);
        }
        dialogBox.EnabledActionSelector(false);
        dialogBox = gameController.ToggleChallengeDialogBox(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isChoosing", false } });

    }

    [PunRPC]
    public IEnumerator BeChallenged()
    {
        dialogBox = gameController.ToggleChallengeDialogBox(true);
        if (view.IsMine)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isChallenger", true } });
            Challenge();
        }
        if (!view.IsMine)
        {
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { "isChoosing", true } });
            dialogBox.EnableDialogText(true);
            StartCoroutine(dialogBox.TypeDialog("A player has challenged you in the art of dueling"));
            yield return new WaitForSeconds(2f);
            StartCoroutine(dialogBox.TypeDialog("Choose an action: "));
            dialogBox.EnabledActionSelector(true);
        }
    }

    public Sprite Sprite
    {
        get => Sprite;
    }
}
