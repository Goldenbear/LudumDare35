using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveWithUVs : MonoBehaviour {

	private List<Transform> players = new List<Transform>();

	public Vector3 addedDirection {get; set;}
	private Transform effectedGO;
	public Vector3 newOffset {get; set;}
	private Vector3 currOffset;

	public float maxSpeed  = 0.02f;
	public float lerpSpeed = 0.75f;

	// Use this for initialization
	void Awake () 
	{
		GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
		for (int i=0; i < playersGO.Length; i++)
		{
			players.Add(playersGO[i].transform);
		}
		effectedGO = this.gameObject.transform;
		currOffset = effectedGO.localPosition;	
		newOffset = effectedGO.localPosition;
	}

	// Update is called once per frame
	void Update () 
	{
		addedDirection = Vector3.zero;
		for (int i=0; i < players.Count; i++)
		{
			addedDirection = addedDirection - new Vector3(players[i].up.x, players[i].up.y, 0.0f) * Time.deltaTime;
		}


		addedDirection = Vector3.ClampMagnitude (addedDirection, maxSpeed);

		newOffset += (addedDirection * 0.5f);

		currOffset = Vector3.Lerp (currOffset, newOffset, lerpSpeed * Time.deltaTime);

		effectedGO.localPosition = currOffset;

	}
}
