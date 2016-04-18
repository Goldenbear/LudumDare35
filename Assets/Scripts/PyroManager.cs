using UnityEngine;
using System.Collections;

public class PyroManager : MonoBehaviour
{
    public GameObject[] Explosions;
    //public GameObject EnemyExplosion;

    public void SpawnExplosion(int explosionType, Vector3 explosionPosition)
    {
        GameObject.Instantiate(Explosions[explosionType], explosionPosition, Quaternion.identity);
    }
}

public enum ExplosionType
{
    PLAYER = 0,
    ENEMY = 1
}