using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileMovesForward : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D rb;

    void Start() =>
        rb = GetComponent<Rigidbody2D>();
    
    
    void FixedUpdate() =>
        rb.MovePosition(rb.position + (Vector2)transform.up * moveSpeed * Time.deltaTime);
}
