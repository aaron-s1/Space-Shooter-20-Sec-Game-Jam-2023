using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using TMPro;

public class FadeBeforeEnd : MonoBehaviour
{
    public int totalScoreValue;

    [SerializeField] GameObject fadeScreen;

    [Space(10)]
    [SerializeField] GameObject missileKillsTitleObject;
    [SerializeField] GameObject missileKillsTextObject;
    string missileKillsText;

    [SerializeField] GameObject blackHoleKillsTitleObject;
    [SerializeField] GameObject blackHoleKillsTextObject;
    string blackHoleKillsText;


    [SerializeField] GameObject totalScoreTitleObject;
    [SerializeField] GameObject totalScoreTextObject;
    string totalScoreText;
    // [SerializeField] GameObject totalScoreKillsTextObject;

    //hide later.
    int missileKills;    
    int blackHoleKills;

    // int missileKillsScore;
    // int blackHoleKillsScore;
    // int totalScore;

    
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

        // missileKillsScore = missileKills * 10;
        // blackHoleKillsScore = blackHoleKills * 50;

        totalScoreValue = (missileKills * 10) + (blackHoleKills * 40);

        StartCoroutine(ShowScoresOverTime());
    }

    

    IEnumerator ShowScoresOverTime()
    {
        Debug.Log("ShowScores called");
        Debug.Break();
        // fadeScreen.SetActive(true);

        // currently too lazy to make cleaner 

        // step 1
        yield return new WaitForSeconds(1f);
        missileKillsTitleObject.SetActive(true);        
        yield return new WaitForSeconds(1f);
        missileKillsTextObject.SetActive(true);

        // missileKillsTextObject.GetComponent<Text>() = "0";
        // missileKillsText = "0";
        killsAccrualValue = 0;
        accruingMissileScore = true;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => accruingMissileScore == false);

        Debug.Log("accruing missile score is now false");

        // step 2
        yield return new WaitForSeconds(1f);
        blackHoleKillsTitleObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackHoleKillsTextObject.SetActive(true);

        // missileKillsText = "0";
        killsAccrualValue = 0;
        accruingBlackHoleScore = true;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        yield return new WaitUntil(() => accruingBlackHoleScore == false);        

        Debug.Log("begin black hole scores");
        // step 3
        yield return new WaitForSeconds(1f);
        totalScoreTitleObject.SetActive(true);
        totalScoreTextObject.SetActive(true);
        totalScoreTextObject.GetComponent<TextMeshProUGUI>().text = totalScoreValue.ToString();
        // totalScoreKillsTextObject.SetActive(true);
        // totalScoreText = totalScoreValue.ToString();

    
        Debug.Log("ShowScoresOverTime() ended.");

        yield break;
    }

    int killsAccrualPerFrame = 5;
    int killsAccrualValue;

    bool accruingMissileScore;
    bool accruingBlackHoleScore;


    void FixedUpdate()
    {
        if (accruingMissileScore)
        {
            if (killsAccrualValue != missileKills)
            {
                killsAccrualValue += killsAccrualPerFrame;
                missileKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();
                // blackHoleKillsText = currentAccrualValue.ToString();

                if (killsAccrualValue >= missileKills)
                {
                    missileKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();
                    killsAccrualValue = 0;
                    accruingMissileScore = false;
                    // currentAccrualValue = blackHoleKillsScore;
                }
            }
        }


        else if (accruingBlackHoleScore && !accruingMissileScore)
        {
            if (killsAccrualValue != blackHoleKills)
            {
                killsAccrualValue += killsAccrualPerFrame;
                blackHoleKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();
                // blackHoleKillsText = currentAccrualValue.ToString();

                if (killsAccrualValue >= blackHoleKills)
                {
                    blackHoleKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();
                    killsAccrualValue = 0;
                    accruingBlackHoleScore = false;
                    // currentAccrualValue = blackHoleKillsScore;
                }
            }
        }
    }
}
