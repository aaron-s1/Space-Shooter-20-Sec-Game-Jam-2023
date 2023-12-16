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
    

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy")
        {
            GameObject enemy = other.gameObject;
            EnemyIsHit enemyIsHit = enemy.GetComponent<EnemyIsHit>();


            if (enemy.activeInHierarchy && !enemyIsHit.alreadyHit && gameObject.activeInHierarchy)
            {
                Debug.Log("missile hit enemy");
                enemyIsHit.alreadyHit = true;
                CancelInvoke("DisableAfterSeconds");

                IEnumerator startDying = enemyIsHit.StartDying(totalExplosionChains);
                StartCoroutine(startDying);

                if (bulletCanPierce)
                    Invoke("DisableAfterSeconds", 1.5f);
                else
                    // Invoke("DisableAfterSeconds", 10f);
                    gameObject.SetActive(false);
            }
        }
    }


    void DisableAfterSeconds() =>
        gameObject.SetActive(false);


    public void StartPiercingWhenEnabledAgain() =>
        startPiercingWhenEnabledAgain = true;

    public void IncrementChainExplosionsWhenEnabledAgain(int newChainAmount) =>
        canExplodeEnemiesWhenEnabledAgain = true;
}
