using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Singularity : MonoBehaviour
{
    [SerializeField] public float GRAVITY_PULL = 100f;
    public static float m_GravityRadius = 1f;
    
    Vector2 gravityCenter;


    void Start()
    {
        m_GravityRadius = GetComponent<CircleCollider2D>().radius;
        gravityCenter = transform.position;

        if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = true;

    }

    public int enemiesSeen;
    public bool allEnemiesSeen;

    void OnTriggerStay2D(Collider2D other)
    {
        if (TryGetComponents(other, out var pullable, out var rb))
        {
            // Debug.Log("Black hole saw something pullable.");

            if (other.gameObject.tag == "Enemy")
            {
                if (other.GetComponent<EnemyIsHit>().canBeSeenByBlackHole)
                {
                    // Debug.Log("Black hole sees enemy!");
                    enemiesSeen++;
                    other.GetComponent<EnemyIsHit>().canBeSeenByBlackHole = false;
                }
            }

            float gravityIntensity = Vector2.Distance(gravityCenter, other.transform.position) / m_GravityRadius;

            rb.AddForce((gravityCenter - (Vector2)other.transform.position) * gravityIntensity * rb.mass * GRAVITY_PULL * Time.smoothDeltaTime);
        }
    }

    bool TryGetComponents(Collider2D collider, out SingularityPullable pullable, out Rigidbody2D rb)
    {
        pullable = collider.GetComponent<SingularityPullable>();
        rb = collider.attachedRigidbody;

        return pullable != null && rb != null;
    }
}
