using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 2.0f;
    float initialY;

    void Start()
    {
        initialY = transform.position.y;
    }

    void Update()
    {
        float newPosition = Mathf.Repeat(Time.time * scrollSpeed, 5);
        transform.position = new Vector3(transform.position.x, initialY - newPosition, transform.position.z);
    }
}
