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
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        ParticleSystem shockWave = shockwaveParticleToInstantiate.GetComponent<ParticleSystem>();
        Instantiate(shockwaveParticleToInstantiate, transform.position, Quaternion.identity);
        shockWave.Play();

        float startTime = Time.time;

        yield return new WaitWhile(() => shockWave.isPlaying);
        float endTime = Time.time;

float timePassed = endTime - startTime;
Debug.Log(timePassed);
        if (canBecomeBlackHole)
        {
            ParticleSystem blackHole = blackHoleParticleToInstantiate.GetComponent<ParticleSystem>();
            Instantiate(blackHoleParticleToInstantiate, transform.position, Quaternion.identity);
            blackHole.Play();
            yield return new WaitWhile(() => blackHole.isPlaying);
            Debug.Log("hole???");
        }

        Debug.Log("player fully died.");
        yield break;
    }

    // void Start() =>
        // Invoke("TestMultiplier", 3f);


    // void TestMultiplier() =>
        // FireMissile.Instance.fireRateMultiplier = 10;
}
