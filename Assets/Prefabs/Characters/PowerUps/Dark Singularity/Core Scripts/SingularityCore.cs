using System.Collections;
using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class SingularityCore : MonoBehaviour
{
    //This script is responsible for what happens when the pullable objects reach the core
    //by default, the game objects are simply turned off
    //as this is much more performant than destroying the objects

    GameManager gameManager;
    [SerializeField] GameObject singularity;

    int enemiesAvailable;
    int enemiesEaten;


    void Awake()
    {
        if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = true;        
    }

    void Start() 
    {
        gameManager = GameManager.Instance;
        StartCoroutine(WaitUntilBlackHoleIsActive());
    }

    IEnumerator WaitUntilBlackHoleIsActive()
    {
        yield return new WaitUntil(() => singularity.activeInHierarchy == true);
        yield return new WaitForSeconds(0.7f);
        enemiesAvailable = singularity.GetComponentInChildren<Singularity>().totalEnemiesSeen;

        canStartEating = true;
        yield break;
    }

    bool canStartEating;
    

    void OnTriggerEnter2D (Collider2D other)
    {
        if (!canStartEating)
            return;
            
        // if (other.gameObject.GetComponent<SingularityPullable>().pullable)
        // {
            if (other.gameObject.tag == "Enemy")
            {                
                enemiesEaten++;
                gameManager.AddToKills(true);
            }
        // }

        other.gameObject.SetActive(false);

        if (other.gameObject.tag == "PowerUp")
            Debug.Log("core ate powerup");

        if (enemiesEaten == enemiesAvailable)
            gameManager.blackHoleAteAllEnemies = true;
    }
}
