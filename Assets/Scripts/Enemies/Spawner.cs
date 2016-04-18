using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

public class Spawner : MonoBehaviour
{
    public enum StartTrigger
    {
        Immediate,
        PreviousSpawnComplete,
        PreviousEnemiesDead,
    }

    protected enum VelocitySet
    {
        None,
        Random,
        Specified
    }

    [Serializable]
    public class EnemySpawnedEvent : UnityEvent<Enemy> { }

    public EnemySpawnedEvent OnEnemySpawned = new EnemySpawnedEvent();

    [SerializeField, Tooltip("Prefab of the enemy to spawn")]
    List<GameObject> enemyPrefabs;

    [SerializeField, Tooltip("Number of enemies to spawn")]
    protected int numEnemies;

    [SerializeField, Tooltip("Seconds between spawn times")]
    protected float spawnTime;

    [SerializeField, Tooltip("General direction the enemies will leave the screen")]
    protected Enemy.ExitStrategy exit;

    [SerializeField, Tooltip("How we set an enemy's initial velocity")]
    protected VelocitySet velocitySet;

    [SerializeField, Tooltip("The initial velocity of the ships")]
    protected Vector3 enemyInitialVelocity;

    [SerializeField, Tooltip("What triggers the starting of this spawner")]
    protected StartTrigger startTrigger;

    protected int enemiesSpawned = 0;

    public bool IsSpawning { get { return enemiesSpawned > 0; } }
    public StartTrigger SpawnTrigger { get { return startTrigger; } }


    void Awake()
    {
        
    }

    void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count < 1)
        {
            Debug.LogError("Spawner doesn't have enemy types");
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
        Enemy e = (Instantiate(ChooseEnemyPrefab(), spawnPoint, rotation) as GameObject).GetComponent<Enemy>();
        if(velocitySet == VelocitySet.Specified)
        {
            e.SetInitialVelocity(enemyInitialVelocity);
        }
        e.SetExitStrategy(exit);
        
        enemiesSpawned++;

        OnEnemySpawned.Invoke(e);
        return e;
    }

    GameObject ChooseEnemyPrefab()
    {
        int index = UnityEngine.Random.Range(0, enemyPrefabs.Count);
        return enemyPrefabs[index];
    }

    protected virtual IEnumerator Spawn()
    {
        while(enemiesSpawned < numEnemies)
        {
            SpawnEnemy();
            if (spawnTime > 0) yield return new WaitForSeconds(spawnTime);
        }
        yield return null;
    }

    protected virtual bool IsSpawnComplete()
    {
        return enemiesSpawned == numEnemies;
    }
}

