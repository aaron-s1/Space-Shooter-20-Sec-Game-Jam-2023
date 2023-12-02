using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class SingularityCore : MonoBehaviour
{
    //This script is responsible for what happens when the pullable objects reach the core
    //by default, the game objects are simply turned off
    //as this is much more performant than destroying the objects

    GameManager gameManager;


    void OnTriggerStay2D (Collider2D other) {
        if (other.GetComponent<SingularityPullable>())
        {
            if (other.gameObject.tag == "Enemy")
                GameManager.Instance.AddToKills(true);
                
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
