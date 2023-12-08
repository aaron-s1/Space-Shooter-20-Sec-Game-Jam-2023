using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner Instance { get; private set;}

    // Every time an enemy dies, it tells the spawner to start spawning faster.
    public float spawnRateScale = 1f;
     
    [System.Serializable]
    public class WaveParameters
    {
        public float movementSpeed;
        public float verticalOffset;
        public Transform spawnTransform;
    }

    [SerializeField] GameObject enemyPrefabVariant1;
    [SerializeField] GameObject enemyPrefabVariant2;
    [SerializeField] float timeBetweenWaves;
    [SerializeField] WaveParameters[] waveParameters;


    void Awake()
    {
        Instance = this;
    }


    public IEnumerator SpawnWaves()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenWaves * spawnRateScale);

            WaveParameters wave1 = waveParameters[Random.Range(0, waveParameters.Length)];
            WaveParameters wave2 = waveParameters[Random.Range(0, waveParameters.Length)];

            wave2.verticalOffset *= -1;

            SpawnEnemyWave(wave1, enemyPrefabVariant1);
            SpawnEnemyWave(wave2, enemyPrefabVariant2);
        }
    }

    void SpawnEnemyWave(WaveParameters wave, GameObject enemyPrefab)
    {
        int directionMultiplier = (wave == waveParameters[0]) ? 1 : -1;

        float xOffset = directionMultiplier;
        float xPos = wave.spawnTransform.position.x + xOffset;

        GameObject enemy = Instantiate(enemyPrefab, new Vector3(xPos, wave.spawnTransform.position.y + wave.verticalOffset, wave.spawnTransform.position.z), Quaternion.identity);
        EnemyMove enemyMove = enemy.GetComponent<EnemyMove>();

        if (enemyMove != null)
            enemyMove.moveSpeed = wave.movementSpeed * directionMultiplier;
    }
}
