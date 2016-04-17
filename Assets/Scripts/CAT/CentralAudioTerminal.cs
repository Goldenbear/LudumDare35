using UnityEngine;
using System.Collections;

public class CentralAudioTerminal : MonoBehaviour
{
    Object[] myMusic;
    public AudioClip intro;
    public AudioClip rampUp;
    public AudioClip[] chunks;
    //public AudioClip[] breaks;

    private AudioClip currentClip;
    private AudioClip nextClip;
    private AudioClip previousClip;

    public AudioSource[] audioChannel;
    private AudioSource currentChannel;
    private AudioSource nextChannel;
    private AudioSource previousChannel;

    //public bool loop = true;

    private float timer;
    private float currentAudioClipLength;
    private float nextAudioClipLength;
    private float previousAudioClipLength;

    private int iterator;
    public bool gameOver = false;

    void Awake()
    {
        currentChannel = audioChannel[0];
        nextChannel = audioChannel[1];

        currentChannel.clip = intro;
        currentAudioClipLength = currentChannel.clip.length;
        currentClip = currentChannel.clip;

        nextChannel.clip = rampUp;
        nextAudioClipLength = nextChannel.clip.length;
        nextClip = currentChannel.clip;
    }

    void Start()
    {
        //audio.Play();
        audioChannel[0].Play();
    }

    void Update()
    {
        if (chunks.Length > 0 && !gameOver)
        {
            // Increase timer with the time difference between this and the previous frame:
            timer += Time.deltaTime;

            if (timer >= currentAudioClipLength)
            {
                currentChannel.Stop();
                nextChannel.Play();
                currentAudioClipLength = nextAudioClipLength;
                timer = 0;

                //JuggleChannels prev/current/next
                previousChannel = currentChannel;
                previousClip = currentClip;
                previousAudioClipLength = currentAudioClipLength;
                
                currentChannel = nextChannel;
                currentClip = nextClip;
                //currentAudioClipLength = nextAudioClipLength;
                
                nextChannel = previousChannel;
                SetNextClip();
                nextChannel.clip = nextClip;
                nextAudioClipLength = nextClip.length;
            }
        }

        if (gameOver)
        {
            StopAllChannels();
            GameObject.Destroy(this.gameObject);
        }
    }

    void SetNextClip()
    {
        nextClip = chunks[Random.Range(0, chunks.Length)] as AudioClip;
        if (nextClip == currentClip)
        {
            SetNextClip();
        }
    }

    public void StopAllChannels()
    {
        currentChannel.Stop();
        nextChannel.Stop();
        if (previousChannel != null)
        {
            previousChannel.Stop();
        }
    }
}