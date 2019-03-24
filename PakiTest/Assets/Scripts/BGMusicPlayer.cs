using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicPlayer : MonoBehaviour {

    private bool willPlay = false;
    
    // Use this for initialization
    void Start ()
    {
        // 音楽
        int music = GamerPrefs.GetMusicOn(); // 悪い
        willPlay = (music == 1) ? true : false;
        AudioSource audioSource = GetComponent<AudioSource>();
        if (willPlay)
        {
            if (!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
    }
	
}
