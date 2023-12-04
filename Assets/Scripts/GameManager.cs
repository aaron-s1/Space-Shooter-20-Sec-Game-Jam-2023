using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] TextMeshProUGUI secondsRemainingUI;
    [SerializeField] FadeBeforeEnd fadeBeforeEnd;

    [Space(10)]
    [SerializeField] PowerUpSelector powerUpSelector;

    IEnumerator spawnEnemies;

    // [SerializeField] GameObject blackHolePowerUpParticle;

    PlayerController player;

    
    int secondsPassedSinceGameStart;

    int regularKills;
    int blackHoleKills;
    int totalKills;

    int score;

    bool gameHasStarted = false;
    bool gameHasEnded = false;


    // bool enemiesNowExplode;
    

    // public int Score =>
        // score;


    void Awake() =>
        Instance = this;

    void Start()
    {
        gameHasStarted = true;
        // later on, add conditionals before starting
        // take off the 1.
        StartCoroutine("PrepStartOfGame");

        // spawnEnemies = gameObject.GetComponent<EnemySpawner>().Spawn();

        player = PlayerController.Instance;
        // player = GameObject.FindGameObjectWithTag("Player");
        // StartCoroutine("ActivateBlackHole");
        // Invoke("ActivateBlackHole", 6f);
    }
    
    // void StartGame()
    // {
    //     gameHasStarted = true;        
    // }

    #region Handle game states.
    
    IEnumerator PrepStartOfGame()
    {
        // do stuff.
        // do UI stuff.
        // yield times.

        yield return StartCoroutine(StartGame());
    }


    IEnumerator StartGame()
    {
        FireMissile.Instance.PrepFiringThenFire();

        yield return new WaitForSeconds(1f);
        secondsPassedSinceGameStart++;
        secondsRemainingUI.text = (20 - secondsPassedSinceGameStart).ToString();

        if (secondsPassedSinceGameStart == 4)
            StartCoroutine(powerUpSelector.SpawnDrones());


        if (secondsPassedSinceGameStart == 20)
        {
            secondsRemainingUI.text = " ";
            yield return StartCoroutine("EndGame");
        }
        else
            yield return StartCoroutine("CountdownToEndGame");
    }

    IEnumerator EndGame()
    {
        gameHasEnded = true;
        StopCoroutine(spawnEnemies);

        GameObject.FindObjectOfType<PowerUpSelector>().GetComponent<PowerUpSelector>().StopAllCoroutines();

        yield return StartCoroutine(player.Die());

        fadeBeforeEnd.TallyUpKillsAndScore(regularKills, blackHoleKills);

        FullyEndGame();

        yield break;
    }

    void FullyEndGame()
    {

    }

    #endregion

    #region Power-ups.



    
    

    #endregion



    #region Score system.
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
            score += 10;
        else
            score += 10;

        scoreUI.text = score.ToString();        
    }
    #endregion

}
