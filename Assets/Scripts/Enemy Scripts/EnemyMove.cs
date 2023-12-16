using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float moveSpeed;
    public float _sineAmplitudeMultiplier;
    public float upwardMultiplier; // New variable for upward movement

    float initialY;
    float randomY;

    bool randomNewYMultiplierFound;
    float randomNewYMultiplier = 1f;


    void Start() =>
        initialY = transform.position.y;

    void FixedUpdate() =>
        Move();


    // void OnEnable() =>
        // Debug.Log("I became an active enemy!");


    public void SetMovementParameters(float speedMultiplier) =>
        moveSpeed = speedMultiplier;

        

    void Move()
    {
        float amplitude = _sineAmplitudeMultiplier * moveSpeed;
        float newY = initialY + Mathf.Sin(Time.time * amplitude);

        

        if (!randomNewYMultiplierFound)
        {
            randomNewYMultiplierFound = true;
            randomNewYMultiplier = Random.Range(50f, 100f) * 0.01f;
        }

        newY *= randomNewYMultiplier;

        
        float upwardMovement = upwardMultiplier * moveSpeed;

        
        // Debug.Log(upwardMovement);
        // Debug.Log(Vector3.up * upwardMovement * Time.deltaTime);

        Vector3 newPosition = transform.position +
            Vector3.right * moveSpeed * Time.deltaTime +
            Vector3.up * upwardMovement * Time.deltaTime;

        // Set the new position
        transform.position = newPosition;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
