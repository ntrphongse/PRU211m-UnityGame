using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHud : MonoBehaviour
{
    [SerializeField] Text nameText;
    [SerializeField] Text levelText;
    [SerializeField] KPBar kpBar;
    [SerializeField] Image kpImage;

    Lecturer _lecturer;

    public void SetData(Lecturer lecturer)
    {
        _lecturer = lecturer;
        nameText.text = lecturer.Base.Name;
        levelText.text = "lvl" + lecturer.Level;
        if (lecturer.Base.Name == "01101100011010000110101101110000")
        {
            kpImage.color = Color.red;
            kpBar.SetKP(lecturer.KP / lecturer.MaxKp);
        }
        else
        {
            kpBar.SetKP(lecturer.KP / lecturer.MaxKp);
        }
    }

    public IEnumerator UpdateKP()
    {
        yield return kpBar.SetKpSmooth((float)_lecturer.KP / (float)_lecturer.MaxKp);
    }
}
