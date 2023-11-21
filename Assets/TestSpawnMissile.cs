using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpawnMissile : MonoBehaviour
{


    public static TestSpawnMissile Instance { get; private set; }
    
    [SerializeField] GameObject missilePrefab;
    [SerializeField] float timeBetweenMissileFirings = 1f;
    [SerializeField] int poolSize = 300;
    [Space(10)]
    [SerializeField] Transform leftMissileOriginPoint;
    [SerializeField] Transform rightMissileOriginPoint;

    List<GameObject> missilePool;

    private int _missileFireMultiplier = 1;


    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        missilePool = new List<GameObject>();
        InitializeMissilePool();

        InvokeRepeating("PlayerStartsFiring", 2f, timeBetweenMissileFirings / fireRateMultiplier);

        Invoke("NewMissilesNowPierce", 4f);
        // ScaleUpTurrets(3f);
        // Invoke("UpgradeFireRate", 5f);
    }


    public int fireRateMultiplier
    {
        get { return _missileFireMultiplier; }

        set
        {
            if (_missileFireMultiplier != value)
            {
                _missileFireMultiplier = value;
                OnMissileFireMultiplierChanged();
            }
        }
    }
    



    // when calling this, cancel invoke on it first.
    // public void PlayerStartsFiring(int fireMultiplier = 1)
    // {

    //     // CancelInvoke("PlayerStartsFiring");
    //     // Debug.Log("tomato");
    // }

    void InitializeMissilePool()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject missile = Instantiate(missilePrefab, Vector3.zero, Quaternion.identity);
            missile.SetActive(false);
            missilePool.Add(missile);
        }
    }

    public void PlayerStartsFiring()
    {
        // Get missiles from the pool for both turrets
        GameObject missileLeft = GetPooledMissile();
        // GameObject missileRight = GetPooledMissile();

        // if (missileLeft != null && missileRight != null)
        // {
        //     // Set missile positions and rotations
        //     missileLeft.transform.position = leftOriginPoint.position;
        //     missileLeft.transform.rotation = leftOriginPoint.rotation;

        //     missileRight.transform.position = rightOriginPoint.position;
        //     missileRight.transform.rotation = rightOriginPoint.rotation;

        //     // Activate the missiles
        //     missileLeft.SetActive(true);
        //     missileRight.SetActive(true);
        //     Debug.Log(missileLeft);
        //     Debug.Log(missileRight);
        // }

        if (missileLeft != null)
        {
            missileLeft.transform.position = leftMissileOriginPoint.position;
            missileLeft.transform.rotation = leftMissileOriginPoint.rotation;
            missileLeft.SetActive(true);
        }

        GameObject missileRight = GetPooledMissile();

        if (missileRight != null)
        {
            missileRight.transform.position = rightMissileOriginPoint.position;
            missileRight.transform.rotation = rightMissileOriginPoint.rotation;
            missileRight.SetActive(true);
        }
    }

    GameObject GetPooledMissile()
    {
        // Find the first inactive missile in the pool
        return missilePool.Find(missile => !missile.activeInHierarchy);
    }


    void OnMissileFireMultiplierChanged()
    {
        ScaleUpTurrets(fireRateMultiplier);
        CancelInvoke("PlayerStartsFiring");
        InvokeRepeating("PlayerStartsFiring", 0, timeBetweenMissileFirings / fireRateMultiplier);
    }



    // add an animation later?
    public void ScaleUpTurrets(float fireRateMultiplier)
    {
        // Prevent model from getting too large. 
        // 2 = scaleMultiplier of 1.5f. 3 or greater is 1.75f.
        float scaleMultiplier = 1.25f;
        scaleMultiplier += (0.25f * (fireRateMultiplier - 1));
        scaleMultiplier = Mathf.Min(scaleMultiplier, 1.75f);


        var leftTurret = leftMissileOriginPoint.transform.parent.gameObject.transform;
        var rightTurret = rightMissileOriginPoint.transform.parent.gameObject.transform;

        leftTurret.localScale *= scaleMultiplier;
        rightTurret.localScale *= scaleMultiplier;
    }

    public void NewMissilesNowPierce()
    {
        foreach (GameObject bullet in missilePool)
            bullet.GetComponent<MissileHitEnemy>().StartPiercingWhenEnabledAgain();
            // .StartCoroutine("StartPiercingUponEnable()");
    }
}
