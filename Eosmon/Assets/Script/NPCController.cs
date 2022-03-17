
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;
   public void Interact()
    {
        Debug.Log("NPC interact!");
        StartCoroutine(DialogManager.Instance.ShowDialog(dialog));
    }
}
