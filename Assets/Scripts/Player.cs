﻿using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// A Player ship and its controls
/// </summary>
public class Player : Ship 
{
	private const float k_sensitivity = 50.0f;
	private const float k_turnSpeed = 360.0f;

	// Public members
	public int m_playerNumber = 0;
	public int m_score = 0;
	public Text m_scoreUIText;
	public Image m_healthUIBar;

	// Private members
	private float m_healthBarOriginalWidth;

	// Use this for initialization
	protected override void Start() 
	{
        base.Start();

		if(m_healthUIBar != null)
			m_healthBarOriginalWidth = m_healthUIBar.rectTransform.sizeDelta.x;
	}
	
	// Update is called once per frame
	void Update() 
	{
		PlayerControls();

		UpdateHUD();
	}

	// Face ship in this direction
	void FaceDirection(Vector3 desiredDir)
	{
		Quaternion qCurrent = transform.rotation;
		Quaternion qDesired = Quaternion.LookRotation(Vector3.forward, desiredDir);
		transform.rotation = Quaternion.RotateTowards(qCurrent, qDesired, k_turnSpeed * Time.deltaTime);
	}

	// Update controls from the appropriate input for this player
	void PlayerControls() 
	{
		string prefix = "P"+(m_playerNumber+1);
		float xAxisL = Input.GetAxis(prefix+"HorizontalL");
		float yAxisL = Input.GetAxis(prefix+"VerticalL");
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		float xAxisR = Input.GetAxis(prefix+"HorizontalR_OSX");
		float yAxisR = Input.GetAxis(prefix+"VerticalR_OSX");
#else
		float xAxisR = Input.GetAxis(prefix+"HorizontalR");
		float yAxisR = Input.GetAxis(prefix+"VerticalR");
#endif
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
			FaceDirection(fireDir);
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

	// Update player's HUD
	void UpdateHUD()
	{
		if(m_scoreUIText != null)
			m_scoreUIText.text = string.Format("{0:00000000}", m_score);

		if(m_healthUIBar != null)
		{
			Vector2 size = m_healthUIBar.rectTransform.sizeDelta;
			size.x = m_healthBarOriginalWidth * m_health / 100.0f;
			m_healthUIBar.rectTransform.sizeDelta = size;
		}
	}
}
