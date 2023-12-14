using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public float spawnRateScale = 1f;

    [System.Serializable]
    public class WaveParameters
    {
        public Transform spawnTransform;
        public SpeedRange speedRange;
        // public float movementSpeed;
        public float verticalOffset;
    }

    [SerializeField] int totalPoolSize;
    [SerializeField] GameObject enemyPrefabVariant1;
    [SerializeField] GameObject enemyPrefabVariant2;
    [SerializeField] float timeBetweenWaves;

    [Space(10)]
    [SerializeField] WaveParameters[] waveParameters;

    [SerializeField] SpeedRange enemySpeedRange;

    BulletPool bulletPool;


    void Awake() =>
        Instance = this;


    void Start()
    {
        InitializeBulletPool();
        // StartCoroutine(SpawnWaves());
    }

    void InitializeBulletPool() =>
        bulletPool = new BulletPool(enemyPrefabVariant1, totalPoolSize);


    public IEnumerator SpawnWaves()
    {
        // Debug.Log("spawn waves called");
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves * spawnRateScale);

            WaveParameters wave1 = waveParameters[Random.Range(0, waveParameters.Length)];
            WaveParameters wave2 = waveParameters[Random.Range(0, waveParameters.Length)];

            wave2.verticalOffset *= -1;

            SpawnEnemyWave(wave1, enemyPrefabVariant1);
            SpawnEnemyWave(wave2, enemyPrefabVariant2);

            yield return null;
        }
    }


    void SpawnEnemyWave(WaveParameters wave, GameObject enemyPrefab)
    {
        int directionMultiplier = (wave == waveParameters[0]) ? 1 : -1;

        float xOffset = directionMultiplier;
        float xPos = wave.spawnTransform.position.x + xOffset;

        GameObject enemy = bulletPool.GetEnemy();
        enemy.transform.position = new Vector3(xPos, wave.spawnTransform.position.y + wave.verticalOffset, wave.spawnTransform.position.z);

        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();

        if (enemyMove != null)
            enemyMove.SetMovementParameters(wave.speedRange.GetRandomSpeed() * directionMultiplier);
            // enemyMove.SetMovementParameters(wave.movementSpeed * directionMultiplier);
    }
}



    public class BulletPool
    {
        GameObject prefab;
        int poolSize;
        List<GameObject> activeEnemyList;
        List<GameObject> inactiveEnemyList;

        public BulletPool(GameObject prefab, int poolSize)
        {
            this.prefab = prefab;
            this.poolSize = poolSize;
            InitializePool();
        }

        void InitializePool()
        {
            activeEnemyList = new List<GameObject>();
            inactiveEnemyList = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Object.Instantiate(prefab);
                enemy.SetActive(false);
                inactiveEnemyList.Add(enemy);
            }
        }

        public GameObject GetEnemy()
        {
            GameObject enemy;

            if (inactiveEnemyList.Count > 0)
            {
                enemy = inactiveEnemyList[inactiveEnemyList.Count - 1];
                inactiveEnemyList.RemoveAt(inactiveEnemyList.Count - 1);
            }
            else
                enemy = Object.Instantiate(prefab);

            enemy.SetActive(true);
            activeEnemyList.Add(enemy);

            return enemy;
        }

        public void ReturnEnemy(GameObject enemy)
        {
            if (activeEnemyList.Contains(enemy))
            {
                activeEnemyList.Remove(enemy);
                enemy.SetActive(false);
                inactiveEnemyList.Add(enemy);
            }
        }
    }


    [System.Serializable]
    public class SpeedRange
    {
        [Range(1f, 10f)]
        public float minSpeed;

        [Range(1f, 10f)]
        public float maxSpeed;

        public float GetRandomSpeed() =>
            Random.Range(minSpeed, maxSpeed);
    }
