using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KPBar : MonoBehaviour
{
    [SerializeField] GameObject knowledge;
    // Start is called before the first frame update
    void Start()
    {
        knowledge.transform.localScale = new Vector3(0.5f, 1f);
    }

    public void SetKP(float kpNormalised)
    {
        knowledge.transform.localScale = new Vector3(kpNormalised, 1f);
    }
}
