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
    int killsAccrualValue;

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
        if (killsAccrualValue != missileKills)
        {
            killsAccrualValue += killsAccrualPerFrame;
            missileKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();

            if (killsAccrualValue >= missileKills)
            {
                missileKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();
                killsAccrualValue = 0;
                accruingMissileScore = false;
            }
        }

        else if (accruingBlackHoleScore)
        {
            if (blackHoleKills == 0)
            {
                blackHoleKillsTextObject.GetComponent<TextMeshProUGUI>().text = "0";
                killsAccrualValue = 0;
                accruingBlackHoleScore = false;
                return;
            }

            if (killsAccrualValue != blackHoleKills)
            {
                killsAccrualValue += killsAccrualPerFrame;
                blackHoleKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();

                if (killsAccrualValue >= blackHoleKills)
                {
                    blackHoleKillsTextObject.GetComponent<TextMeshProUGUI>().text = killsAccrualValue.ToString();
                    killsAccrualValue = 0;
                    accruingBlackHoleScore = false;
                }
            }
        }
    }



    public void TallyUpKillsAndScore(int regularKills, int blackHoleKills)
    {
        this.missileKills = regularKills;
        this.blackHoleKills = blackHoleKills;

        totalScoreValue = (missileKills * 10) + (blackHoleKills * 10);

        StartCoroutine(ShowKillsOverTime());
    }


    IEnumerator CountUpKills(GameObject killsTitleObject, GameObject killsTextObject, bool isAccruing)
    {
        yield return new WaitForSeconds(1f);
        killsTitleObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        killsTextObject.SetActive(true);

        killsAccrualValue = 0;
        isAccruing = true;

        yield return new WaitUntil(() => !isAccruing);
        
    }

    // IEnumerator CountUpMissileKills()
    // {
    //     yield return new WaitForSeconds(1f);
    //     missileKillsTitleObject.SetActive(true);        
    //     yield return new WaitForSeconds(1f);
    //     missileKillsTextObject.SetActive(true);
    //     killsAccrualValue = 0;
    //     accruingMissileScore = true;

    //     yield return new WaitForEndOfFrame();
    //     yield return new WaitForEndOfFrame();
    // }

    // IEnumerator CountUpBlackHoleKills()
    // {
    //     yield return new WaitForSeconds(1f);
    //     blackHoleKillsTitleObject.SetActive(true);
    //     yield return new WaitForSeconds(1f);
    //     blackHoleKillsTextObject.SetActive(true);

    //     killsAccrualValue = 0;
    //     accruingBlackHoleScore = true;

    //     yield return new WaitForEndOfFrame();
    //     yield return new WaitForEndOfFrame();
    // }

    IEnumerator CountUpTotalScore()
    {
        yield return new WaitForSeconds(1f);
        totalScoreTitleObject.SetActive(true);
        totalScoreTextObject.SetActive(true);
        totalScoreTextObject.GetComponent<TextMeshProUGUI>().text = totalScoreValue.ToString();        
    }
    

    IEnumerator ShowKillsOverTime()
    {
        Debug.Break();
        // fadeScreen.SetActive(true);

        yield return StartCoroutine(CountUpKills(missileKillsTitleObject, missileKillsTextObject, accruingMissileScore));
        yield return StartCoroutine(CountUpKills(blackHoleKillsTitleObject, blackHoleKillsTextObject, accruingBlackHoleScore));
        // yield return new WaitUntil(() => accruingMissileScore == false);

        // yield return new WaitUntil(() => accruingBlackHoleScore == false);        

        yield return StartCoroutine(CountUpTotalScore());

        yield return new WaitForSeconds(1f);
        restartButtons.SetActive(true);

        yield break;
    }
}
