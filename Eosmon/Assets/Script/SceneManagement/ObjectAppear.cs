using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectAppear : MonoBehaviour
{
    [SerializeField] private Image customSprite;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnOpen();
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnClose();
        }
    }
    void OnOpen()
    {
        customSprite.enabled = true;
    }
    void OnClose()
    {
        customSprite.enabled = true;
    }
}
