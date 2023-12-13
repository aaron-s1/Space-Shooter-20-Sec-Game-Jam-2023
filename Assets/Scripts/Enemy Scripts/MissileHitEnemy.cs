using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissileHitEnemy : MonoBehaviour
{
    GameManager gameManager;

    // check what abilities the missile has.
    bool bulletCanPierce;
    bool startPiercingWhenEnabledAgain;        
    bool canExplodeEnemiesWhenEnabledAgain;
    int totalExplosionChains = 0;

    // bool canAddScore = true;


    void Start() =>
        gameManager = GameManager.Instance;


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
    

    void OnTriggerEnter2D(Collider2D enemy) {
        if (enemy.gameObject.tag == "Enemy")
        {
            CancelInvoke("DisableAfterSeconds");

            EnemyIsHit enemyIsHit = enemy.gameObject.GetComponent<EnemyIsHit>();

            if (!enemyIsHit.alreadyHit && gameObject.activeInHierarchy)
            {
                // if missile can pierce, it can't do so again.
                bulletCanPierce = startPiercingWhenEnabledAgain = false;
                Invoke("DisableAfterSeconds", 1.5f);

                enemyIsHit.alreadyHit = true;
                IEnumerator startDying = enemyIsHit.StartDying(totalExplosionChains);
                StartCoroutine(startDying);
            }

            // if (bulletCanPierce)
            // {
            //     bulletCanPierce = false;
            //     Invoke("DisableAfterSeconds", 1.5f);
            // }

            // else
            //     gameObject.SetActive(false);
        }
    }


    void DisableAfterSeconds() =>
        gameObject.SetActive(false);


    public void StartPiercingWhenEnabledAgain() =>
        startPiercingWhenEnabledAgain = true;

    public void IncrementChainExplosionsWhenEnabledAgain(int newChainAmount) =>
        canExplodeEnemiesWhenEnabledAgain = true;
}
