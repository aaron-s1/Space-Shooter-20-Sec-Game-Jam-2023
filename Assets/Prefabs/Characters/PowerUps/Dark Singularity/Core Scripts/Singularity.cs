using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Singularity : MonoBehaviour
{
    [SerializeField] public float GRAVITY_PULL = 100f;
    public static float m_GravityRadius = 1f;

    void Start()
    {
        m_GravityRadius = GetComponent<CircleCollider2D>().radius;

        if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = true;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (TryGetComponents(other, out var pullable, out var rb))
        {
            Debug.Log("Components found");
            // var enemy = other.gameObject.GetComponent<EnemyIsHit>();
            var enemy = other.gameObject.GetComponent<SingularityPullable>();
            if (enemy == null)
            {
                Debug.Log("No EnemyIsHit component");
                GameManager.Instance.blackHoleAteAllEnemies = true;
                return;
            }

            Vector2 gravityCenter = transform.position;
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
