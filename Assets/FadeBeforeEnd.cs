using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FadeBeforeEnd : MonoBehaviour
{
    public int totalScoreValue;

    [SerializeField] GameObject fadeScreen;
    [SerializeField] GameObject missileKillsTitleObject;
    [SerializeField] GameObject blackHoleKillsTitleObject;
    [SerializeField] GameObject totalScoreTitleObject;

    [SerializeField] GameObject missileKillsTextObject;
    [SerializeField] GameObject blackHoleKillsTextObject;
    [SerializeField] GameObject totalScoreKillsTextObject;

    string missileKillsText;
    string blackHoleKillsText;
    string totalScoreKillsText;

    //hide later.
    int missileKills;    
    int blackHoleKills;

    int missileKillsScore;
    int blackHoleKillsScore;
    int totalScore;

    
    float fadeScreenFillAmount;


    void Awake()
    {

        // fadeScreenFillAmount = fadeScreen.GetComponentInChildren<Image>().fillAmount;
        fadeScreen.SetActive(true);
    }

    public void TallyUpKillsAndScore(int regularKills, int blackHoleKills)
    {
        this.missileKills = regularKills;
        this.blackHoleKills = blackHoleKills;

        missileKillsScore = missileKills * 10;
        blackHoleKillsScore = blackHoleKills * 10;

        totalScoreValue = (missileKillsScore + blackHoleKillsScore) * 10;

        StartCoroutine(ShowScoresOverTime());
    }

    

    IEnumerator ShowScoresOverTime()
    {
        fadeScreen.SetActive(true);

        // currently too lazy to make cleaner 

        // step 1
        yield return new WaitForSeconds(1f);
        missileKillsTitleObject.SetActive(true);        
        yield return new WaitForSeconds(1f);
        missileKillsTextObject.SetActive(true);
        missileKillsText = "0";
        currentAccrualValue = 0;
        accruingMissileScore = true;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => accruingMissileScore = false);


        // step 2
        yield return new WaitForSeconds(1f);
        blackHoleKillsTitleObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackHoleKillsTextObject.SetActive(true);
        missileKillsText = "0";
        currentAccrualValue = 0;
        accruingBlackHoleScore = true;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => accruingBlackHoleScore = false);

        // step 3
        yield return new WaitForSeconds(1f);
        totalScoreTitleObject.SetActive(true);
        totalScoreKillsTextObject.SetActive(true);
        totalScoreKillsText = totalScoreValue.ToString();

    
        Debug.Log("ShowScoresOverTime() ended.");

        yield break;
    }

    int scoreAccrualPerFrame = 100;
    int currentAccrualValue;

    bool accruingMissileScore;
    bool accruingBlackHoleScore;


    // too lazy to clean up right now
    void FixedUpdate()
    {
        if (accruingMissileScore)
        {
            if (currentAccrualValue != missileKillsScore)
            {
                currentAccrualValue += scoreAccrualPerFrame;
                missileKillsText = currentAccrualValue.ToString();

                if (currentAccrualValue >= missileKillsScore)
                {
                    currentAccrualValue = missileKillsScore;
                    currentAccrualValue = 0;
                    accruingMissileScore = false;
                }
            }
        }

        else if (accruingBlackHoleScore && !accruingMissileScore)
        {
            if (currentAccrualValue != blackHoleKillsScore)
            {
                currentAccrualValue += scoreAccrualPerFrame;

                if (currentAccrualValue >= blackHoleKillsScore)
                {
                    currentAccrualValue = blackHoleKillsScore;
                    accruingBlackHoleScore = false;
                    currentAccrualValue = 0;
                }
            }            
        }
    }
}
