using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedPowerUpMovesToPlayer : MonoBehaviour
{
    Vector3 playerPos;

    // public bool moveToPlayer;
    [SerializeField] float moveDuration;

    void Start()
    {
        // playerPos = PlayerController.Instance.gameObject.transform.position;
    }

    public IEnumerator Move(Vector3 detachPoint, GameObject powerUpList)
    {
        playerPos = PlayerController.Instance.gameObject.transform.position;
        // var detachPointAsWorld = transform.TransformPoint(detachPoint);
        // playerPos = transform.TransformPoint(playerPos);

        gameObject.transform.SetParent(null);
        float timeElapsed = 0f;
        Debug.Break();
        // Vector3 startPos = dronePos;
        //transform.position;

        while (timeElapsed < moveDuration)
        {
            // transform.position = Vector3.Lerp(transform.position, playerPos, timeElapsed / moveDuration);
            transform.position = Vector3.Lerp(detachPoint, playerPos, timeElapsed / moveDuration);
            yield return null;

            timeElapsed += Time.deltaTime;
        }

        transform.position = playerPos;
        gameObject.transform.SetParent(powerUpList.transform);

        Debug.Log("power up reached player");
        gameObject.SetActive(false);
    }
}
