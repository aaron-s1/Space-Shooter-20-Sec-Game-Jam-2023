using System.Net;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireMissile : MonoBehaviour
{
    public static FireMissile Instance { get; private set; }
    
    [SerializeField] GameObject missilePrefab;
    // [SerializeField] float timeBetweenMissileFirings = 1f;
    [SerializeField] int poolSize = 150;
    [Space(10)]
    [SerializeField] Transform leftMissileOriginPoint;
    [SerializeField] Transform rightMissileOriginPoint;

    [SerializeField] AudioSource firingSound;

    List<GameObject> missilePool;
    Transform missilePoolParent;


    int _missileFireMultiplier = 1;
    [SerializeField] float _finalFireRateDivisor = 1f;
    int _explosionChains = 0;
    



    void Awake() =>
        Instance = this;


    
    void Start()
    {
        firingSound = GetComponent<AudioSource>();
        missilePool = new List<GameObject>();
        InitializeMissilePool();

        // InvokeRepeating("PlayerStartsFiring", 2f, 1 / _missileFireMultiplier);
    }


    public int fireRateMultiplier
    {
        get { return _missileFireMultiplier; }

        set
        {
            if (_missileFireMultiplier != value)
            {
                _missileFireMultiplier = value;
                OnMissileFireRateMultiplierChanged(_missileFireMultiplier);
            }
        }
    }

    public int explosionChains
    {
        get { return _explosionChains; }

        set
        {
            if (_explosionChains != value)
            {
                _explosionChains = value;
                OnExplodeChainIncrementation();
            }
        }
    }    



    void InitializeMissilePool()
    {
        missilePoolParent = new GameObject("Missile Pool").transform;
        for (int i = 0; i < poolSize; i++)
        {
            GameObject missile = Instantiate(missilePrefab, Vector3.zero, Quaternion.identity, missilePoolParent);
            missile.SetActive(false);
            missilePool.Add(missile);
        }
    }


    
    public void PrepFiringThenFire() =>
        InvokeRepeating("PlayerStartsFiring", 0, (1 / (_missileFireMultiplier / _finalFireRateDivisor)));


    public void PlayerStartsFiring()
    {
        GameObject missileLeft = GetPooledMissile();

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

        // if (!firingSound.isPlaying)
            firingSound.Play();
    }

    GameObject GetPooledMissile() =>
        missilePool.Find(missile => !missile.activeInHierarchy);


    void OnMissileFireRateMultiplierChanged(int newMultiplier)
    {
        if (newMultiplier != 0)
        {
            ScaleUpTurrets(newMultiplier);
            CancelInvoke("PlayerStartsFiring");
            InvokeRepeating("PlayerStartsFiring", 0, 1 / (fireRateMultiplier / _finalFireRateDivisor));
        }
    }

    void OnExplodeChainIncrementation()
    {
        NewMissilesExplodeMoreEnemies();
    }



    
    public void ScaleUpTurrets(int multiplier)
    {
        if (multiplier > 3)
            return;

        Animator leftTurretAnim = PlayerController.Instance.gameObject.transform.GetChild(0).gameObject.GetComponent<Animator>();
        Animator rightTurretAnim = PlayerController.Instance.gameObject.transform.GetChild(1).gameObject.GetComponent<Animator>();

        if (multiplier == 2)
        {
            leftTurretAnim.SetTrigger("level2");
            rightTurretAnim.SetTrigger("level2");
        }
        if (multiplier == 3)
        {
            leftTurretAnim.SetTrigger("level3");
            rightTurretAnim.SetTrigger("level3");
        }
    }


    public void NewMissilesNowPierce()
    {
        foreach (GameObject bullet in missilePool)
            bullet.GetComponent<MissileHitEnemy>().StartPiercingWhenEnabledAgain();
    }

    public void NewMissilesExplodeMoreEnemies()
    {
        foreach (GameObject bullet in missilePool)
            bullet.GetComponent<MissileHitEnemy>().IncrementChainExplosionsWhenEnabledAgain(_explosionChains);
    }

    public void StopFiring()
    {
        _missileFireMultiplier = 0;
        CancelInvoke("PlayerStartsFiring");
        StopAllCoroutines();
    }
}
