using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//////////////////////////////////////////////////////////////////////////
// Settings and logic for a single game level
//////////////////////////////////////////////////////////////////////////
public class LevelManager : MonoBehaviour 
{
	// Constants
	public enum ELevelState
	{
		k_intro,
		k_play,			
		k_pause,
		k_gameover
	}

	// Sub-classes

	// A sound clip + configuration
	[System.Serializable]
	public class SoundConfig
	{
		public AudioClip 			m_clip = null;
		public float 				m_volume = 1.0f;
		public bool					m_loop = false;
		public bool					m_playOnAwake = false;

		public void Play(AudioSource source)
		{
			source.loop = m_loop;
			source.playOnAwake = m_playOnAwake;
			source.clip = m_clip;
			source.volume = m_volume;
			source.Play();
		}
	}

	// Globals
	private static LevelManager g_instance;

	public static LevelManager Get { get { return g_instance; } }

	// Public members
	public SoundConfig				m_introSound;
	public SoundConfig				m_backgroundSound;
	public SoundConfig				m_gameOverSound;

    public GameObject JoshSpawnPrefab;
    public GameObject SeanSpawnPrefab;
    public GameObject SpencerSpawnPrefab;
    public GameObject TomSpawnPrefab;

    public Player[] Players { get { return m_players; } }

	// Private members
	private ELevelState 			m_levelState = ELevelState.k_intro;
	private Player[]				m_players;

	private Text 					m_messageText;
	private Text 					m_message2Text;
	private float 					m_messageTimer = 0.0f;

	private AudioSource 			m_backgroundSource = null;
	private AudioSource 			m_gameOverSource = null;

    [SerializeField]
    SpawnDirector activeDirector;

	/// <summary>
	/// Constructor
	/// </summary>
	private void Awake()
	{
		if(g_instance == null)
			g_instance = this;

		// Find players
		m_players = FindObjectsOfType(typeof(Player)) as Player[];

		// Ensure players are in player number order
		if((m_players.Length == 2) && (m_players[0].m_playerNumber == 1))
		{
			Player temp = m_players[0];
			m_players[0] = m_players[1];
			m_players[1] = temp;
		}

		// Deactivate unwanted players
		for(int i=0; i<m_players.Length; i++)
			if(m_players[i].m_playerNumber >= GameManager.Get.NumPlayers)
				m_players[i].Deactivate();

		// Find UI elements
		Text[] texts = FindObjectsOfType(typeof(Text)) as Text[];
		for(int i=0; i<texts.Length; i++)
		{
			if(texts[i].gameObject.name == "Message")
				m_messageText = texts[i];
			if(texts[i].gameObject.name == "Message2")
				m_message2Text = texts[i];
		}

		// Create audio sources
		m_backgroundSource = gameObject.AddComponent<AudioSource>();
		m_gameOverSource = gameObject.AddComponent<AudioSource>();

		// Start intro sound
		m_introSound.Play(m_backgroundSource);

		// Start intro
		StateChange(ELevelState.k_intro);
	}

    void Start()
    {
        if(activeDirector == null)
        {
            ChooseNewDirector();
        }
    }

	/// <summary>
	/// Update level
	/// </summary>
	private void Update()
	{
		// Update level state
		switch(m_levelState)
		{
			case ELevelState.k_intro:
			{
				StateChange(ELevelState.k_play);
			}
			break;
			
			case ELevelState.k_play:
			{
				int numPlayersDead = 0;

				// Level is over when all players are dead or inactive
				for(int i=0; i<m_players.Length; i++)
				{
					if((m_players[i].m_currentHealth <= 0) || !m_players[i].gameObject.activeInHierarchy)
						numPlayersDead++;
				}

				// Is level over?
				if(numPlayersDead == m_players.Length)
					StateChange(ELevelState.k_gameover);

				// Update time and score
				UpdateUI();

				// Once intro has completed loop background sound
				if(!m_backgroundSource.isPlaying)
					m_backgroundSound.Play(m_backgroundSource);

                if(activeDirector.IsComplete)
                {
                    ChooseNewDirector();
                }
			}
			break;

			case ELevelState.k_gameover:
			{				
				// Go to leaderboard screen
				if(Time.time > m_messageTimer)
					GameManager.Get.GameOver();
			}
			break;

			default:
			break;
		}
	}

	/// <summary>
	/// Change level state
	/// </summary>
	/// <param name="newState">New state.</param>
	private void StateChange(ELevelState newState)
	{
		switch(newState)
		{
			case ELevelState.k_intro:
			{
				// Initialise level
				GameManager.Get.P1Score = 0;
				GameManager.Get.P2Score = 0;

				UpdateUI();

				if(m_messageText != null)
					m_messageText.text = "";
				if(m_message2Text != null)
					m_message2Text.text = "";
			}
			break;
				
			case ELevelState.k_play:
			{
				if(m_messageText != null)
					m_messageText.text = "";
				if(m_message2Text != null)
					m_message2Text.text = "";
			}
			break;

			case ELevelState.k_gameover:
			{	
				// Pass scores to GameManager so they can be inserted into the leaderboard
				GameManager.Get.P1Score = m_players[0].m_score;
				GameManager.Get.P2Score = m_players[1].m_score;

				//?
				m_gameOverSound.Play(m_gameOverSource);

				if(m_messageText != null)
				{
					m_messageText.text = "Game Over";
					m_messageTimer = Time.time + 3.0f; 
				}
			}
			break;

			default:
			break;
		}
		
		m_levelState = newState;
	}

	/// <summary>
	/// Update UI text strings
	/// </summary>
	private void UpdateUI()
	{
		// Message clear time out?
		if(m_messageTimer > 0.0f)
		{
			if(Time.time > m_messageTimer)
			{
				if(m_messageText != null)
					m_messageText.text = "";
				if(m_message2Text != null)
					m_message2Text.text = "";
				m_messageTimer = 0.0f;
			}
		}
	}

    void ChooseNewDirector()
    {
        if(activeDirector != null)
        {
            Destroy(activeDirector.gameObject);
        }

        var d = new GameObject[] { JoshSpawnPrefab, SeanSpawnPrefab, SpencerSpawnPrefab, TomSpawnPrefab };
        int index;

        do
        {
            index = Random.Range(0, 4);
        } while (d[index] == null);


        Debug.LogFormat("<color=purple>Started Director:</color> {0}", d[index].name);
        activeDirector = Instantiate<GameObject>(d[index]).GetComponent<SpawnDirector>();
    }

}
