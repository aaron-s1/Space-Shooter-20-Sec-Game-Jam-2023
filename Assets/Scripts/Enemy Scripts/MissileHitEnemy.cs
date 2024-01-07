// using System.Runtime.Intrinsics.X86;
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

    // new Collider2D collider;
    // bool canAddScore = true;


    // void Awake()
    // {
    //     collider = GetComponent<Collider2D>();
    // }

    void Start() 
    {
        gameManager = GameManager.Instance;
    }


    void OnEnable()
    {
        // collider.enabled = true;

        Invoke("DisableAfterSeconds", 3f);

        if (startPiercingWhenEnabledAgain)
            bulletCanPierce = true;

        if (canExplodeEnemiesWhenEnabledAgain)
        {
            canExplodeEnemiesWhenEnabledAgain = false;
            totalExplosionChains++;
        }
    }

    // public Collider2D[] inwardsEnemies;
    // public Collider2D[] outwardsEnemies;
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == 6)
        // if (enemy.activeInHierarchy && !enemyIsHit.alreadyHit && gameObject.activeInHierarchy)
        {
            GameObject enemy = other.gameObject;
            EnemyIsHit enemyIsHit = enemy.GetComponent<EnemyIsHit>();
            if (enemyIsHit == null || enemyIsHit.alreadyHit)
                return;

            CancelInvoke("DisableAfterSeconds");
            StartCoroutine(DisableTest());

            StartCoroutine(KillStruckEnemy(other, totalExplosionChains));
            ExplodeEnemiesAroundStruckEnemy(enemy);

        }
    }

    IEnumerator DisableTest()
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
            // collider.enabled = false;
    }


    void ExplodeEnemiesAroundStruckEnemy(GameObject enemy)
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

    public void IncrementChainExplosionsWhenEnabledAgain(int newChainAmount) =>
        canExplodeEnemiesWhenEnabledAgain = true;
}
