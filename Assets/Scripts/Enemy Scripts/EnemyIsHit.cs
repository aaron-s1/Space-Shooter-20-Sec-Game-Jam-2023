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
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BlackHole")
            CancelInvoke("DisableAfterSeconds");
    }    

    void OnDisable() =>
        ResetVariablesToDefaults();


    void ResetVariablesToDefaults()
    {
        alreadyHit = canExplodeOtherEnemies = false;
        renderer.enabled = true;
        canAddScoreFurther = true;
                
        totalExplosionChains = 0;
    }


    #region Handle death states.
    public IEnumerator StartDying(int totalExplosionChains = 0, bool dontExplode = false)
    {
        alreadyHit = true;
        CancelInvoke("DisableAfterSeconds");

        if (gameManager.gameHasEnded)
        {
            StopAllCoroutines();
            yield break;
        }

        EnemySpawner.Instance.spawnRateScalar *= multiplicativeSpawnRateAdjustment;

        this.totalExplosionChains = totalExplosionChains;

        ParticleSystem activeParticle;

        if (this.totalExplosionChains > 0)
        {
            StartCoroutine(soundManager.PlayExplosionClip());
            canExplodeOtherEnemies = true;
            activeParticle = explosionParticle;
        }

        else
            activeParticle = poofParticle;

        yield return HandleDeathFlagsThenDie(activeParticle);
    }


    void OnTriggerStay2D(Collider2D other)
    {
        // Explode other enemies.
        if (other.gameObject.tag == "Enemy" && canExplodeOtherEnemies)
        {
            EnemyIsHit otherEnemy = other.gameObject.GetComponent<EnemyIsHit>();

            // testing limiting explosions
            // canExplodeOtherEnemies = false;
            
            if (!otherEnemy.alreadyHit)
                StartCoroutine(otherEnemy.StartDying(totalExplosionChains - 1));
        }

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
        var timeForParticleToPersist = 0.45f;

        // Other enemies can now only be exploded at the moment an exploding enemy dies.
        yield return new WaitForEndOfFrame();
        canExplodeOtherEnemies = false;
        if (totalExplosionChains > 0)
            totalExplosionChains--;

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