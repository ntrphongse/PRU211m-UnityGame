using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Answer", menuName="Answer/Create new answer")]
public class AnswerBase : ScriptableObject
{
    [SerializeField] string name;
    [TextArea]
    [SerializeField] string description;
    [SerializeField] mobType type;
    [SerializeField] int power;


    public AnswerBase(string name, string description, mobType type, int power)
    {
        this.name = name;
        this.description = description;
        this.type = type;
        this.power = power;
    }

    public string Name
    {
        get { return name; }
    }
    public string Description
    {
        get { return description; }
    }
    public mobType Type
    {
        get { return type; }
    }
    public int Power
    {
        get { return power; }
    }
}
