using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private long score = 0;
    [SerializeField] private Text scoreDisplay;

    public void Update()
    {
        DisplayScore();
    }

    private void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    private void DisplayScore()
    {
        scoreDisplay.text = "Score : " + score.ToString();
    }
}
