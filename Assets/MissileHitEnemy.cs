using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissileHitEnemy : MonoBehaviour
{
    GameManager gameManager;

    bool bulletCanPierce;
    bool startPiercingWhenEnabledAgain;
        
    bool canExplodeEnemies = true; //
    bool canExplodeEnemiesWhenEnabledAgain;

    // increment 
    int totalExplosionChains = 0;


    bool canAddScore = true;



    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void OnEnable()
    {
        Invoke("DisableAfterSeconds", 3f);

        if (startPiercingWhenEnabledAgain)
            bulletCanPierce = true;
        if (canExplodeEnemiesWhenEnabledAgain)
            canExplodeEnemies = true;
    }
    

    // void OnDisable() {
        // Debug.Log("missile disabled itself");
    // }


    void OnTriggerEnter2D(Collider2D enemy) {
        if (enemy.gameObject.tag == "Enemy")
        {
            CancelInvoke("DisableAfterSeconds");

            EnemyIsHit enemyIsHit = enemy.gameObject.GetComponent<EnemyIsHit>();

            if (!enemyIsHit.alreadyHit)
            {
                Debug.Log("missile blew up enemy. should only happen once.");
                IEnumerator startDying = enemyIsHit.StartDying(totalExplosionChains);
                StartCoroutine(startDying);
            }

            if (bulletCanPierce)
                Invoke("DisableAfterSeconds", 20f);
            else
                gameObject.SetActive(false);
        }
    }


    void DisableAfterSeconds() 
    {
        gameObject.SetActive(false);
        Debug.Log("DisableAfterSeconds() disabled missile");
    }


    

    public void StartPiercingWhenEnabledAgain() =>
        startPiercingWhenEnabledAgain = true;

    public void StartExplodingEnemiesWhenEnabledAgain()
    {
        totalExplosionChains++;
        canExplodeEnemiesWhenEnabledAgain = true;
    }
}
