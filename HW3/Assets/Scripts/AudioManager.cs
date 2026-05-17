using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioSource audioSource;
    public AudioClip[] playList;
    private bool isPlaying = false;
    int currentTrackIndex = 0;
    void Start()
    {
        audioSource.clip = playList[0];
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        // if there is press on M key, stop the music
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (isPlaying)
            {
                audioSource.Pause();
                isPlaying = false;
            }
            else
            {
                audioSource.UnPause();
                isPlaying = true;
            }
        } else if (Input.GetKeyDown(KeyCode.N))
        {
            currentTrackIndex = (currentTrackIndex + 1) % playList.Length;
            audioSource.clip = playList[currentTrackIndex];
            audioSource.Play();
            isPlaying = true;
        }
    }
}
