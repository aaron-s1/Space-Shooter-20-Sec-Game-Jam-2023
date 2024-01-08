// using System.Runtime.Intrinsics.X86;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MissileHitEnemy : MonoBehaviour
{
    GameManager gameManager;

    bool bulletCanPierce;
    bool startPiercingWhenEnabledAgain;        
    bool canExplodeEnemiesWhenEnabledAgain;

    int totalExplosionChains;


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


    void OnTriggerEnter2D(Collider2D enemy)
    {
        if (enemy.gameObject.layer == 6)
        {
            EnemyIsHit enemyIsHit = enemy.gameObject.GetComponent<EnemyIsHit>();
            
            if (enemyIsHit != null && !enemyIsHit.alreadyHit)
            {
                CancelInvoke("DisableAfterSeconds");
                StartCoroutine(HandlePiercingThenDisable());

                StartCoroutine(KillStruckEnemy(enemy, totalExplosionChains));
                ExplodeNearbyEnemies(enemy.gameObject);
            }
        }
    }


    IEnumerator HandlePiercingThenDisable()
    {
        if (bulletCanPierce)
            bulletCanPierce = false;
        else
        {
            yield return new WaitForEndOfFrame();
            gameObject.SetActive(false);
        }
    }


    IEnumerator KillStruckEnemy(Collider2D enemy, int explosionChains = 0)
    {
        enemy.gameObject.layer = 0;

        EnemyIsHit enemyIsHit = enemy.gameObject.GetComponent<EnemyIsHit>();
        StartCoroutine(enemyIsHit.StartDying(explosionChains));

        yield break;
    }


    void ExplodeNearbyEnemies(GameObject enemy)
    {
        if (totalExplosionChains >= 1)
        {
            Collider2D[] inwardsEnemies = Physics2D.OverlapCircleAll(enemy.transform.position, 0.5f, 1 << 6);

            foreach (var inwardsEnemy in inwardsEnemies)
            {
                KillStruckEnemy(inwardsEnemy, totalExplosionChains - 1);

                if (totalExplosionChains == 2)
                {                    
                    Collider2D[] outwardsEnemies = Physics2D.OverlapCircleAll(inwardsEnemy.transform.position, 0.5f, 1 << 6);

                    foreach (var outwardsEnemy in outwardsEnemies)
                        KillStruckEnemy(outwardsEnemy, totalExplosionChains - 2);
                }
            }
        }
    }



    void DisableAfterSeconds() =>
        gameObject.SetActive(false);


    public void StartPiercingWhenEnabledAgain() =>
        startPiercingWhenEnabledAgain = true;

    public void IncrementExplosionsWhenEnabledAgain(int newChainAmount) =>
        canExplodeEnemiesWhenEnabledAgain = true;
}
