using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    GameObject blackHole;


    public bool canBecomeBlackHole;


    void Awake()
    {
        Instance = this;
    }

    void Start() =>
        Invoke("TestMultiplier", 3f);


    void TestMultiplier() =>
        FireMissile.Instance.fireRateMultiplier = 10;
}
