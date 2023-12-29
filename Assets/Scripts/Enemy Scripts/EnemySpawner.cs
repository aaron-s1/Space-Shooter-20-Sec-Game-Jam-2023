using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public float spawnRateScalar = 1f;

    [System.Serializable]
    public class WaveParameters
    {
        public Transform spawnTransform;
        public SpeedRange speedRange;
    }

    [SerializeField] int sizeOfAllPools;
    [SerializeField] GameObject enemyPrefabVariant1;
    [SerializeField] GameObject enemyPrefabVariant2;
    // [SerializeField] GameObject enemyPrefabVariant3;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] int spawnsUntilNextRandom_Y_Pos;

    [Space(10)]
    [SerializeField] WaveParameters[] waveParameters;

    EnemyPool enemyPoolVariant1;
    EnemyPool enemyPoolVariant2;
    // EnemyPool enemyPoolVariant3;

    int spawnsSinceLast_Y_Pos_LastRandomized;
    float ySpawnRangeOffset;

    // List<GameObject> spawnedEnemies = new List<GameObject>();

    void Awake() =>
        Instance = this;

    void Start()
    {
        InitializePools();
        ySpawnRangeOffset = Random.Range(-0.25f, 2.25f);
        // StartCoroutine(SpawnWaves());
    }

    void InitializePools()
    {
        enemyPoolVariant1 = new EnemyPool(enemyPrefabVariant1, sizeOfAllPools / 2);
        enemyPoolVariant2 = new EnemyPool(enemyPrefabVariant2, sizeOfAllPools / 2);
        // enemyPoolVariant3 = new EnemyPool(enemyPrefabVariant3, sizeOfAllPools / 3);
    }

    public IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves * spawnRateScalar);

            WaveParameters wave = waveParameters[Random.Range(0, waveParameters.Length)];

            SpawnEnemyWave(wave, enemyPoolVariant1, true);
            SpawnEnemyWave(wave, enemyPoolVariant2, true);
            // SpawnEnemyWave(wave, enemyPoolVariant3, true);

            SpawnEnemyWave(wave, enemyPoolVariant1, false);
            SpawnEnemyWave(wave, enemyPoolVariant2, false);
            // SpawnEnemyWave(wave, enemyPoolVariant3, false);

            // Clear the list for the next wave
            // spawnedEnemies.Clear();

            yield return null;
        }
    }

    void SpawnEnemyWave(WaveParameters wave, EnemyPool pool, bool spawnOnLeftSide)
    {
        spawnsSinceLast_Y_Pos_LastRandomized++;

        if (spawnsSinceLast_Y_Pos_LastRandomized >= spawnsUntilNextRandom_Y_Pos)
        {
            ySpawnRangeOffset = Random.Range(0f, 2f);
            spawnsSinceLast_Y_Pos_LastRandomized = 0;
        }

        int directionMultiplier = spawnOnLeftSide ? 1 : -1;

        float xOffset = directionMultiplier;
        float xPos = wave.spawnTransform.position.x + xOffset;

        GameObject enemy = pool.GetEnemy();
        enemy.transform.position = new Vector3(xPos, wave.spawnTransform.position.y + ySpawnRangeOffset, wave.spawnTransform.position.z);

        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();

        if (enemyMove != null)
            enemyMove.SetMovementParameters(wave.speedRange.GetRandomSpeed() * directionMultiplier);

        // Add the spawned enemy to the list
        // spawnedEnemies.Add(enemy);
    }

    public void ResetSingleton() => Instance = null;
}


    public class EnemyPool
    {
        GameObject prefab;
        int poolSize;
        Transform enemyPoolParent; // Reference to the parent object
        List<GameObject> activeEnemyList;
        List<GameObject> inactiveEnemyList;

        public EnemyPool(GameObject prefab, int poolSize)
        {
            this.prefab = prefab;
            this.poolSize = poolSize;
            InitializePool();
        }

        void InitializePool()
        {
            enemyPoolParent = new GameObject("Enemy Pool").transform;

            activeEnemyList = new List<GameObject>();
            inactiveEnemyList = new List<GameObject>();

            for (int i = 0; i < poolSize; i++)
            {
                GameObject enemy = Object.Instantiate(prefab, enemyPoolParent);
                // Debug.Log("spawner disabled enemy");
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
            {
                enemy = Object.Instantiate(prefab, enemyPoolParent);
                // Debug.Log("spawner created enemy");
            }

            enemy.SetActive(true);
            activeEnemyList.Add(enemy);

            return enemy;
        }

        public void ReturnEnemy(GameObject enemy)
        {
            if (activeEnemyList.Contains(enemy))
            {
                activeEnemyList.Remove(enemy);
                // Debug.Log("spawner disabled enemy");
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

