using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    public float scrollSpeed = 2.0f;
    float initialY;

    void Start() =>
        initialY = transform.position.y;

    void Update()
    {
        if (scrollSpeed > 0)
        {
            float newPosition = Mathf.Repeat(Time.time * scrollSpeed, 5);
            transform.position = new Vector3(transform.position.x, initialY - newPosition, transform.position.z);
        }
    }

    public void StopScrolling() => scrollSpeed = 0;
}
