using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Splash screen menu navigation
public class SplashMenu : MonoBehaviour 
{
	// Public members
	public GameObject[] m_buttons;

	// Private members
	private int m_currentButton = 0;
	private bool isAxisInNeutral = true;

	// Use this for initialization
	void Start () 
	{
		if(m_buttons.Length == 0)
		{
			gameObject.SetActive(false);
			return;
		}

		Image newImage = m_buttons[m_currentButton].GetComponent<Image>();
		newImage.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () 
	{
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		string suffix = "_OSX";
#else
		string suffix = "";
#endif

		// Move up/down menu items
		float yAxisL = Input.GetAxis("P1VerticalL") + Input.GetAxis("P2VerticalL");

		if(yAxisL < -0.5f)
		{
			if(isAxisInNeutral)
			{
				MoveUp();
				isAxisInNeutral = false;
			}
		}
		else
		if(yAxisL > 0.5f)
		{
			if(isAxisInNeutral)
			{
				MoveDown();
				isAxisInNeutral = false;
			}
		}
		else
		{
			isAxisInNeutral = true;
		}

		// Select current menu item
		if( Input.GetButtonDown("P1Fire1"+suffix) || 
			Input.GetButtonDown("P1Fire2"+suffix) || 
			Input.GetButtonDown("P1Fire3"+suffix) || 
			Input.GetButtonDown("P1Fire4"+suffix) || 
			Input.GetKeyDown("space") ||
			Input.GetKeyDown("return") )
		{
			if(m_currentButton == 0)
				GameManager.Get.GameStart(1, 1);
			else
			if(m_currentButton == 1)
				GameManager.Get.GameStart(2);
		}

		if( Input.GetButtonDown("P2Fire1"+suffix) ||
			Input.GetButtonDown("P2Fire2"+suffix) ||
			Input.GetButtonDown("P2Fire3"+suffix) ||
			Input.GetButtonDown("P2Fire4"+suffix) )
		{
			if(m_currentButton == 0)
				GameManager.Get.GameStart(1, 2);
			else
			if(m_currentButton == 1)
				GameManager.Get.GameStart(2);
		}
	}

	private void MoveUp()
	{
		Image oldImage = m_buttons[m_currentButton].GetComponent<Image>();
		oldImage.color = Color.grey;

		m_currentButton--;
		if(m_currentButton < 0)
			m_currentButton = m_buttons.Length-1;

		Image newImage = m_buttons[m_currentButton].GetComponent<Image>();
		newImage.color = Color.white;
	}

	private void MoveDown()
	{
		Image oldImage = m_buttons[m_currentButton].GetComponent<Image>();
		oldImage.color = Color.grey;

		m_currentButton++;
		if(m_currentButton >= m_buttons.Length)
			m_currentButton = 0;

		Image newImage = m_buttons[m_currentButton].GetComponent<Image>();
		newImage.color = Color.white;
	}
}
