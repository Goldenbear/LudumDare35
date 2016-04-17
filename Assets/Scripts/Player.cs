using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// A Player ship and its controls
/// </summary>
public class Player : Ship 
{
	private const float k_sensitivity = 50.0f;

	// Public members
	public int m_playerNumber = 0;
	public int m_score = 0;
	public Text m_scoreUIText;

	

	// Use this for initialization
	protected override void Start() 
	{
        base.Start();
	}
	
	// Update is called once per frame
	void Update() 
	{
		PlayerControls();

		if(m_scoreUIText != null)
			m_scoreUIText.text = string.Format("{0:00000000}", m_score);
	}

	// Update controls from the appropriate input for this player
	void PlayerControls() 
	{
		string prefix = "P"+(m_playerNumber+1);
		float xAxisL = Input.GetAxis(prefix+"HorizontalL");
		float yAxisL = Input.GetAxis(prefix+"VerticalL");
		float xAxisR = Input.GetAxis(prefix+"HorizontalR");
		float yAxisR = Input.GetAxis(prefix+"VerticalR");
		Vector3 moveDir = new Vector3(xAxisL, -yAxisL, 0.0f);
		Vector3 fireDir = new Vector3(xAxisR, -yAxisR, 0.0f);

		// Move
		if(moveDir.sqrMagnitude > 0.5f)
		{
            Vector3 horizontal = Vector3.right * xAxisL * k_sensitivity;
            Vector3 vertical = Vector3.down * yAxisL * k_sensitivity;
            Move(horizontal, vertical);
		}

		// Fire
		if(fireDir.sqrMagnitude > 0.5f)
		{
			Fire(fireDir.normalized);
		}
		else
		{
			StopFiring();
		}

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
