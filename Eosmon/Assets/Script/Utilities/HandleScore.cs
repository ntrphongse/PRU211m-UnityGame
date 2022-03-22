using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandleScore : MonoBehaviour
{
    public Text scoreText;
    static int score;
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        scoreText.text = score.ToString();
    } 
    
    //this game object will not get destroyed between scene loading
    void Awake()
    {
        DontDestroyOnLoad(scoreText);
    }

    void addScore()
    {
        score += 5; 
        scoreText.text = score.ToString();
    }

    void diffScore()
    {
        score -= 5;
        scoreText.text = score.ToString();
    }

    void Update()
    {
    }
}
