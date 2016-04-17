using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Top-level game singleton
/// </summary>
public class GameManager : MonoBehaviour 
{
	// UI states
	public enum EGameState
	{
		k_splash,
		k_level,			
		k_death
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
	private int m_highScore = 0;
	private int m_score = 0;
	private int m_numPlayers = 2;
	private int m_playerOneNumber = 1;
	private int m_playerTwoNumber = 2;

	// Public methods
	public int Score { get { return m_score; } set { m_score = value; } }
	public int HighScore { get { return m_highScore; } }
	public int NumPlayers { get { return m_numPlayers; } }
	public int PlayerOneNumber { get { return m_playerOneNumber; } }
	public int PlayerTwoNumber { get { return m_playerTwoNumber; } }

	public void GameStart(int numPlayers, int playerOneNumber=1, int playerTwoNumber=2)
	{
		m_numPlayers = numPlayers;
		m_playerOneNumber = playerOneNumber;
		m_playerTwoNumber = playerTwoNumber;
		StateChange(EGameState.k_level);
	}

	public void GameOver()
	{
		// New high score?
		if(m_score > m_highScore)
		{
			m_highScore = m_score;
			PlayerPrefs.SetInt("Highscore", m_highScore);
		}

		StateChange(EGameState.k_splash);//k_death
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

		// Read high score
		m_highScore = PlayerPrefs.GetInt("Highscore", m_highScore);

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
			
			case EGameState.k_death:
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

			case EGameState.k_death:
			{
			}
			break;
		}
		
		m_gameState = newState;
	}
}
