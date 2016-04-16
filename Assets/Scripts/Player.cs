using UnityEngine;
using System.Collections;

/// <summary>
/// A Player ship and its controls
/// </summary>
public class Player : Ship 
{
	private const float k_sensitivity = 50.0f;

	// Public members
	public int m_playerNumber = 0;

	// Private members
	private Rigidbody rigidbody;

	// Use this for initialization
	void Start () 
	{
		rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update() 
	{
		PlayerControls();
	}

	// Update controls from the appropriate input for this player
	void PlayerControls() 
	{
		string prefix = "P"+(m_playerNumber+1);
		float xAxisL = Input.GetAxis(prefix+"HorizontalL");
		float yAxisL = Input.GetAxis(prefix+"VerticalL");
		float xAxisR = Input.GetAxis(prefix+"HorizontalR");
		float yAxisR = Input.GetAxis(prefix+"VerticalR");

		rigidbody.AddForce(Vector3.right * xAxisL * k_sensitivity);
		rigidbody.AddForce(Vector3.down * yAxisL * k_sensitivity);

		if(Input.GetButtonDown(prefix+"Fire1"))
		{
			ShapeShift(ShipShape.k_square);
			Debug.Log(prefix+"Fire1");
		}
		else
		if(Input.GetButtonDown(prefix+"Fire2"))
		{
			ShapeShift(ShipShape.k_circle);
			Debug.Log(prefix+"Fire2");
		}
		else
		if(Input.GetButtonDown(prefix+"Fire3"))
		{
			ShapeShift(ShipShape.k_triangle);
			Debug.Log(prefix+"Fire3");
		}
		else
		if(Input.GetButtonDown(prefix+"Fire4"))
		{
			ShapeShift(ShipShape.k_cross);
			Debug.Log(prefix+"Fire4");
		}
	}
}
