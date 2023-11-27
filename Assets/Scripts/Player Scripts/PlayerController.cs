using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject blackHole;
    // spawn black hole as needed.


    void Start() =>
        Invoke("TestMultiplier", 3f);


    void TestMultiplier() =>
        FireMissile.Instance.fireRateMultiplier = 10;
}
