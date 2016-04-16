using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField, Tooltip("Prefab of the ship to spawn")]
    GameObject enemyPrefab;

    [SerializeField, Tooltip("Number of ships to spawn")]
    int numEnemies;

    [SerializeField, Tooltip("Seconds between spawn times")]
    float spawnTime;

    int enemiesSpawned = 0;

    // Use this for initialization
    void Start()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("Gun does not have a ship object");
        }

        this.StartCoroutine(Spawn());
    }

    // Update is called once per frame
    void Update()
    {
        if (IsSpawnComplete()) Destroy(this.gameObject);
    }

    protected Enemy SpawnEnemy()
    {
        Enemy e = (Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject).GetComponent<Enemy>();
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

