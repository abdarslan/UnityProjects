using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Made public so other scripts can access it (e.g., AudioManager.instance.PlayNextTrack())
    public static AudioManager instance; 

    public AudioSource audioSource;
    public AudioClip[] playList;
    
    // Renamed for clarity so it doesn't conflict with Unity's internal audioSource.isPlaying
    private bool isManuallyPaused = false; 
    private int currentTrackIndex = 0;

    void Awake()
    {
        // Singleton pattern
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Safety check: Only play if the playlist has items and the AudioSource is assigned
        if (playList.Length > 0 && audioSource != null)
        {
            audioSource.clip = playList[0];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioManager: No audio clips in playlist or AudioSource is missing!");
        }
    }

    void Update()
    {
        // Exit early if setup is incomplete to prevent errors in the Update loop
        if (playList.Length == 0 || audioSource == null) return;

        // Toggle Pause/Play
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isManuallyPaused)
            {
                audioSource.UnPause();
                isManuallyPaused = false;
            }
            else
            {
                audioSource.Pause();
                isManuallyPaused = true;
            }
        } 
        // Skip forward
        else if (Input.GetKeyDown(KeyCode.N))
        {
            PlayNextTrack();
        }

        // Auto-play the next track when the current one finishes naturally.
        // We verify it isn't manually paused so the game doesn't skip tracks while muted.
        if (!audioSource.isPlaying && !isManuallyPaused)
        {
            PlayNextTrack();
        }
    }

    // Extracted the track-switching logic into its own method to avoid repeating code
    public void PlayNextTrack()
    {
        currentTrackIndex = (currentTrackIndex + 1) % playList.Length;
        audioSource.clip = playList[currentTrackIndex];
        audioSource.Play();
        isManuallyPaused = false; 
    }
}