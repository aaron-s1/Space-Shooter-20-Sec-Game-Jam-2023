using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set; }

    public float spawnRateScalar = 1f;

    [System.Serializable]
    public class WaveParameters
    {
        public Transform spawnTransform;
        public SpeedRange speedRange;
        // public float movementSpeed;
        // public float verticalOffset;
    }

    [SerializeField] int totalPoolSize;
    [SerializeField] GameObject enemyPrefabVariant1;
    [SerializeField] GameObject enemyPrefabVariant2;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] int spawnsUntilNextRandom_Y_Pos;

    [Space(10)]
    [SerializeField] WaveParameters[] waveParameters;

    [SerializeField] SpeedRange enemySpeedRange;


    EnemyPool enemyPool;
    int spawnsSinceLast_Y_Pos_LastRandomized;
    float ySpawnRangeOffset;


    void Awake() =>
        Instance = this;


    void Start()
    {
        InitializeEnemyPool();
        ySpawnRangeOffset = Random.Range(-0.25f, 2.25f);
            // Debug.Log(ySpawnRangeOffset);
        // StartCoroutine(SpawnWaves());
    }

    void InitializeEnemyPool() =>
        enemyPool = new EnemyPool(enemyPrefabVariant1, totalPoolSize);


    public IEnumerator SpawnWaves()
    {
        Debug.Break();
        // Debug.Log("spawn waves called");
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves * spawnRateScalar);

            WaveParameters wave1 = waveParameters[Random.Range(0, waveParameters.Length)];
            // WaveParameters wave2 = waveParameters[Random.Range(0, waveParameters.Length)];

            // wave2.verticalOffset *= -1;

            SpawnEnemyWave(wave1, enemyPrefabVariant1);
            // SpawnEnemyWave(wave2, enemyPrefabVariant2);

            yield return null;
        }
    }


    void SpawnEnemyWave(WaveParameters wave, GameObject enemyPrefab)
    {
        spawnsSinceLast_Y_Pos_LastRandomized++;

        if (spawnsSinceLast_Y_Pos_LastRandomized >= spawnsUntilNextRandom_Y_Pos)
        {        
            ySpawnRangeOffset = Random.Range(0f, 2f);
            spawnsSinceLast_Y_Pos_LastRandomized = 0;
        }

        int directionMultiplier = (wave == waveParameters[0]) ? 1 : -1;

        float xOffset = directionMultiplier;
        float xPos = wave.spawnTransform.position.x + xOffset;


        GameObject enemy = enemyPool.GetEnemy();
        enemy.transform.position = new Vector3(xPos, wave.spawnTransform.position.y + ySpawnRangeOffset, wave.spawnTransform.position.z);

        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();

        if (enemyMove != null)
            enemyMove.SetMovementParameters(wave.speedRange.GetRandomSpeed() * directionMultiplier);
            // enemyMove.SetMovementParameters(wave.movementSpeed * directionMultiplier);
    }

    
}



    public class EnemyPool
    {
        GameObject prefab;
        int poolSize;
        List<GameObject> activeEnemyList;
        List<GameObject> inactiveEnemyList;

        // public void ListEnemies()
        // {
        //     Debug.Log("ListEnemies() called");

        //     foreach (var obj in activeEnemyList)
        //         Debug.Log("active enemies = " + obj.name);

        //     foreach (var obj in inactiveEnemyList)
        //         Debug.Log("inactive enemies = " + obj.name);
        // }

        public EnemyPool(GameObject prefab, int poolSize)
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
                Debug.Log("spawner disabled enemy");
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
                Debug.Log("spawner disabled enemy");
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
