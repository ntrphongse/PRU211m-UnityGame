using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using ExitGames.Client.Photon;

public class PlayerMovement : MonoBehaviourPun
{
    [SerializeField] BattleDialogBox dialogBox;
    [SerializeField] GameController gameController;
    public float moveSpeed;
    public LayerMask solidObjects;
    public LayerMask interactableLayer;

    public bool isFighting;
    public bool isDancing;

    public bool isWaiting;

    int currentAction;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public event Action<string> OnEncounter;

    PhotonView view;

    public Player player;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        view = GetComponent<PhotonView>();
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        player = PhotonNetwork.LocalPlayer;
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { "isChoosing", false } });
    }


    public void HandleUpdate()
    {
        bool isChoosing = false;
        if (PhotonNetwork.IsConnected && PhotonNetwork.LocalPlayer != null)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("isChoosing"))
            {
                isChoosing = (bool)PhotonNetwork.LocalPlayer.CustomProperties["isChoosing"];
            }
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
                    else
                    {
                        if (!PhotonNetwork.IsConnected || (PhotonNetwork.IsConnected && view.IsMine))
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
                        if (Input.GetKeyDown(KeyCode.Z)) Interact();
                    }
                }
            }
        }
    }
    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        Debug.Log(collider.gameObject.name);
        if (collider != null)
        {
            if (collider.gameObject.name.Contains("Player"))
            {
                if (PhotonNetwork.IsConnected && SceneManager.GetActiveScene().name == "Room")
                {
                    dialogBox = gameController.ToggleChallengeDialogBox(true);
                    view.RPC("BeChallenged", collider.GetComponent<PlayerMovement>().player);
                }
            }
            else
            {
                collider.GetComponent<Interactable>()?.Interact(collider);
            }
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
    public void Fight()
    {
    }


    public void Challenge()
    {
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
