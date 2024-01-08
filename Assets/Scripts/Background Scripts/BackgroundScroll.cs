using System.Collections;
using UnityEngine;

public class BackgroundScroll : MonoBehaviour
{
    // For now, gave up trying to get sprite repeating to work. 
    // Just made a bunch of copies of the sprite background. Works fine.


    public float scrollSpeed = 2.0f;
    float originalScrollSpeed;
    // public float repeatThreshold;
    // float initialY;

    // float yRepeatHeight;

    // float repeatHeight;


    void Start()
    {
        // initialY = transform.position.y;
        originalScrollSpeed = scrollSpeed;
        scrollSpeed = 0;
        StopScrolling();

        StartCoroutine(WaitForGameStart());
    }

    IEnumerator WaitForGameStart()
    {
        yield return new WaitUntil (() => GameManager.Instance.gameHasStarted == true);
        scrollSpeed = originalScrollSpeed;
    }


    void Update()
    {
        if (scrollSpeed > 0)
            transform.position -= new Vector3(0f, scrollSpeed * Time.deltaTime, 0f);
    }

    public void StopScrolling() => scrollSpeed = 0;
}
