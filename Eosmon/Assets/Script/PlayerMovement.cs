using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public LayerMask solidObjects;
    public LayerMask interactableLayer;

    private bool isMoving;
    private bool isDead;

    private Vector2 input;

    private Animator animator;

    public event Action OnEncounter;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void CheckEncounter()
    {
        if (UnityEngine.Random.Range(1, 10) < 5)
        {
            OnEncounter();
        }
    }

    public void HandleUpdate()
    {
        if (SceneManager.GetActiveScene().name == "GameOver")
        {
            animator.SetBool("IsDead", true);
        }
        else
        {

            if (!isMoving)
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
                        StartCoroutine(Move(targetPos));
                }
            }
            animator.SetBool("isMoving", isMoving);
            if (Input.GetKeyDown(KeyCode.Z)) Interact();

        }


    }
    void Interact()
    {
        var facingDir = new Vector3(animator.GetFloat("moveX"), animator.GetFloat("moveY"));
        var interactPos = transform.position + facingDir;
        Debug.DrawLine(transform.position, interactPos, Color.green, 0.5f);
        var collider = Physics2D.OverlapCircle(interactPos, 0.3f, interactableLayer);
        if (collider != null)
        {
            collider.GetComponent<Interactable>()?.Interact(collider);
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
        return !(Physics2D.OverlapCircle(targetPos, 0.3f, solidObjects | interactableLayer) != null);

    }

    public Sprite Sprite
    {
        get => Sprite;
    }
}
