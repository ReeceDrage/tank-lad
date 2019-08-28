using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScoreChange : MonoBehaviour
{
    [SerializeField] private int scoreChangeOnHit = 15;
    [SerializeField] private int scoreChangeOnDeath = 45;

    private GameObject gameManager;

    private void Start()
    {
        gameManager = GameObject.Find("Game State Manager");
    }

    private void ApplyHit()
    {
        gameManager.SendMessage("AddScore", scoreChangeOnHit);
    }

    private void ApplyDeath()
    {
        gameManager.SendMessage("AddScore", scoreChangeOnDeath);
    }
}
