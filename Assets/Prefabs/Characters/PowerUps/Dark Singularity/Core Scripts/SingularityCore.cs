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
        // if (other.gameObject.GetComponent<SingularityPullable>().pullable)
        // {
            if (other.gameObject.tag == "Enemy")
            {
                Debug.Log("black hole CORE ate enemy");
                GameManager.Instance.AddToKills(true);
            }

            other.gameObject.SetActive(false);
        // }
    }

    void Awake()
    {
        if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = true;        
    }

    void Start() =>
        gameManager = GameManager.Instance;        
}
