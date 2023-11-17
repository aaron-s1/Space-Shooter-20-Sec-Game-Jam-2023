using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    void Start() =>
        Invoke("TestMultiplier", 3f);


    void TestMultiplier() =>
        TestSpawnMissile.Instance.fireRateMultiplier = 10;        
}
