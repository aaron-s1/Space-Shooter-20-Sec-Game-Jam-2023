using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Direction moveDirection;
    // public float rotationSpeed = 1f;

    Vector3 nextSlerpPosition;


    public enum Direction
    {
        MoveLeft, MoveRight
    }



    void Start() 
    {
        float xDirection;

        if (moveDirection == Direction.MoveLeft)
            xDirection = -2f;
        else
            xDirection = 2f;

        nextSlerpPosition = transform.position + new Vector3(xDirection, 1, 0);
    }


    void Update() =>
        MoveOnSlerpPath();


    void MoveOnSlerpPath()
    {
        //
        Quaternion targetRotation = Quaternion.LookRotation(nextSlerpPosition - transform.position, Vector3.forward);

        // transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        transform.position = Vector3.MoveTowards(transform.position, nextSlerpPosition, moveSpeed * Time.deltaTime);


        if (Vector3.Distance(transform.position, nextSlerpPosition) < 0.05f)
        {
            if (moveDirection == Direction.MoveLeft)
                nextSlerpPosition.x -= 2f;
            else
                nextSlerpPosition.x += 2f;

            // nextSlerpPosition.x -= 2f;

            if (nextSlerpPosition.y == 1)
                nextSlerpPosition.y = 0;
            else
                nextSlerpPosition.y = 1;

            // nextSlerpPosition = transform.position + new Vector3(xDirection, , 0);
        }
    }
}
