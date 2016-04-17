using UnityEngine;
using System.Collections;

public class ExaustLight : MonoBehaviour {

	public float maxIntensity;
	public float startIntensity;

	public float RampUpSpeed;

	public float flickerRange;
	private bool isRamped = false;
	private float currIntensity;
	private Light light;

	void Awake () 
	{
		light = this.GetComponent<Light>();
		light.intensity = startIntensity;
		currIntensity = startIntensity;
	}

	void Update () 
	{
		if(!isRamped)
		{
			currIntensity = Mathf.Lerp(currIntensity, maxIntensity, RampUpSpeed * Time.deltaTime); 
			light.intensity = currIntensity;
			Debug.Log(currIntensity);
			if (light.intensity >= maxIntensity -0.05)
			{
				isRamped = true;
			}
		}
		else
		{
			float randIntensityChange = Random.Range(maxIntensity-flickerRange, maxIntensity+flickerRange);
			currIntensity = randIntensityChange;
			light.intensity = currIntensity;
		}
	}
}
