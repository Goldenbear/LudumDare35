using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleSpawner : FormationSpawner
{
    enum SpawnOrder
    {
        InOrder,
        Randomish
    }

    [SerializeField, Tooltip("Spawning behaviour")]
    SpawnOrder order;

    protected override void GenerateSpawnPoints()
    {
        List<Vector3> newPoints = new List<Vector3>();
        float degreesBetween = 360 / numEnemies;
        float radius = formationSize / 2;

        // TODO: Don't always start at the top
        Vector3 nextSpawn = new Vector3(0,radius);

        while(newPoints.Count < numEnemies)
        {
            newPoints.Add(nextSpawn);
            nextSpawn = Quaternion.AngleAxis(degreesBetween, Vector3.back) * nextSpawn;
        }

        if (order == SpawnOrder.InOrder)
        {
            spawnPoints = newPoints;
        }
        else
        {
            List<Vector3> randomPoints = new List<Vector3>();

            while(randomPoints.Count < numEnemies)
            {
                int index = Random.Range(0, newPoints.Count - 1);
                randomPoints.Add(newPoints[index]);
                newPoints.RemoveAt(index);
            }

            spawnPoints = randomPoints;
        }
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.magenta;
        Vector3 thisPos = this.transform.position;

        float radius = formationSize / 2;

        var hFrom = new Vector3(thisPos.x - radius, thisPos.y);
        var hTo = new Vector3(thisPos.x + radius, thisPos.y);
        var vFrom = new Vector3(thisPos.x, thisPos.y - radius);
        var vTo = new Vector3(thisPos.x, thisPos.y + radius);

        Gizmos.DrawLine(hFrom, hTo);
        Gizmos.DrawLine(vFrom, vTo);
    }
}
