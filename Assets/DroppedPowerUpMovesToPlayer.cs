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
        playerPos = PlayerController.Instance.gameObject.transform.position;
    }

    public IEnumerator Move(Vector3 dronePos, GameObject powerUpList)
    {
        gameObject.transform.SetParent(null);
        float timeElapsed = 0f;
        Debug.Break();
        // Vector3 startPos = dronePos;
        //transform.position;

        while (timeElapsed < moveDuration)
        {
            transform.position = Vector3.Lerp(transform.position, playerPos, timeElapsed / moveDuration);
            yield return null;

            timeElapsed += Time.deltaTime;
        }

        Debug.Log("?????");

        transform.position = playerPos;
        gameObject.transform.SetParent(powerUpList.transform);

        gameObject.SetActive(false);
    }
}
