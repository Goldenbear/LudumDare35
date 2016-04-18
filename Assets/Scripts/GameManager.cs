using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Top-level game singleton
/// </summary>
public class GameManager : MonoBehaviour 
{
	public const int k_numHighScores = 10;

	// UI states
	public enum EGameState
	{
		k_splash,
		k_level,			
		k_leaderboard
	}

	// Globals
	private static GameManager g_instance;

	/// <summary>
	/// Singleton instance getter
	/// </summary>
	/// <value>The get.</value>
	public static GameManager Get
	{
		get
		{
			if(g_instance == null)
			{
				g_instance = FindObjectOfType(typeof(GameManager)) as GameManager;

				if(g_instance == null)
				{
					g_instance = new GameObject("GameManager").AddComponent<GameManager>();
				}
			}

			return g_instance;
		}
	}

	// Public members

	// Private members
	private EGameState m_gameState = EGameState.k_splash;
	private int m_numPlayers = 2;
	private int m_playerOneInputNumber = 1;
	private int m_playerTwoInputNumber = 2;
	private int[] m_highScores;
	private string[] m_highNames;
	private string m_p1Name = "AAA";
	private string m_p2Name = "AAA";
	private int m_p1Score = 0;
	private int m_p2Score = 0;
	private int m_p1HighIndex = -1;
	private int m_p2HighIndex = -1;

	// Public methods
	public int[] HighScores { get { return m_highScores; } }
	public string[] HighNames { get { return m_highNames; } }
	public int NumPlayers { get { return m_numPlayers; } }
	public int PlayerOneInputNumber { get { return m_playerOneInputNumber; } }
	public int PlayerTwoInputNumber { get { return m_playerTwoInputNumber; } }
	public int P1Score { get { return m_p1Score; } set { m_p1Score = value; } }
	public int P2Score { get { return m_p2Score; } set { m_p2Score = value; } }
	public int P1Index { get { return m_p1HighIndex; } }
	public int P2Index { get { return m_p2HighIndex; } }

	// Start game specifying which input is assigned to which player
	public void GameStart(int numPlayers, int playerOneInputNumber=1, int playerTwoInputNumber=2)
	{
		m_numPlayers = numPlayers;
		m_playerOneInputNumber = playerOneInputNumber;
		m_playerTwoInputNumber = playerTwoInputNumber;
		StateChange(EGameState.k_level);
	}

	public void GameOver()
	{
		// New high scores?
		if(m_p1Score > m_p2Score)
		{
			m_p1HighIndex = AddScore(0, m_p1Score);
			m_p2HighIndex = AddScore(1, m_p2Score);
		}
		else
		{
			m_p2HighIndex = AddScore(1, m_p2Score);
			m_p1HighIndex = AddScore(0, m_p1Score);
		}

		StateChange(EGameState.k_leaderboard);
	}

	public void GameRestart()
	{
		StateChange(EGameState.k_splash);
	}

	public int AddScore(int playerNum, int score)
	{
		int insertIndex = -1;

		for(int i=0; i<k_numHighScores; i++)
		{
			if(score > m_highScores[i])
			{
				insertIndex = i;
				break;
			}
		}

		if(insertIndex == -1)
			return insertIndex;

		// Shift scores up
		for(int j=k_numHighScores-1; j>insertIndex; j--)
		{
			m_highScores[j] = m_highScores[j-1];
			m_highNames[j] = m_highNames[j-1];
		}

		// Insert new score
		m_highScores[insertIndex] = score;
		m_highNames[insertIndex] = (playerNum == 0) ? m_p1Name : m_p2Name;

		return insertIndex;
	}

	public void SetScoreName(int playerNum, string name, int scoreIndex)
	{
		m_highNames[scoreIndex] = name;

		if(playerNum == 0)
		{
			m_p1Name = name;
			PlayerPrefs.SetString("P1Name", m_p1Name);
		}
		else
		{
			m_p2Name = name;
			PlayerPrefs.SetString("P2Name", m_p2Name);
		}

		// Save high score table
		for(int i=0; i<k_numHighScores; i++)
		{
			if((m_highScores[i] > 0) && (m_highNames[i] != null))
			{
				PlayerPrefs.SetInt("HighScore"+i, m_highScores[i]);
				PlayerPrefs.SetString("HighName"+i, m_highNames[i]);
			}
		}
	}

	// Private methods

	/// <summary>
	/// Constructor
	/// </summary>
	private void Awake()
	{
		// If a GameManager instance already exists destroy this one
		if(g_instance != null)
		{
			enabled = false;
			Destroy(this);
			return;
		}

		// This is our instance
		g_instance = this;
		DontDestroyOnLoad(g_instance.gameObject);
		DontDestroyOnLoad(g_instance);

		// Read high scores
		m_highScores = new int[k_numHighScores];
		m_highNames = new string[k_numHighScores];
		for(int i=0; i<k_numHighScores; i++)
		{
			m_highScores[i] = PlayerPrefs.GetInt("HighScore"+i, 0);
			m_highNames[i] = PlayerPrefs.GetString("HighName"+i, null);
		}

		// Read default player names
		m_p1Name = PlayerPrefs.GetString("P1Name", m_p1Name);
		m_p2Name = PlayerPrefs.GetString("P2Name", m_p2Name);

		// Start state depends on which scene we started on
		if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Splash")
			StateChange(EGameState.k_splash);
		else
			StateChange(EGameState.k_level);
	}

	/// <summary>
	/// Update Game state
	/// </summary>
	private void Update()
	{
		switch(m_gameState)
		{
			case EGameState.k_splash:
			{
				if(Input.GetKeyDown(KeyCode.Escape))
				{
					Application.Quit();
				}
			}
			break;
			
			case EGameState.k_level:
			{
			}
			break;
			
			case EGameState.k_leaderboard:
			{
			}
			break;
		}
	}

	/// <summary>
	/// Change Game state
	/// </summary>
	/// <param name="newState">New state.</param>
	private void StateChange(EGameState newState)
	{
		switch(newState)
		{
			case EGameState.k_splash:
			{
				if(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "Splash")
					UnityEngine.SceneManagement.SceneManager.LoadScene("Splash");
			}
			break;

			case EGameState.k_level:
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("Gameplay");
			}
			break;

			case EGameState.k_leaderboard:
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("Leaderboard");
			}
			break;
		}
		
		m_gameState = newState;
	}
}
