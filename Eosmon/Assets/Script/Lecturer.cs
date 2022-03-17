using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lecturer : MonoBehaviour
{
    public MobBase Base { get; set; }
    public int Level;

    public int KP { get; set; }
    public Lecturer(MobBase lBase, int lLevel)
    {
        Base = lBase;
        Level = lLevel;
        KP = MaxKp;
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; }
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; }
    }
    public int MaxKp
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 50; }
    }

    public bool TakeDamage(QuestionBase question, Lecturer answerer)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float d = 10 * ((float)answerer.Attack / Defense) * 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        KP -= damage;
        if (KP <= 0)
        {
            KP = 0;
            return true;
        }
        else
        {
            return false;
        }
    }

    public QuestionBase GetRandomQuestion(TextAsset jsonFile)
    {
        string jsonString = JsonHelper.fixJson(jsonFile.text);
        Debug.Log(jsonString);
        JsonHelper questions = JsonConvert.DeserializeObject<JsonHelper>(jsonString);
        Debug.Log(questions);

        int ran = (int)Mathf.Ceil(Random.Range(0, (questions.Items.Count - 2)));
        QuestionBase question = questions.Items[ran];
        Debug.Log(question.Question.ToString());
        return question;
    }
}

