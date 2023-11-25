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

    [SerializeField] GameObject blackHolePowerUpParticle;

    
    int secondsPassedSinceGameStart;

    int regularKills;
    int blackHoleKills;
    int totalKills;

    int score;

    bool gameHasStarted = false;
    bool gameHasEnded = false;

    GameObject player;

    // bool enemiesNowExplode;
    

    // public int Score =>
        // score;


    void Awake() =>
        Instance = this;

    void Start()
    {
        // later on, add conditionals before starting
        StartCoroutine("CountdownToEndGame", 1f);
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine("ActivateBlackHole");
        // Invoke("ActivateBlackHole", 6f);
    }
    
    void StartGame()
    {
        gameHasStarted = true;        
    }


    #region Handle game states.
    IEnumerator CountdownToEndGame()
    {
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
        // gameHasEnded = true;
        // do nothing for now
        yield return null;
    }

    #endregion

    #region Power-ups.



    

    public IEnumerator ActivateBlackHole()
    {
        player.GetComponent<TestSpawnMissile>().fireRateMultiplier = 0;
        foreach (Transform child in player.transform)
            child.gameObject.SetActive(false);
        
        GameObject blackHoleInstance = Instantiate(blackHolePowerUpParticle, player.transform);
        // Instantiate(blackHolePowerUpParticle, player.transform);
        // Debug.Log(blackHoleInstance);

        // wait for particles to prepare before pulling
        yield return new WaitForSeconds(1f);
        Debug.Break();
        blackHoleInstance.transform.GetChild(3).gameObject.SetActive(true);
    }


    

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
