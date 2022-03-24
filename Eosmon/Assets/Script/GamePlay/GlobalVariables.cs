using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static int totalScore;
    public static int difficulty;
    public static int hasSound;

    public void setDifficulty(int option)
    {
        difficulty = option;
        Debug.Log(difficulty);
    }
}
