using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovesForward : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    
    void FixedUpdate()
    {
        transform.Translate(0, moveSpeed, 0);
    }
}
