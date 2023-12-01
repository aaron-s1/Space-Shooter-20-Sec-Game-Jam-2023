using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [SerializeField] GameObject shockwaveParticleToInstantiate;
    [SerializeField] GameObject blackHoleParticleToInstantiate;
    

    public bool canBecomeBlackHole;



    void Awake()
    {
        Instance = this;
    }

    void Start() => StartCoroutine(Die());


    public IEnumerator Die()
    {
        GetComponent<FireMissile>().StopFiring();

        foreach (Transform child in transform)
            child.gameObject.SetActive(false);
        
        gameObject.GetComponent<SpriteRenderer>().enabled = false;


        // handle particles.
        Quaternion flippedRotation = Quaternion.Euler(0f, 0f, 90f);

        ParticleSystem shockWave1 = Instantiate(shockwaveParticleToInstantiate, transform.position, Quaternion.identity).GetComponent<ParticleSystem>();
        ParticleSystem shockWave2 = Instantiate(shockwaveParticleToInstantiate, transform.position, flippedRotation).GetComponent<ParticleSystem>();
        shockWave1.Play();
        shockWave2.Play();

        yield return new WaitForSeconds(shockWave1.main.duration);


        if (canBecomeBlackHole)
        {
            ParticleSystem blackHole = blackHoleParticleToInstantiate.GetComponent<ParticleSystem>();
            Instantiate(blackHoleParticleToInstantiate, transform.position, Quaternion.identity);
            blackHole.Play();
            yield return new WaitForSeconds(blackHole.main.duration);
        }

        Debug.Log("player fully died.");
        yield break;
    }

    // void Start() =>
        // Invoke("TestMultiplier", 3f);


    // void TestMultiplier() =>
        // FireMissile.Instance.fireRateMultiplier = 10;
}
