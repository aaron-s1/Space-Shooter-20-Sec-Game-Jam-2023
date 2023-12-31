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

    public bool gameHasStarted;


    [HideInInspector] public bool blackHoleAteAllEnemies;

    


    

    // public int Score =>
        // score;


    void Awake() =>
        Instance = this;

    void Start()
    {
        // gameHasStarted = true;
        spawnEnemies = EnemySpawner.Instance.SpawnWaves();
        player = PlayerController.Instance;
    }
    

    #region Handle game states.
    
    public IEnumerator PrepStartOfGame(GameObject howToPlayScreen)
    {
        yield return StartCoroutine(SoundManager.Instance.PlayStartButtonThenStartGame());
        blackBackgroundScreen.SetActive(false);
        howToPlayScreen.SetActive(false);

        gameHasStarted = true;
        
        player.gameObject.SetActive(true);
        StartCoroutine(spawnEnemies);

        yield return StartCoroutine(CountdownBeforeGameStarts());
    }

    public IEnumerator IncreaseVolumeOverTime(float overTimeLength)
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.time = whenToStartAmbienceMusic;

        audioSource.volume = 0.1f;

        GetComponent<AudioSource>().Play();


        // increases to 0.4f
        for (int i = 0; i < 30; i++)
        {
            yield return new WaitForSeconds(overTimeLength / 30);
            audioSource.volume += 0.01f;
        }

        yield break;
    }

    // Give player a few seconds to get ready. Add effects or countdown or something later.    
    IEnumerator CountdownBeforeGameStarts()
    {
        // yield return StartCoroutine(IncreaseVolumeOverTime(1.5f));
        StartCoroutine(IncreaseVolumeOverTime(1.5f));

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
        
        // GameObject.FindObjectOfType<PowerUpSelector>().GetComponent<PowerUpSelector>().StopAllCoroutines();
        GameObject.FindObjectOfType<PowerUpSelector>().StopAllCoroutines();
        // GameObject.FindObjectOfType<BackgroundScroll>().StopScrolling();

        yield return StartCoroutine(player.Die());


        // not dynamic. set to above blackBackgroundScreen's animation length.
        blackBackgroundScreen.SetActive(true);
        blackBackgroundScreen.GetComponent<Animator>().SetTrigger("FadeIn");
        yield return new WaitForSeconds(0.6f);

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


    public void ResetSingleton() => Instance = null;
}
