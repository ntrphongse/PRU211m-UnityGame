using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] MobBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    Image image;

    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public Lecturer Lecturer { get; set; }
    public void Setup()
    {
        Lecturer = new Lecturer(_base, level);
        if (isPlayerUnit)
        {
            image.sprite = Lecturer.Base.FrontSprite;
        }
        else
        {
            image.sprite = Lecturer.Base.BackSprite;
        }
    }

    public void PlayEnterAnimation()
    {

    }
}
