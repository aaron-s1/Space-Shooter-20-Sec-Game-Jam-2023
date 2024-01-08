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

    [HideInInspector] public bool alreadyHit;
    [HideInInspector] public bool canBeSeenByBlackHole;

    bool canAddScoreFurther = true;


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


    void OnDisable() =>
        ResetVariablesToDefaults();


    void ResetVariablesToDefaults()
    {
        alreadyHit = false;
        gameObject.layer = 6;
        renderer.enabled = true;
        canAddScoreFurther = true;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        // If seen by Black Hole's pull trigger, stop logic/movement.
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


    #region Handle death states.
    public IEnumerator StartDying(int explosionChains = 0, bool dontExplode = false)
    {
        alreadyHit = true;

        if (gameManager.gameHasEnded)
        {
            StopAllCoroutines();
            yield break;
        }

        CancelInvoke("DisableAfterSeconds");

        EnemySpawner.Instance.spawnRateScalar *= multiplicativeSpawnRateAdjustment;

        ParticleSystem activeParticle;


        if (explosionChains > 0)
            activeParticle = explosionParticle;
        else
            activeParticle = poofParticle;

        StartCoroutine(HandleDeathFlagsThenDie(activeParticle));
    }

    
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
        var particlePersistLength_TENTATIVE = 0.45f/2;
        yield return new WaitForSeconds(particlePersistLength_TENTATIVE);

        gameObject.SetActive(false);
    }

    #endregion



    // Don't let an enemy persist for too long (if not interacted with).
    void DisableAfterSeconds() =>
        gameObject.SetActive(false);


    // void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.red;
    //     Gizmos.DrawWireSphere(transform.localPosition, 1f);
    // }        
}