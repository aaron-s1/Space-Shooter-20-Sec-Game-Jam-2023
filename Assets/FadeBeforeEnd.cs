using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeBeforeEnd : MonoBehaviour
{
    [SerializeField] GameObject fadeScreen;
    
    float fadeScreenFillAmount;

    void Awake()
    {        
        fadeScreenFillAmount = fadeScreen.GetComponentInChildren<Image>().fillAmount;
        fadeScreen.SetActive(true);
    }
}
