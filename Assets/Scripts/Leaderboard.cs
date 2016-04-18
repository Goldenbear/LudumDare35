using UnityEngine;
using UnityEngine.UI;
using System;
using System.Text;
using System.Collections;

// Leaderboard screen menu navigation
public class Leaderboard : MonoBehaviour 
{
	// Public members
	public GameObject[] m_scores;
	public GameObject m_message;

	// Private members
	private int m_p1NameCharIndex = 0;
	private int m_p2NameCharIndex = 1;
	private bool isAxisInNeutral = true;

	// Use this for initialization
	void Start () 
	{
		// Set up leaderboard entries
		for(int i=0; i<m_scores.Length; i++)
		{
			Text text = m_scores[i].GetComponent<Text>();
			text.text = (i+1).ToString()+" "+GameManager.Get.HighNames[i]+" "+string.Format("{0:00000000}", GameManager.Get.HighScores[i]);
		}

		if(GameManager.Get.P1Index == -1)
			m_p1NameCharIndex = 3;
		if(GameManager.Get.P2Index == -1)
			m_p2NameCharIndex = 3;

		if((m_p1NameCharIndex == 3) && (m_p2NameCharIndex == 3))
			AllDone();
	}

	private string ReplaceChar(string text, int index, char newChar)
	{
		StringBuilder sb = new StringBuilder(text);
		sb[index] = newChar;
		return sb.ToString();
	}

	// Update is called once per frame
	void Update () 
	{
#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
		string suffix = "_OSX";
#else
		string suffix = "";
#endif

		// Edit P1 leaderboard name?
		if(GameManager.Get.P1Index != -1)
		{
			string prefix = "P"+GameManager.Get.PlayerOneNumber;

			// Edit a letter?
			if(m_p1NameCharIndex < 3)
			{
				float yAxisL = Input.GetAxis(prefix+"VerticalL");

				if(yAxisL < -0.5f)
				{
					if(isAxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P1Index];

						if(name[m_p1NameCharIndex] < 'Z')
						{
							name = ReplaceChar(name, m_p1NameCharIndex, (char)(name[m_p1NameCharIndex]+1));
							GameManager.Get.SetScoreName(0, name, GameManager.Get.P1Index);
						}

						isAxisInNeutral = false;
					}
				}
				else
				if(yAxisL > 0.5f)
				{
					if(isAxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P1Index];

						if(name[m_p1NameCharIndex] > 'A')
						{
							name = ReplaceChar(name, m_p1NameCharIndex, (char)(name[m_p1NameCharIndex]-1));
							GameManager.Get.SetScoreName(0, name, GameManager.Get.P1Index);
						}

						isAxisInNeutral = false;
					}
				}
				else
				{
					isAxisInNeutral = true;
				}

				Text text = m_scores[GameManager.Get.P1Index].GetComponent<Text>();
				text.text = (GameManager.Get.P1Index+1).ToString()+" "+GameManager.Get.HighNames[GameManager.Get.P1Index]+" "+string.Format("{0:00000000}", GameManager.Get.HighScores[GameManager.Get.P1Index]);
				if((Time.time % 1.0f) < 0.5f)
					text.text = ReplaceChar(text.text, 2+m_p1NameCharIndex, '_');
			}

			// Next letter
			if( Input.GetButtonDown(prefix+"Fire1"+suffix) || 
				Input.GetButtonDown(prefix+"Fire2"+suffix) || 
				Input.GetButtonDown(prefix+"Fire3"+suffix) || 
				Input.GetButtonDown(prefix+"Fire4"+suffix) || 
				Input.GetKeyDown("space") ||
				Input.GetKeyDown("return") )
			{
				if((m_p1NameCharIndex >= 3) && (m_p2NameCharIndex >= 3))
				{
					GameManager.Get.GameRestart();
				}

				m_p1NameCharIndex++;

				if(m_p2NameCharIndex == 3)
					AllDone();
			}
		}

		// Edit P2 leaderboard name?
		if(GameManager.Get.P1Index != -1)
		{
			string prefix = "P"+GameManager.Get.PlayerTwoNumber;

			// Edit a letter?
			if(m_p2NameCharIndex < 3)
			{
				float yAxisL = Input.GetAxis(prefix+"VerticalL");

				if(yAxisL < -0.5f)
				{
					if(isAxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P2Index];

						if(name[m_p2NameCharIndex] < 'Z')
						{
							name = ReplaceChar(name, m_p2NameCharIndex, (char)(name[m_p2NameCharIndex]+1));
							GameManager.Get.SetScoreName(1, name, GameManager.Get.P2Index);
						}

						isAxisInNeutral = false;
					}
				}
				else
				if(yAxisL > 0.5f)
				{
					if(isAxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P2Index];

						if(name[m_p2NameCharIndex] > 'A')
						{
							name = ReplaceChar(name, m_p2NameCharIndex, (char)(name[m_p2NameCharIndex]-1));
							GameManager.Get.SetScoreName(1, name, GameManager.Get.P2Index);
						}

						isAxisInNeutral = false;
					}
				}
				else
				{
					isAxisInNeutral = true;
				}

				Text text = m_scores[GameManager.Get.P2Index].GetComponent<Text>();
				text.text = (GameManager.Get.P2Index+1).ToString()+" "+GameManager.Get.HighNames[GameManager.Get.P2Index]+" "+string.Format("{0:00000000}", GameManager.Get.HighScores[GameManager.Get.P2Index]);
				if((Time.time % 1.0f) < 0.5f)
					text.text = ReplaceChar(text.text, 2+m_p2NameCharIndex, '_');
			}

			// Next letter
			if( Input.GetButtonDown(prefix+"Fire1"+suffix) ||
				Input.GetButtonDown(prefix+"Fire2"+suffix) ||
				Input.GetButtonDown(prefix+"Fire3"+suffix) ||
				Input.GetButtonDown(prefix+"Fire4"+suffix) )
			{
				if((m_p1NameCharIndex >= 3) && (m_p2NameCharIndex >= 3))
				{
					GameManager.Get.GameRestart();
				}

				m_p2NameCharIndex++;

				if(m_p2NameCharIndex == 3)
					AllDone();
			}
		}
	}

	private void AllDone()
	{
		Text text = m_message.GetComponent<Text>();
		text.text = "";
	}
}
