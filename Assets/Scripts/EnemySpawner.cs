using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject enemyVariant1;
    [SerializeField] GameObject enemyVariant2;

    List<GameObject> enemyPool;
    [SerializeField] int poolSize = 100;
    [SerializeField] float enemySpawnRate;

    bool spawnEnemies;

    void Start()
    {
        enemyPool = new List<GameObject>();
        InitializePool();
        SpawnEnemies(true);    
    }
    
    public IEnumerator Spawn(float spawnRate = 0)
    {
        if (spawnRate == 0)
        {
            Debug.Log("Enemy Spawner stopped spawning.");
            yield break;
        }

        while (spawnEnemies)
        {
            yield return new WaitForSeconds(spawnRate);
            GameObject enemy = GetRandomPooledEnemy();

            if (enemy != null)
            {
                // set positions, rotations.
                enemy.SetActive(true);
            }
        }
    }


    void InitializePool()
    {
        for (int i = 0; i < poolSize / 2; i++)
        {
            GameObject enemy = Instantiate(enemyVariant1, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }

        for (int i = 0; i < poolSize / 2; i++)
        {
            GameObject enemy = Instantiate(enemyVariant2, Vector3.zero, Quaternion.identity);
            enemy.SetActive(false);
            enemyPool.Add(enemy);
        }        
    }

    GameObject GetRandomPooledEnemy()
    {
        List<GameObject> inactiveEnemies = enemyPool.FindAll(enemy => !enemy.activeInHierarchy);

        if (inactiveEnemies.Count > 0)
        {
            int randomIndex = Random.Range(0, inactiveEnemies.Count);
            return inactiveEnemies[randomIndex];
        }

        return null;
    }

    public void SpawnEnemies(bool newValue) 
    {
        spawnEnemies = newValue;

        if (newValue == true)
            StartCoroutine(Spawn(enemySpawnRate));
        else
            StopCoroutine(Spawn());
    }


    // GameObject GetRandomPooledEnemy()
    // {
    //     int randomIndex = Random.Range(0, poolSize);
    //     return enemyPool[randomIndex];
    //     // return enemyPool.Find(missile => !missile.activeInHierarchy);
    // }
}
