using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BattleUnit : MonoBehaviour
{
    [SerializeField] MobBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;
    public Lecturer Lecturer { get; set; }
    public void Setup()
    {
        Lecturer = new Lecturer(_base, level);
        if (isPlayerUnit)
        {
            GetComponent<Image>().sprite = Lecturer.Base.FrontSprite;
        }
        else
        {
            GetComponent<Image>().sprite = Lecturer.Base.BackSprite;
        }
    }
}
