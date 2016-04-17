using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class healthColor : MonoBehaviour {

	public Renderer playerRend;

	public Image image;

	// Use this for initialization
	void Awake () 
	{
		image.color = playerRend.material.color;
	}
	
	// Update is called once per frame
	void Update () 
	{
		image.color = playerRend.material.color;
	}
}
