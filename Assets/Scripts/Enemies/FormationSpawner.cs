using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FormationSpawner : Spawner
{
    [SerializeField, Tooltip("Units from one size of the formation to the other")]
    protected float formationSize;


    protected List<Vector3> spawnPoints;

    protected override IEnumerator Spawn()
    {
        GenerateSpawnPoints();

        if(spawnPoints == null || spawnPoints.Count < numEnemies)
        {
            Debug.LogErrorFormat("We did not generate enough spawn points for all enemies. Killing spawn.");
            RemoveSpawner();
        }

        foreach(Vector3 sp in spawnPoints)
        {
            SpawnEnemy(sp);
            if (spawnTime > 0) yield return new WaitForSeconds(spawnTime);
        }

        yield return null;
    }

    protected virtual void GenerateSpawnPoints()
    {
        List<Vector3> newPoints = new List<Vector3>();

        // TODO: Generate a better list?

        for(int i = 0; i < numEnemies; i++)
        {
            newPoints.Add(Vector3.zero);
        }

        spawnPoints = newPoints;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;

        if(spawnPoints != null)
        {
            foreach(Vector3 v in spawnPoints)
            {
                Gizmos.DrawWireSphere(v, 1);
            }
        }
        
    }
}

