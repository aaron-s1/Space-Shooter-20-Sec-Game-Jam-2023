using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHitEnemy : MonoBehaviour
{
    
    ParticleSystem explosionParticle;

    void Awake()
    {
        explosionParticle = GetComponentInChildren<ParticleSystem>();    
    }

    void OnTriggerEnter2D(Collider2D enemy) {
        if (enemy.gameObject.tag == "Enemy")
        {
            Debug.Log($"{gameObject} hit enemy");
            enemy.gameObject.SetActive(false);
            Debug.Log($"{gameObject} disabled {enemy.gameObject}");
            
            GetComponent<MeshRenderer>().enabled = false;
            explosionParticle.Play();
        }
    }
}
