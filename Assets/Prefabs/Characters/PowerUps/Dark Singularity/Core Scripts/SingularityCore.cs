using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class SingularityCore : MonoBehaviour
{
    //This script is responsible for what happens when the pullable objects reach the core
    //by default, the game objects are simply turned off
    //as this is much more performant than destroying the objects

    GameManager gameManager;


    void OnTriggerEnter2D (Collider2D other)
    {
        // Debug.Log("black hole saw something");

        if (other.gameObject.GetComponent<SingularityPullable>().pullable)
        {
            // gameManager.AddToKills(true);

            Debug.Log("touched something pullable");

            if (other.gameObject.tag == "Enemy")
                GameManager.Instance.AddToKills(true);
            // if (other.gameObject.tag == "Enemy")
            // {
            // }

            other.gameObject.SetActive(false);
        }
    }

    void Awake()
    {
        if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = true;        
    }

    void Start() =>
        gameManager = GameManager.Instance;        
}
