using UnityEngine;
[RequireComponent(typeof(CircleCollider2D))]
public class Singularity : MonoBehaviour
{
    //This is the main script which pulls the objects nearby
    [SerializeField] public float GRAVITY_PULL = 100f;
    public static float m_GravityRadius = 1f;

    void Awake() {
        m_GravityRadius = GetComponent<CircleCollider2D>().radius;

        if (GetComponent<CircleCollider2D>())
            GetComponent<CircleCollider2D>().isTrigger = true;
    }
    
    void OnTriggerStay2D(Collider2D other)
    {
    // void OnTriggerStay2D (Collider2D other) {
        if (other.attachedRigidbody && other.GetComponent<SingularityPullable>())
        {
            var enemy = other.gameObject.GetComponent<EnemyIsHit>();
            if (enemy == null)
                return;

            

            Debug.Log("singularity saw enemy");
            enemy.StopAllCoroutines();
            enemy.alreadyHit = true;
            enemy.CancelInvoke("DisableAfterSeconds");
            enemy.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            // enemy.gameObject.GetComponent<EnemyMove>().SetMovementParameters(0);

            // enemy.gameObject.SetActive(false);
            // GameManager.Instance.AddToKills(true);
            // return;

            float gravityIntensity = Vector2.Distance(transform.position, other.transform.position) / m_GravityRadius;
            other.attachedRigidbody.AddForce((transform.position - other.transform.position) * gravityIntensity * other.attachedRigidbody.mass * GRAVITY_PULL * Time.smoothDeltaTime);
        }
    }
}
