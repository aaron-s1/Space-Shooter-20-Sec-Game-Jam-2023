using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileHitEnemy : MonoBehaviour
{
    GameManager gameManager;

    bool bulletCanPierce = false;
    bool startPiercingWhenEnabledAgain = false;
        
    ParticleSystem explosionParticle;



    void Awake()
    {
        explosionParticle = GetComponentInChildren<ParticleSystem>();        
    }

    void Start()
    {
        gameManager = GameManager.Instance;
    }

    void OnEnable()
    {
        Invoke("DisableAfterSeconds", 3f);

        if (startPiercingWhenEnabledAgain)
            bulletCanPierce = true;
    }
    

    void OnDisable() {
        Debug.Log("missile disabled itself");
    }


    void OnTriggerEnter2D(Collider2D enemy) {
        if (enemy.gameObject.tag == "Enemy")
        {
            gameManager.AddToKills(true);
            CancelInvoke("DisableAfterSeconds");
            Debug.Log($"{gameObject} hit enemy");


            enemy.gameObject.SetActive(false);
            explosionParticle.Play();


            if (!bulletCanPierce)
            {
                GetComponent<MeshRenderer>().enabled = false;
                StartCoroutine("DisableAfterParticleEnds");
            }
            else
                Invoke("DisableAfterSeconds", 1f);
        }
    }

    IEnumerator DisableAfterParticleEnds()
    {
        yield return new WaitUntil(() => !explosionParticle.isPlaying);
        Debug.Log("DisableAfterParticleEnds() disabled missile");
        gameObject.SetActive(false);
    }


    void DisableAfterSeconds() 
    {
        gameObject.SetActive(false);
        Debug.Log("DisableAfterSeconds() disabled missile");
    }
    


    public void StartPiercingWhenEnabledAgain() =>
        startPiercingWhenEnabledAgain = true;
}
