using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissileHitEnemy : MonoBehaviour
{
    GameManager gameManager;

    bool bulletCanPierce;
    bool startPiercingWhenEnabledAgain;
        
    // bool canExplodeEnemies;
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
        {
            canExplodeEnemiesWhenEnabledAgain = false;
            totalExplosionChains++;
        }
    }
    

    void OnDisable() {
        Debug.Log("missile disabled itself");
    }


    void OnTriggerEnter2D(Collider2D enemy) {
        if (enemy.gameObject.tag == "Enemy")
        {
            CancelInvoke("DisableAfterSeconds");

            EnemyIsHit enemyIsHit = enemy.gameObject.GetComponent<EnemyIsHit>();

            if (!enemyIsHit.alreadyHit && gameObject.activeInHierarchy)
            {
                Debug.Log("missile blew up enemy. should only happen once.");
                IEnumerator startDying = enemyIsHit.StartDying(totalExplosionChains);
                StartCoroutine(startDying);
            }

            if (bulletCanPierce || enemyIsHit.alreadyHit)
                Invoke("DisableAfterSeconds", 3f);
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



    // void IncrementChainExplosions() =>
    //     totalExplosionChains++;


    public void IncrementChainExplosionsWhenEnabledAgain(int newChainAmount)
    {
        canExplodeEnemiesWhenEnabledAgain = true;
        // totalExplosionChains = newChainAmount;
        // IncrementChainExplosions();
    }
}
