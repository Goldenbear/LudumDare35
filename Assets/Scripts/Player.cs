using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// A Player ship and its controls
/// </summary>
public class Player : Ship 
{
	private const float k_sensitivity = 50.0f;
	private const float k_turnSpeed = 360.0f;

    private PlayerAudioManger pam;

	// Public members
	public int m_playerNumber = 0;
	public int m_score = 0;
	public Text m_scoreUITitle;
	public Text m_scoreUIText;
	public Image m_healthUIBar;

	public void Deactivate()
	{
		gameObject.SetActive(false);

		if(m_scoreUITitle != null)
			m_scoreUITitle.gameObject.SetActive(false);
		if(m_scoreUIText != null)
			m_scoreUIText.gameObject.SetActive(false);
		if(m_healthUIBar != null)
			m_healthUIBar.transform.parent.gameObject.SetActive(false);
	}

	// Private members
	private float m_healthBarOriginalWidth;

	// Use this for initialization
	protected override void Start() 
	{
        base.Start();

		if(m_healthUIBar != null)
			m_healthBarOriginalWidth = m_healthUIBar.rectTransform.sizeDelta.x;

        pam = GetComponent<PlayerAudioManger>();

		OnShipDestroyed.AddListener(OnShipDeath);
	}
	
	// Update is called once per frame
	void Update() 
	{
		PlayerControls();

		UpdateHUD();
	}

	// Update controls from the appropriate input for this player
	void PlayerControls() 
	{
		int playerNumber = (m_playerNumber == 0) ? GameManager.Get.PlayerOneInputNumber : GameManager.Get.PlayerTwoInputNumber;
		string prefix = "P"+playerNumber;
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		string suffix = "_OSX";
#else
		string suffix = "";
#endif
		float xAxisL = Input.GetAxis(prefix+"HorizontalL");
		float yAxisL = Input.GetAxis(prefix+"VerticalL");
		float xAxisR = Input.GetAxis(prefix+"HorizontalR"+suffix);
		float yAxisR = Input.GetAxis(prefix+"VerticalR"+suffix);
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
			FaceDirection(fireDir, k_turnSpeed);
			Fire(fireDir.normalized);
		}
		else
		{
			StopFiring();
		}

		if(Input.GetButtonDown(prefix+"Fire1"+suffix))
		{
			ShapeShift(ShipShape.k_cross);
			//Debug.Log(prefix+"Fire1");
            pam.PlayShiftShapeSound();
		}
		else
		if(Input.GetButtonDown(prefix+"Fire2"+suffix))
		{
			ShapeShift(ShipShape.k_circle);
			//Debug.Log(prefix+"Fire2");
            pam.PlayShiftShapeSound();
		}
		else
		if(Input.GetButtonDown(prefix+"Fire3"+suffix))
		{
			ShapeShift(ShipShape.k_square);
			//Debug.Log(prefix+"Fire3");
            pam.PlayShiftShapeSound();
		}
		else
		if(Input.GetButtonDown(prefix+"Fire4"+suffix))
		{
			ShapeShift(ShipShape.k_triangle);
			//Debug.Log(prefix+"Fire4");
            pam.PlayShiftShapeSound();
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

	// Death
	void OnShipDeath(Ship deadShip)
	{
		// Remove player from the scene (dont destroy as stuff like projectiles keeps references to the player)
		gameObject.SetActive(false);
	}
}
