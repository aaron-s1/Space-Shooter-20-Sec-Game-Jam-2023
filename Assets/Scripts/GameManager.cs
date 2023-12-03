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

    [SerializeField] GameObject endGameUIScreen;

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
        StartCoroutine("CountdownToEndGame", 1f);

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
    IEnumerator CountdownToEndGame()
    {
        // spawnEnemies = StartCoroutine(gameObject.GetComponent<EnemySpawner>().Spawn());
        // StartCoroutine(spawnEnemies);

        yield return new WaitForSeconds(1f);
        secondsPassedSinceGameStart++;
        secondsRemainingUI.text = (20 - secondsPassedSinceGameStart).ToString();


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

        endGameUIScreen.SetActive(true);

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
            score += 5;
        else
            score += 10;

        scoreUI.text = score.ToString();        
    }
    #endregion

}
