using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SuggestedAnswers : ScriptableObject
{
    public string A { get; set; }
    public string B { get; set; }
    public string C { get; set; }
    public string D { get; set; }
    public SuggestedAnswers(string a, string b, string c, string d)
    {
        this.A = a;
        this.B = b;
        this.C = c;
        this.D = d;

    }
}
