using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab of the enemy to spawn")]
    GameObject enemyPrefab;

    [SerializeField, Tooltip("Number of enemies to spawn")]
    protected int numEnemies;

    [SerializeField, Tooltip("Seconds between spawn times")]
    protected float spawnTime;

    protected int enemiesSpawned = 0;

    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Gun does not have a ship object");
        }

        this.StartCoroutine(Spawn());
    }

    void Update()
    {
        if (IsSpawnComplete()) RemoveSpawner();
    }

    protected void RemoveSpawner()
    {
        Destroy(this.gameObject);
    }

    protected Enemy SpawnEnemy()
    {
        return SpawnEnemy(transform.position, transform.rotation);
    }

    protected Enemy SpawnEnemy(Vector3 spawnPoint, Quaternion rotation = new Quaternion())
    {
        Enemy e = (Instantiate(enemyPrefab, spawnPoint, rotation) as GameObject).GetComponent<Enemy>();
        enemiesSpawned++;
        return e;
    }

    protected virtual IEnumerator Spawn()
    {
        while(enemiesSpawned < numEnemies)
        {
            Enemy enemy = SpawnEnemy();
            if (spawnTime > 0) yield return new WaitForSeconds(spawnTime);
        }
        yield return null;
    }

    protected virtual bool IsSpawnComplete()
    {
        return enemiesSpawned == numEnemies;
    }
}

