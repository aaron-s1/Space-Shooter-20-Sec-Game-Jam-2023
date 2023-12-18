using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.UI;
using TMPro;

public class EndGameScreen : MonoBehaviour
{
    public int totalScoreValue;

    [SerializeField] GameObject restartButtons;
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

    int missileKills;    
    int blackHoleKills;

    int killsAccrualPerFrame = 20;
    int killsAmountAccrualPerFrame;

    bool accruingMissileScore;
    bool accruingBlackHoleScore;

    
    float fadeScreenFillAmount;


    void Awake()
    {
        // fadeScreenFillAmount = fadeScreen.GetComponentInChildren<Image>().fillAmount;
        // fadeScreen.SetActive(true);
    }


    void FixedUpdate()
    {
        if (accruingMissileScore)
        {
            accruingBlackHoleScore = false;

            killsAmountAccrualPerFrame += killsAccrualPerFrame;
            missileKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAmountAccrualPerFrame.ToString();

            if (killsAmountAccrualPerFrame >= missileKills)
            {
                missileKillsTextObject.GetComponent<TextMeshProUGUI>().text = missileKills.ToString();
                killsAmountAccrualPerFrame = 0;
                accruingMissileScore = false;
            }
        }

        else if (accruingBlackHoleScore)
        {
            // blackHoleKills = GameManager.Instance.blackHoleKills;

            accruingMissileScore = false;
            killsAmountAccrualPerFrame = 0;

            blackHoleKillsTextObject.GetComponent<TextMeshProUGUI>().text = blackHoleKills.ToString();
            accruingBlackHoleScore = false;
        }
    }



    public void TallyUpKillsAndScore(int regularKills, int blackHoleKills)
    {
        this.missileKills = regularKills;
        this.blackHoleKills = blackHoleKills;

        // totalScoreValue = (missileKills * 10) + (blackHoleKills * 10);

        StartCoroutine(ShowKillsOverTime());
    }


    IEnumerator CountUpKills(GameObject killsTitleObject, GameObject killsTextObject, bool typeIsAccruing)
    {
        yield return new WaitForSeconds(1f);
        killsTitleObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        killsTextObject.SetActive(true);
        missileKills = GameManager.Instance.regularKills;

        killsAmountAccrualPerFrame = 0;

        if (typeIsAccruing == accruingMissileScore)
        {
            Debug.Log("accruing missle score");
            accruingMissileScore = true;
        }
        else if (typeIsAccruing == accruingBlackHoleScore)
            accruingBlackHoleScore = true;

        yield return new WaitUntil(() => !typeIsAccruing);
        // yield return new WaitUntil(() => !accruingBlackHoleScore);      
    }

    IEnumerator CountUpMissileKills()
    {
        yield return new WaitForSeconds(1f);
        missileKillsTitleObject.SetActive(true);        
        yield return new WaitForSeconds(1f);
        missileKillsTextObject.SetActive(true);


        missileKills = GameManager.Instance.regularKills;
        killsAmountAccrualPerFrame = 0;
        accruingMissileScore = true;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }

    IEnumerator CountUpBlackHoleKills()
    {
        yield return new WaitForSeconds(1f);
        blackHoleKillsTitleObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        blackHoleKillsTextObject.SetActive(true);

        blackHoleKills = GameManager.Instance.blackHoleKills;

        killsAmountAccrualPerFrame = 0;
        accruingBlackHoleScore = true;

        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
    }

    IEnumerator CountUpTotalScore()
    {
        var missileScore = missileKills * 10;
        var blackHoleScore = GameManager.Instance.blackHoleKills * 40;
        var totalScore = blackHoleScore + missileScore;

        yield return new WaitForSeconds(1f);        
        totalScoreTitleObject.SetActive(true);
        totalScoreTextObject.SetActive(true);
        totalScoreTextObject.GetComponent<TextMeshProUGUI>().text = totalScore.ToString();        
    }
    

    IEnumerator ShowKillsOverTime()
    {
        // fadeScreen.SetActive(true);

        // accruingMissileScore = true;
        // yield return StartCoroutine(CountUpKills(missileKillsTitleObject, missileKillsTextObject, accruingMissileScore));
        // accruingMissileScore = false;

        // accruingBlackHoleScore = true;
        // yield return StartCoroutine(CountUpKills(blackHoleKillsTitleObject, blackHoleKillsTextObject, accruingBlackHoleScore));
        // accruingBlackHoleScore = false;

        yield return StartCoroutine(CountUpMissileKills());
        yield return new WaitUntil(() => accruingMissileScore == false);
        

        if (PlayerController.Instance.canBecomeBlackHole)
        {
            killsAmountAccrualPerFrame = 2;
            yield return StartCoroutine(CountUpBlackHoleKills());
            yield return new WaitUntil(() => accruingBlackHoleScore == false);
        }

        yield return StartCoroutine(CountUpTotalScore());

        yield return new WaitForSeconds(1f);
        restartButtons.SetActive(true);

        yield break;
    }
}
