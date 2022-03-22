using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Lecturer", menuName = "Lecturer/Create new Lecturer")]
public class MobBase : ScriptableObject
{
    [SerializeField] string mob_name;

    [TextArea]
    [SerializeField] string description;

    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    //Base stats
    [SerializeField] int maxKp;
    [SerializeField] int attack = 10;
    [SerializeField] int defense = 5;
    [SerializeField] int speed;
    public void SetName(string name)
    {
        mob_name = name;
    }
    public void SetFrontSprite(Sprite sprite)
    {
        frontSprite = sprite;
    }
    public void SetBackSprite(Sprite sprite)
    {
        backSprite = sprite;
    }
    public string Name
    {
        get { return mob_name; }
    }

    public Sprite FrontSprite
    {
        get { return frontSprite; }
    }
    public Sprite BackSprite
    {
        get { return backSprite; }
    }
    public string Description
    {
        get { return description; }
    }
    public void SetMaxKp(int MaxKp)
    {
        maxKp = MaxKp;
    }
    public int MaxKp
    {
        get { return maxKp; }
    }
    public int Attack
    {
        get { return attack; }
    }
    public int Defense
    {
        get { return defense; }
    }
    public int Speed
    {
        get { return speed; }
    }
}


public enum mobType
{
    None,
    Omnigod,
    Virtuoso,
    Regular
}
