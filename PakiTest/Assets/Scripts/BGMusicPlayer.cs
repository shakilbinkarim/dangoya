using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicPlayer : MonoBehaviour {

    private bool willPlay = false;

    private void Start ()
    {
        var music = GamerPrefs.GetMusicOn(); 
        willPlay = (music == 1) ? true : false;
        var audioSource = GetComponent<AudioSource>();
        if (willPlay)
        {
            if (audioSource.isPlaying) return;
            audioSource.Play();
        }
        else
        {
            if (!audioSource.isPlaying) return;
            audioSource.Stop();
        }
    }
	
}
