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
    [SerializeField] [Range(0.994f, 0.999f)] float multiplicativeSpawnRateAdjustment;

    SpriteRenderer renderer;

    [HideInInspector] bool canAddScoreFurther = true;

    public bool alreadyHit;
    bool canExplodeOtherEnemies;


    int totalExplosionChains;
    // bool explodeWhenHit;


    void Awake()
    {
        explosionParticle = GetComponentInChildren<ParticleSystem>();     
        renderer = GetComponent<SpriteRenderer>();
        // Debug.Log("I'm an enemy!");
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        Invoke("DisableAfterSeconds", 5f);
    }


    // void FixedUpdate()
    // {
    //     if (alreadyHit)
    //     {
    //         if (GetComponent<EnemyMove>().moveSpeed != 0)
    //             GetComponent<EnemyMove>().moveSpeed = 0;
    //     }
    // }


    void OnDisable() =>
        ResetVariablesToDefaults();


    void ResetVariablesToDefaults()
    {
        alreadyHit = canExplodeOtherEnemies = false;
        renderer.enabled = true;
        canAddScoreFurther = true;
                
        totalExplosionChains = 0;
    }



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

        // OnTriggerStay2D will try to explode nearby enemies until explosionParticle stops playing.
        if (this.totalExplosionChains > 0)
        {
            canExplodeOtherEnemies = true;
            activeParticle = explosionParticle;
        }

        else
            activeParticle = poofParticle;

        yield return HandleDeathFlagsThenDie(activeParticle);
    }


    void OnTriggerStay2D(Collider2D other)
    {        
        if (other.gameObject.tag == "Enemy" && canExplodeOtherEnemies)
        {
            EnemyIsHit otherEnemy = other.gameObject.GetComponent<EnemyIsHit>();
            
            if (!otherEnemy.alreadyHit)
                StartCoroutine(otherEnemy.StartDying(totalExplosionChains - 1));
        }        
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (other.gameObject.tag == "BlackHole")
    //     {
    //         Debug.Log("enemy saw black hole");
    //         CancelInvoke("DisableAfterSeconds");
    //     }
    // }



    // Can only trigger via another enemy instance.    
    IEnumerator HandleDeathFlagsThenDie(ParticleSystem particle)
    {
        // alreadyHit = true;
        canAddScoreFurther = false;
        gameManager.AddToKills();
        renderer.enabled = false;
        // GetComponent<Collider2D>().enabled = false;
        
        yield return StartCoroutine(DisableAfterParticleEnds(particle));
    }

    // ParticleSystem particleBlah;
    // bool beganDisabling;

    // void FixedUpdate()
    // {
    //     // if (beganDisabling)
    //         // Debug.Log(particleBlah.isPlaying);
    // }
    


    IEnumerator DisableAfterParticleEnds(ParticleSystem particle)
    {
        var timeToWait = GetParticleTime(particle);
        // Debug.Log("called particle end");
        // particleBlah = particle;
        // beganDisabling = true;
        particle.Play();
        // Debug.Log($"began playing {particle}");

        yield return new WaitForSeconds(0.45f);
        // Debug.Log($"should end playing {particle}");        
        // particle.Stop();
        // Debug.Log("hard wait of 0.45f");
        var time1 = Time.timeSinceLevelLoad;

        // yield return new WaitUntil(() => !explosionParticle.isPlaying);
        var time2 = Time.timeSinceLevelLoad;
        // Debug.Log(time2-time1); 
        // Debug.Log($"actually ended playing {particle}");

        
        Debug.Log("disable after particle ends disabled object");
        gameObject.SetActive(false);
        // Invoke("DisableAfterSeconds", 5f);
    }

    float GetParticleTime(ParticleSystem particle)
    {
        if (particle == explosionParticle)
            return 1f;
        if (particle == poofParticle)
            return 0.45f;

        Debug.Log("returned 0???");
        return 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "BlackHole")
        {
            Debug.Log("enemy saw black hole");
            CancelInvoke("DisableAfterSeconds");
        }
    }

    // Don't let an enemy persist for too long (if not interacted with).
    void DisableAfterSeconds()
    {
        Debug.Log("enemy disabled itself.");
        gameObject.SetActive(false);
    }
}
