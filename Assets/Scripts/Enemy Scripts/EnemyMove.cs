using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    float moveSpeed;
    public float _sineAmplitudeMultiplier;
    public float upwardMultiplier; // New variable for upward movement

    float initialY;
    float randomY;

    bool randomNewYMultiplierFound;
    float randomNewYMultiplier = 1f;

    Transform newTransform;


    void Start() 
    {
        newTransform = transform;
        initialY = transform.position.y;        
    }


    // void OnEnable() =>
        // SetMovementParameters(3f);

    // This is possible the FPS issue?
    void FixedUpdate() =>
        Move();



    public void SetMovementParameters(float speedMultiplier) =>
        moveSpeed = speedMultiplier;

        

    void Move()
    {
        // return;
        // float amplitude = _sineAmplitudeMultiplier * moveSpeed;
        // float newY = initialY + Mathf.Sin(Time.time * 0);

        float newY = initialY;// + Mathf.Sin(Time.time * 0);

        if (!randomNewYMultiplierFound)
        {
            randomNewYMultiplierFound = true;
            randomNewYMultiplier = Random.Range(50f, 100f) * 0.01f;
        }

//
        newY *= randomNewYMultiplier;

        
        // float upwardMovement = upwardMultiplier * moveSpeed;

        
        // Debug.Log(upwardMovement);
        // Debug.Log(Vector3.up * upwardMovement * Time.deltaTime);


        // Vector3 newPosition = transform.position +
        // Vector3 newPosition = newTransform.position + Vector3.right * moveSpeed * Time.deltaTime;// +
            // Vector3.up * upwardMovement * Time.deltaTime;

        // Set the new position



        
        // newTransform.position += Vector3.right * moveSpeed * Time.deltaTime;
        // newTransform.position = new Vector3(newTransform.position.x, newY, newTransform.position.z);

        newTransform.position = new Vector3(newTransform.position.x + moveSpeed * Time.deltaTime, newY, newTransform.position.z);

        // Vector3 newMoveLeft = Vector3.right * moveSpeed * Time.deltaTime;



        // transform.position = new Vector3(newPosition.x, newY, newPosition.z);
        // newPosition;
        // transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
