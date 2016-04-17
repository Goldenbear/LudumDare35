using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGManager : MonoBehaviour {

	private List<Transform> players = new List<Transform>();

	private Vector2 addedDirection;
	private Renderer BGMat;
	private Vector2 newOffset;
	private Vector2 currOffset;

	public float maxSpeed;
	public float lerpSpeed;

	// Use this for initialization
	void Awake () 
	{
		GameObject[] playersGO = GameObject.FindGameObjectsWithTag("Player");
		for (int i=0; i < playersGO.Length; i++)
		{
			players.Add(playersGO[i].transform);
		}
		BGMat = this.gameObject.GetComponent<Renderer>();
		currOffset = Vector2.zero;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		addedDirection = Vector3.zero;
		for (int i=0; i < players.Count; i++)
		{
			addedDirection = addedDirection + new Vector2(players[i].up.x, players[i].up.y) * Time.deltaTime;
		}


		addedDirection = Vector2.ClampMagnitude (addedDirection, maxSpeed);

		newOffset += (addedDirection * 0.5f);

		currOffset = Vector2.Lerp (currOffset, newOffset, lerpSpeed * Time.deltaTime);

		BGMat.material.SetTextureOffset("_MainTex", currOffset);

	}

}
