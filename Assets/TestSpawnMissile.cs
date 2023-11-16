using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnMissile : MonoBehaviour
{
    [SerializeField] Transform originPoint;
    [SerializeField] GameObject missile;

    void Start() =>
        InvokeRepeating("Spawn", 2f, 1f);

    void Spawn() =>
        Instantiate(missile, originPoint.transform.position, originPoint.transform.rotation);
}
