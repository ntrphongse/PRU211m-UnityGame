using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public Text theText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        theText.color = Color.red; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        theText.color = Color.white; 
    }
}