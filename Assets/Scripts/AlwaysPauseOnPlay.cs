using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlwaysPauseOnPlay : MonoBehaviour
{
    [SerializeField] bool pause;
    
    void Awake() 
    {
        if (pause)
            Debug.Break();
    }
}
