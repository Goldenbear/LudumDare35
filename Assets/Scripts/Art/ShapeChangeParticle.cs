using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShapeChangeParticle : MonoBehaviour {

	public GameObject shiftEffect;
	private bool hasTriggered = false;
	private float timer;
	private float timeOfEffect = 7.0f;

	public List<ParticleSystem> engineEffcts = new List<ParticleSystem>();

	public Player playerScript;

	// Use this for initialization
	void Awake () 
	{

	}

	void Update () 
	{
		if(hasTriggered = true)
		{
			timer += Time.deltaTime;
		}
		// resets timer
		if(timer >= timeOfEffect)
		{
			shiftEffect.SetActive(false);
			timer = 0.0f;
			hasTriggered = false;
		}
	}

	public void ChangeEffect()
	{
		shiftEffect.SetActive(true);
		hasTriggered = true;
	}

	public void triggerShapeChange()
	{
		playerScript.ShapeShiftNow();
		for(int i = 0; i < engineEffcts.Count; i++)
		{
			ParticleSystem.EmissionModule engineStatus = engineEffcts[i].emission;
			engineStatus.enabled = true;
		}

	}
}
