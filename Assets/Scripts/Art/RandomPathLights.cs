using UnityEngine;
using System.Collections;

public class RandomPathLights : MonoBehaviour {

	private float spawnTimer;
	public float minSpawnRandTime;
	public float maxSpawnRandTime;
	public float spawnDelay;
	public float MinX;
	public float MinY;
	public float MaxX;
	public float MaxY;

	public float lightSpeed;
	public GameObject lightPrefab;


	void Update () 
	{
		if(spawnTimer >= spawnDelay)
		{
			// pick random point for spawn
			Vector2 spawnPoint = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
			// pick light target    flip  x and Y based on spawn point
			Vector2 targetPoint = new Vector2(Random.Range(MinX, MaxX), Random.Range(MinY, MaxY));
			// get direction

			// instansiate light with speed in target direction

			spawnDelay = Random.Range (minSpawnRandTime, maxSpawnRandTime);
			spawnTimer = 0.0f;
		}


	}
}
