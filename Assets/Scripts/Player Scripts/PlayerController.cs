using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [SerializeField] GameObject shockwavePrefab;
    [SerializeField] GameObject blackHolePrefab;
    

    public bool canBecomeBlackHole;



    void Awake()
    {
        Instance = this;

        GetComponent<SpriteRenderer>().enabled = true;
        
        foreach (Transform child in transform)
        {
            SpriteRenderer sprite = child.gameObject.GetComponent<SpriteRenderer>();
            if (sprite != null)
                sprite.enabled = true;
        }

        gameObject.SetActive(false);
    }

    // void Start() => StartCoroutine(Die());


    public IEnumerator Die()
    {
        GetComponent<FireMissile>().StopFiring();

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        
        gameObject.GetComponent<SpriteRenderer>().enabled = false;


        // handle particles.
        Quaternion flippedRotation = Quaternion.Euler(0f, 0f, 90f);

        ParticleSystem shockWave1 = Instantiate(shockwavePrefab, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem shockWave2 = Instantiate(shockwavePrefab, transform.position, flippedRotation).GetComponent<ParticleSystem>();
        shockWave1.Play();
        shockWave2.Play();

        yield return new WaitForSeconds(shockWave1.main.duration);
        // Debug.Log($"Die() waited for shockwave for {shockWave1.main.duration}");


        if (canBecomeBlackHole)
        {
            GameObject blackHole = Instantiate(blackHolePrefab, transform.position, Quaternion.identity);

            ParticleSystem blackHoleParticle = blackHolePrefab.GetComponent<ParticleSystem>();
            blackHoleParticle.Play();
            
            yield return new WaitForSeconds(blackHoleParticle.main.duration);
            // Debug.Log($"Die() waited for black hole for {blackHoleParticle.main.duration}");

            // engage pull trigger 
            blackHole.transform.GetChild(3).gameObject.SetActive(true);
            Debug.Log("black hole ended");
        }

        yield return new WaitForSeconds(5f);

        Debug.Log("player fully died.");
        yield break;
    }

    // void Start() =>
        // Invoke("TestMultiplier", 3f);


    // void TestMultiplier() =>
        // FireMissile.Instance.fireRateMultiplier = 10;
}
