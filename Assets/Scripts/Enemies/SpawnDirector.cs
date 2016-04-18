﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnDirector : MonoBehaviour
{

    [SerializeField, Tooltip("The list of spawners to use")]
    List<Spawner> spawnOrder;

    [SerializeField, Tooltip("True if Spawner order is random")]
    bool isOrderRandom;

    List<Enemy> spawnedEnemies = new List<Enemy>();
    Spawner previousSpawner;
    Spawner nextSpawner;

    public bool IsComplete { get { return spawnOrder.Count == 0 && nextSpawner == null; } }

    void Awake()
    {
        // TODO: This is happening twice?
        Debug.Log("SpawnDirector " + this.name + " AWAKE");
        spawnOrder = new List<Spawner>(GetComponentsInChildren<Spawner>());
        foreach(Spawner s in spawnOrder)
        {
            s.gameObject.SetActive(false);
        }
    }

	// Use this for initialization
	void Start () {
        SelectNextSpawner();
	}
	
	// Update is called once per frame
	void Update () {
        if(IsComplete)
        {
            // TODO: Destroy?
            return;
        }

        if(IsNextSpawnerReadyToStart())
        {
            StartNextSpawner();
        }
	}

    void SelectNextSpawner()
    {
        if (spawnOrder.Count > 0)
        {
            previousSpawner = nextSpawner;

            int nextIndex = 0;
            if(isOrderRandom)
            {
                nextIndex = Random.Range(0, spawnOrder.Count);
            }

            nextSpawner = spawnOrder[nextIndex];
            spawnOrder.RemoveAt(nextIndex);

            if (IsNextSpawnerReadyToStart())
            {
                StartNextSpawner();
            }
        }
    }

    void StartNextSpawner()
    {
        if (nextSpawner != null && !nextSpawner.IsSpawning)
        {
            nextSpawner.OnEnemySpawned.AddListener(OnEnemySpawned);
            nextSpawner.gameObject.SetActive(true);
            Debug.LogFormat("<color=blue>Started Spawner:</color> {0} {1}", nextSpawner.name, nextSpawner.SpawnTrigger);
        }

        SelectNextSpawner();
    }

    bool IsNextSpawnerReadyToStart()
    {
        switch(nextSpawner.SpawnTrigger)
        {
            case Spawner.StartTrigger.PreviousEnemiesDead:
                return (HasPreviousStartedSpawning() && spawnedEnemies.Count == 0);
            case Spawner.StartTrigger.PreviousSpawnComplete:
                return previousSpawner == null;
            case Spawner.StartTrigger.Immediate:
            default:
                return true;
        }
    }

    bool HasPreviousStartedSpawning()
    {
        // We assume if they don't exist they spawned and died
        if (previousSpawner == null)
            return true;

        return previousSpawner.IsSpawning;
    }

    void OnEnemySpawned(Enemy newEnemy)
    {
        spawnedEnemies.Add(newEnemy);
        newEnemy.OnDeath.AddListener(OnEnemyDeath);
    }

    void OnEnemyDeath(Enemy deadEnemy)
    {
        spawnedEnemies.Remove(deadEnemy);
    }
}
