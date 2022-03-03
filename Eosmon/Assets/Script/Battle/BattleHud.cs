using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] KPBar kpBar;

    Lecturer _lecturer;

    public void SetData(Lecturer lecturer)
    {
        _lecturer = lecturer;
        nameText.text = lecturer.Base.Name;
        levelText.text = "lvl" + lecturer.Level;
        kpBar.SetKP(lecturer.KP / lecturer.MaxKp);
    }

    public IEnumerator UpdateKP()
    {
        yield return kpBar.SetKpSmooth((float)_lecturer.KP / (float)_lecturer.MaxKp);
    }
}
