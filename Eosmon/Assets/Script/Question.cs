using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QuestionBase : ScriptableObject
{
    public string Question { get; set; }
    public SuggestedAnswers SuggestedAnswers { get; set; }
    public string CorrectAnswer { get; set; }

    public QuestionBase(string _Question, SuggestedAnswers _SuggestedAnswers, string _CorrectAnswer)
    {
        this.Question = _Question;
        this.SuggestedAnswers = _SuggestedAnswers;
        this.CorrectAnswer = _CorrectAnswer;
    }

}
