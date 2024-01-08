using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Transform newTransform;

    float moveSpeed;
    float initialY;

    float randomNewYMultiplier = 1f;
    bool randomNewYMultiplierFound;


    void Start() 
    {
        newTransform = transform;
        initialY = transform.position.y;        
    }


    void FixedUpdate() =>
        Move();


    void Move()
    {
        float newY = initialY;

        if (!randomNewYMultiplierFound)
        {
            randomNewYMultiplierFound = true;
            randomNewYMultiplier = Random.Range(50f, 100f) * 0.01f;
        }

        newY *= randomNewYMultiplier;
        newTransform.position = new Vector3(newTransform.position.x + moveSpeed * Time.deltaTime, newY, newTransform.position.z);

    }

    public void SetMovementParameters(float speedMultiplier) =>
        moveSpeed = speedMultiplier;
}
