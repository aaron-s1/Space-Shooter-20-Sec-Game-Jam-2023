using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    int regularKills;
    int blackHoleKills;
    int totalKills;

    int score;

    // public int Score =>
        // score;


    void Awake() =>
        Instance = this;


    public void AddToKills(bool killCameFromBlackHole = false)
    {
        if (!killCameFromBlackHole)
            regularKills++;
        else
            blackHoleKills++;

        totalKills++;

        AdjustScore(killCameFromBlackHole);        
    }


    void AdjustScore(bool killCameFromBlackHole = false)
    {
        if (killCameFromBlackHole)
            score += 5;
        else
            score += 10;

        // adjust the UI display text for score
    }
}
