using System.Linq;
using System.ComponentModel;
using System.Security.Authentication;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsHit : MonoBehaviour
{
    GameManager gameManager;

    
    [SerializeField] public ParticleSystem poofParticle;
    [SerializeField] public ParticleSystem explosionParticle;
    [SerializeField] [Range(0.98f, 0.999f)] float multiplicativeSpawnRateAdjustment;

    SoundManager soundManager;

    new SpriteRenderer renderer;
    new Rigidbody2D rigidbody;

    [HideInInspector] bool canAddScoreFurther = true;

    [HideInInspector] public bool alreadyHit;
    bool canExplodeOtherEnemies;


    int totalExplosionChains;
    // bool explodeWhenHit;


    void Awake()
    {
        explosionParticle = GetComponentInChildren<ParticleSystem>();     
        renderer = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        soundManager = SoundManager.Instance;
        Invoke("DisableAfterSeconds", 4f);
    }

    // Don't disable if touching black hole. Let black hole disable instead.
    // void OnTriggerEnter(Collider other)
    // {
    //     if (other.gameObject.tag == "BlackHole")
    //         CancelInvoke("DisableAfterSeconds");
    // }    

    void OnDisable() =>
        ResetVariablesToDefaults();


    void ResetVariablesToDefaults()
    {
        alreadyHit = canExplodeOtherEnemies = false;
        gameObject.layer = 6;
        renderer.enabled = true;
        canAddScoreFurther = true;
                
        totalExplosionChains = 0;
    }


    #region Handle death states.
    public IEnumerator StartDying(int explosionChains = 0, bool dontExplode = false)
    {
        
        // Debug.Log("explosion chains = " + explosionChains);
        alreadyHit = true;

        if (gameManager.gameHasEnded)
        {
            StopAllCoroutines();
            yield break;
        }


        CancelInvoke("DisableAfterSeconds");

        EnemySpawner.Instance.spawnRateScalar *= multiplicativeSpawnRateAdjustment;

        ParticleSystem activeParticle;

        // totalExplosionChains = explosionChains;

        // if (explosionChains > 0)
        // if (this.totalExplosionChains > 0)
        // {
        // ExplodeNearbyEnemies(explosionChains);
            // canExplodeOtherEnemies = true;
            // totalExplosionChains--;

        // StartCoroutine(soundManager.PlayExplosionClip());
        if (explosionChains > 0)
            activeParticle = explosionParticle;
        else
            activeParticle = poofParticle;
        // }

        // else
        // {
            // canExplodeOtherEnemies = false;
        // }

        StartCoroutine(HandleDeathFlagsThenDie(activeParticle));
    }

    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.localPosition, 1f);
    // }

    void ExplodeNearbyEnemies(int timesToChain)
    {
        if (timesToChain <= 0)
            return;

        Debug.Log("enemy is exploding");
        StartCoroutine(soundManager.PlayExplosionClip());
        // if (other.gameObject.CompareTag("Enemy") && canExplodeOtherEnemies)
        // {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 1f, 6);

        Debug.Log("total enemies that can be exploded = " + hitEnemies.Length);

        foreach (var hitEnemy in hitEnemies)
        {
            Debug.Log("foreach loop reached in explosion");

            EnemyIsHit otherEnemy = hitEnemy.GetComponent<EnemyIsHit>();

            if (otherEnemy != null && !otherEnemy.alreadyHit)
                StartCoroutine(otherEnemy.StartDying(timesToChain - 1));
        }
        // }
    }


    void OnTriggerEnter2D(Collider2D other)
    // void OnTriggerStay2D(Collider2D other)
    {

// Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, 5f, 6);
// Debug.Log("total enemies that can be exploded = " + hitEnemies.Length);
        // return;
        

        // Explode other enemies.
        // if (other.gameObject.layer == 6 && canExplodeOtherEnemies)
        // return;


        // if (other.gameObject.layer == 6 && canExplodeOtherEnemies)
        // {
        //     Debug.Log("is exploding enemies");

        //     canExplodeOtherEnemies = false;
        //     // totalExplosionChains--;

        //     // return;
        //     EnemyIsHit otherEnemy = other.gameObject.GetComponent<EnemyIsHit>();

        //     if (otherEnemy != null)
        //     {
        //         if (!otherEnemy.alreadyHit)
        //         {
        //             Debug.Log("enemy hit enemy");
        //             // otherEnemy.alreadyHit = true;
        //             // StartCoroutine(otherEnemy.StartDying(totalExplosionChains));
        //         }
        //     }
        // }


        //     try
        //     {
        //         StartCoroutine(other.gameObject.GetComponent<EnemyIsHit>().StartDying(totalExplosionChains - 1));
        //     }
        //     catch (System.Exception)
        //     {
        //         Debug.Log("??");
        //         throw;
        //     }
        // }

        // Stop logic/movement if black hole is active.
        if (other.gameObject.tag == "BlackHole")
        {
            if (rigidbody.gravityScale != 0)
            {
                canBeSeenByBlackHole = true;

                rigidbody.gravityScale = 0;
                alreadyHit = true;
                GetComponent<EnemyMove>().SetMovementParameters(0);
                CancelInvoke("DisableAfterSeconds");
                StopAllCoroutines();
            }
        }
    }

    public bool canBeSeenByBlackHole;// = true;

    
    
    IEnumerator HandleDeathFlagsThenDie(ParticleSystem particle)
    {
        canAddScoreFurther = false;
        gameManager.AddToKills();
        renderer.enabled = false;
        
        yield return StartCoroutine(DisableAfterParticleEnds(particle));
    }


    IEnumerator DisableAfterParticleEnds(ParticleSystem particle)
    {
        particle.Play();
        var timeForParticleToPersist = 0.45f/2;
        // var timeForParticleToPersist = 0.01f;

        // Other enemies can now only be exploded at the moment an exploding enemy dies.
        // yield return new WaitForEndOfFrame();
        // canExplodeOtherEnemies = false;
        // if (totalExplosionChains > 0)
            // totalExplosionChains--;

        yield return new WaitForSeconds(timeForParticleToPersist);
        // yield return new WaitForSeconds(timeForParticleToPersist - timeUntilCanNoLongerExplode);
        gameObject.SetActive(false);
    }

    #endregion



    // Don't let an enemy persist for too long (if not interacted with).
    void DisableAfterSeconds() =>
        gameObject.SetActive(false);
}


// float GetParticleTime(ParticleSystem particle)
// {
//     if (particle == explosionParticle)
//         return 1f;
//     if (particle == poofParticle)
//         return 0.45f;

//     Debug.Log("returned 0???");
//     return 0;
// }