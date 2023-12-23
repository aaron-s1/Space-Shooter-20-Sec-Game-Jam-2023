using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] float whenToStartAmbienceMusic;
    [HideInInspector] public bool gameHasEnded;
    [HideInInspector] public int regularKills;
    [HideInInspector] public int blackHoleKills;

    [SerializeField] TextMeshProUGUI scoreUI;
    [SerializeField] TextMeshProUGUI secondsRemainingUI;

    [SerializeField] GameObject endGameScreen;
    
    [SerializeField] GameObject blackBackgroundScreen;
    [Space(10)]
    [SerializeField] PowerUpSelector powerUpSelector;

    PlayerController player;
    // EndGameScreen blackHoleScorePerEnemy;
    IEnumerator spawnEnemies;
    
    int secondsPassedSinceGameStart;

    int totalKills;
    int score;

    bool gameHasStarted;


    public bool blackHoleAteAllEnemies;


    

    // public int Score =>
        // score;


    void Awake() =>
        Instance = this;

    void Start()
    {
        gameHasStarted = true;
        spawnEnemies = EnemySpawner.Instance.SpawnWaves();
        player = PlayerController.Instance;
    }
    

    #region Handle game states.
    
    public IEnumerator PrepStartOfGame(GameObject howToPlayScreen)
    {
        yield return StartCoroutine(SoundManager.Instance.PlayStartButtonClip());
        blackBackgroundScreen.SetActive(false);
        howToPlayScreen.SetActive(false);
        player.gameObject.SetActive(true);
        StartCoroutine(spawnEnemies);

        yield return StartCoroutine(CountdownBeforeGameStarts());
    }

    // Give player a few seconds to get ready. Add effects or countdown or something later.
    IEnumerator CountdownBeforeGameStarts()
    {
        GetComponent<AudioSource>().time = whenToStartAmbienceMusic;
        GetComponent<AudioSource>().Play();

        for (int i = 0; i < 3; i++)
        {
            yield return new WaitForSeconds(1f);

            int secondsBeforeGameStart = 3 - i - 1;
            secondsRemainingUI.text = (20 + secondsBeforeGameStart).ToString();

            if (secondsBeforeGameStart == 2)
            {
                if (spawnEnemies != null)
                    StartCoroutine(spawnEnemies);
            }

            if (secondsBeforeGameStart == 1)
                player.gameObject.GetComponent<Animator>().SetTrigger("PlayerComesIn");


            if (secondsBeforeGameStart == 0)
            {
                StartCoroutine(CountdownToEndGame());
                FireMissile.Instance.PrepFiringThenFire();
            }
        }

        yield break;        
    }


    IEnumerator CountdownToEndGame()
    {
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
        // if (spawnEnemies != null)

        // Debug.Break();
        GameObject.FindObjectOfType<PowerUpSelector>().GetComponent<PowerUpSelector>().StopAllCoroutines();

        yield return StartCoroutine(player.Die());
        // Debug.Break();

        blackBackgroundScreen.SetActive(true);
        endGameScreen.SetActive(true);
        endGameScreen.GetComponent<EndGameScreen>().TallyUpKillsAndScore(regularKills, blackHoleKills);

        // GetComponent<EnemySpawner>().GetComponent<EnemyPool>().ListEnemies();

        // FullyEndGame();

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
        if (!killCameFromBlackHole)
            score += 10;
        else
            score += 15 * powerUpSelector.GetComponent<PowerUpSelector>().totalPowerUpsAcquired;

        scoreUI.text = score.ToString();        
    }
    #endregion

}
