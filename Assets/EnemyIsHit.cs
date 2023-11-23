using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIsHit : MonoBehaviour
{
    GameManager gameManager;

    bool bulletCanPierce = false;
    bool startPiercingWhenEnabledAgain = false;
        
    [SerializeField] ParticleSystem poofParticle;
    [SerializeField] ParticleSystem explosionParticle;

    SpriteRenderer renderer;

    [HideInInspector] bool canAddScoreFurther = true;

    public bool alreadyHit;
    bool canExplodeEnemies;


    bool explodeWhenHit;


    void Awake()
    {
        explosionParticle = GetComponentInChildren<ParticleSystem>();     
        renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        // Invoke("DisableAfterSeconds", 3f);
    }


    // void OnEnable()
    // {
    //     ResetVariables();
    // }

    void OnDisable()
    {
        ResetVariables();
    }

    void ResetVariables()
    {
        alreadyHit = canExplodeEnemies = false;
        renderer.enabled = true;
        canAddScoreFurther = true;
    }


    public IEnumerator StartDying(bool exploded = false)
    {
        alreadyHit = true;
        canAddScoreFurther = false;
        gameManager.AddToKills();

        canExplodeEnemies = exploded;

        renderer.enabled = false;


        ParticleSystem activeParticle;
        
        if (exploded)
            activeParticle = explosionParticle;
        else
            activeParticle = poofParticle;

        // OnTriggerEnter/Stay checks for explosion hits while this happens.
        Debug.Log("start dying log");
        yield return DisableAfterParticleEnds(activeParticle);
    }


    // if doesn't work, try on stay.
    void OnTriggerStay2D(Collider2D other)
    {
        // !canAddScoreFurther is a quick hack to see if the other enemy has triggered DieByExplosion() already        
        if (other.gameObject.tag == "Enemy" && canExplodeEnemies && !other.gameObject.GetComponent<EnemyIsHit>().alreadyHit)

        // if (other.gameObject.tag == "Enemy" && canExplodeEnemies && alreadyHit)
        {
            Debug.Log("enemy is exploding shit");
            StartCoroutine(other.gameObject.GetComponent<EnemyIsHit>().DieByExplosion());
        }
    }


    // Can only trigger via other enemies.
    // Is another instance.
    IEnumerator DieByExplosion()
    {
        //should prevent missile from hitting exploding enemies multiple times
        alreadyHit = true;

        canAddScoreFurther = false;
        gameManager.AddToKills();
        renderer.enabled = false;

        yield return DisableAfterParticleEnds(poofParticle);
    }
    


    IEnumerator DisableAfterParticleEnds(ParticleSystem particle)
    {
        Debug.Log("DisableAfterParticleEnds");
        particle.Play();
        yield return new WaitUntil(() => !explosionParticle.isPlaying);        
        gameObject.SetActive(false);
    }


    void DisableAfterSeconds() =>
        gameObject.SetActive(false);


    public void StartExplodingWhenHit() =>
        explodeWhenHit = true;
}
