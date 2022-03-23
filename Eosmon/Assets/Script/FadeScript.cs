using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeScript : MonoBehaviour
{
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private bool fadeIn;
    [SerializeField] private bool fadeOut;

    void Start()
    {
        fadeIn = true;
    }

    public void Hide()
    {
        fadeOut = true;
    }

    void Update()
    {
        if (fadeIn)
        {
            if (_canvasGroup.alpha < 1)
            {
                _canvasGroup.alpha += Time.deltaTime;
                if (_canvasGroup.alpha >= 1)
                {
                    fadeIn = false;
                }
            }
        }
        if (fadeOut)
        {
            if (_canvasGroup.alpha >= 0)
            {
                _canvasGroup.alpha -= Time.deltaTime;
                if (_canvasGroup.alpha == 0)
                {
                    fadeOut = false;
                }
            }
        }
    }
}
