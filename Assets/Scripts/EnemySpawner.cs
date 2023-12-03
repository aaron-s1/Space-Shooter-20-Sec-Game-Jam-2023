using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class WaveParameters
    {
        public float waveSpeed;
        public float verticalOffset;
    }

    [SerializeField] GameObject enemyPrefabVariant1;
    [SerializeField] GameObject enemyPrefabVariant2;
    [SerializeField] float timeBetweenWaves = 5f;
    [SerializeField] WaveParameters[] waveParameters;

    void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves);

            WaveParameters randomWave = waveParameters[Random.Range(0, waveParameters.Length)];

            GameObject chosenEnemyPrefab;
            
            if (Random.Range(0f, 1f) < 0.5f)
                chosenEnemyPrefab = enemyPrefabVariant1;
            else
                chosenEnemyPrefab = enemyPrefabVariant2;

            SpawnEnemyWave(randomWave, chosenEnemyPrefab);
        }
    }

    void SpawnEnemyWave(WaveParameters wave, GameObject enemyPrefab)
    {
        GameObject enemy = Instantiate(enemyPrefab, transform.position + new Vector3(0, wave.verticalOffset, 0), Quaternion.identity);
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();

        if (enemyMove != null)
        {
            // Set movement parameters for the enemy
            enemyMove.moveSpeed = wave.waveSpeed;
        }
    }
}



// using System.Collections;
// using UnityEngine;

// public class EnemySpawner : MonoBehaviour
// {
//     [System.Serializable]
//     public class SpawnPoint
//     {
//         public Vector3 position;
//     }

//     [System.Serializable]
//     public class Path
//     {
//         public Vector3[] waypoints;
//     }

//     [SerializeField] GameObject enemyPrefab;
//     [SerializeField] float timeBetweenWaves = 5f;
//     [SerializeField] SpawnPoint[] spawnPoints;
//     [SerializeField] Path[] paths;

//     void Start()
//     {
//         StartCoroutine(SpawnWaves());
//     }

//     IEnumerator SpawnWaves()
//     {
//         while (true)
//         {
//             yield return new WaitForSeconds(timeBetweenWaves);

//             foreach (SpawnPoint spawnPoint in spawnPoints)
//             {
//                 SpawnEnemy(spawnPoint);
//             }
//         }
//     }

//     void SpawnEnemy(SpawnPoint spawnPoint)
//     {
//         GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
//         EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();

//         if (enemyMove != null)
//         {
//             // Choose a random path
//             Path randomPath = paths[Random.Range(0, paths.Length)];
//             enemyMove.SetPath(randomPath.waypoints);
//         }
//     }
// }
