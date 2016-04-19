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
	private int m_p2NameCharIndex = 0;
	private bool isP1AxisInNeutral = true;
	private bool isP2AxisInNeutral = true;

	// Use this for initialization
	void Start () 
	{
		// Set up leaderboard entries
		for(int i=0; i<m_scores.Length; i++)
		{
			SetEntryName(m_scores[i], GameManager.Get.HighNames[i]);
			SetEntryScore(m_scores[i], string.Format("{0:00000000}", GameManager.Get.HighScores[i]));
		}

		if(GameManager.Get.P1Index == -1)
			m_p1NameCharIndex = 3;
		if(GameManager.Get.P2Index == -1)
			m_p2NameCharIndex = 3;

		if((m_p1NameCharIndex == 3) && (m_p2NameCharIndex == 3))
			AllDone();
	}

	private void SetEntryScore(GameObject entryObj, string score)
	{
		GameObject scoreObj = entryObj.transform.Find("Score").gameObject;
		Text text = scoreObj.GetComponent<Text>();
		text.text = score;
	}

	private void SetEntryName(GameObject entryObj, string name)
	{
		GameObject scoreObj = entryObj.transform.Find("Name").gameObject;
		Text text = scoreObj.GetComponent<Text>();
		text.text = name;
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
			string prefix = "P"+GameManager.Get.PlayerOneInputNumber;

			// Edit a letter?
			if(m_p1NameCharIndex < 3)
			{
				float yAxisL = Input.GetAxis(prefix+"VerticalL");

				if(yAxisL < -0.2f)
				{
					if(isP1AxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P1Index];

						if(name[m_p1NameCharIndex] < 'Z')
						{
							name = ReplaceChar(name, m_p1NameCharIndex, (char)(name[m_p1NameCharIndex]+1));
							GameManager.Get.SetScoreName(0, name, GameManager.Get.P1Index);
						}

						isP1AxisInNeutral = false;
					}
				}
				else
				if(yAxisL > 0.2f)
				{
					if(isP1AxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P1Index];

						if(name[m_p1NameCharIndex] > 'A')
						{
							name = ReplaceChar(name, m_p1NameCharIndex, (char)(name[m_p1NameCharIndex]-1));
							GameManager.Get.SetScoreName(0, name, GameManager.Get.P1Index);
						}

						isP1AxisInNeutral = false;
					}
				}
				else
				{
					isP1AxisInNeutral = true;
				}

				// Update display name flashing letter being edited
				SetEntryName(m_scores[GameManager.Get.P1Index], GameManager.Get.HighNames[GameManager.Get.P1Index]);
				if((Time.time % 0.5f) < 0.25f)
				{
					string tempName = ReplaceChar(GameManager.Get.HighNames[GameManager.Get.P1Index], m_p1NameCharIndex, '_');
					SetEntryName(m_scores[GameManager.Get.P1Index], tempName);
				}
			}
			else
			{
				// Update display name
				SetEntryName(m_scores[GameManager.Get.P1Index], GameManager.Get.HighNames[GameManager.Get.P1Index]);
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

				if((m_p1NameCharIndex >= 3) && (m_p2NameCharIndex >= 3))
					AllDone();
			}
		}

		// Edit P2 leaderboard name?
		if(GameManager.Get.P2Index != -1)
		{
			string prefix = "P"+GameManager.Get.PlayerTwoInputNumber;

			// Edit a letter?
			if(m_p2NameCharIndex < 3)
			{
				float yAxisL = Input.GetAxis(prefix+"VerticalL");

				if(yAxisL < -0.2f)
				{
					if(isP2AxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P2Index];

						if(name[m_p2NameCharIndex] < 'Z')
						{
							name = ReplaceChar(name, m_p2NameCharIndex, (char)(name[m_p2NameCharIndex]+1));
							GameManager.Get.SetScoreName(1, name, GameManager.Get.P2Index);
						}

						isP2AxisInNeutral = false;
					}
				}
				else
				if(yAxisL > 0.2f)
				{
					if(isP2AxisInNeutral)
					{
						string name = GameManager.Get.HighNames[GameManager.Get.P2Index];

						if(name[m_p2NameCharIndex] > 'A')
						{
							name = ReplaceChar(name, m_p2NameCharIndex, (char)(name[m_p2NameCharIndex]-1));
							GameManager.Get.SetScoreName(1, name, GameManager.Get.P2Index);
						}

						isP2AxisInNeutral = false;
					}
				}
				else
				{
					isP2AxisInNeutral = true;
				}

				// Update display name flashing letter being edited
				SetEntryName(m_scores[GameManager.Get.P2Index], GameManager.Get.HighNames[GameManager.Get.P2Index]);
				if((Time.time % 0.5f) < 0.25f)
				{
					string tempName = ReplaceChar(GameManager.Get.HighNames[GameManager.Get.P2Index], m_p2NameCharIndex, '_');
					SetEntryName(m_scores[GameManager.Get.P2Index], tempName);
				}
			}
			else
			{
				// Update display name
				SetEntryName(m_scores[GameManager.Get.P2Index], GameManager.Get.HighNames[GameManager.Get.P2Index]);
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

				if((m_p1NameCharIndex >= 3) && (m_p2NameCharIndex >= 3))
					AllDone();
			}

		}
	}

	private void AllDone()
	{
		Text text = m_message.GetComponent<Text>();
		text.text = "Press any button to continue";
	}
}
