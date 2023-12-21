using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryDisablesObjects : MonoBehaviour
{
    [SerializeField] bool disableMissiles;
    [SerializeField] bool disableEnemy;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (disableMissiles)
        {
            if (other.gameObject.tag == "Missile")
                other.gameObject.SetActive(false);
        }
        
        if (disableEnemy)
        {
            if (other.gameObject.tag == "Enemy")
                other.gameObject.SetActive(false);
        }
    }
}
