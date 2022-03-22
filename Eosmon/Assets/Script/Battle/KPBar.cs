using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KPBar : MonoBehaviour
{
    [SerializeField] GameObject knowledge;

    // Start is called before the first frame update
    void Start()
    {
        knowledge.transform.localScale = new Vector3(1f, 1f);
    }

    public void SetKP(float kpNormalised)
    {
        knowledge.transform.localScale = new Vector3(kpNormalised, 1f);
    }

    public IEnumerator SetKpSmooth(float newKp)
    {
        float curKp = knowledge.transform.localScale.x;
        float changeAmt = curKp - newKp;
        while (curKp - newKp > Mathf.Epsilon)
        {
            curKp -= changeAmt * Time.deltaTime;
            knowledge.transform.localScale = new Vector3(curKp, 1f);
            yield return null;
        }
        knowledge.transform.localScale = new Vector3(newKp, 1f);
    }
}
