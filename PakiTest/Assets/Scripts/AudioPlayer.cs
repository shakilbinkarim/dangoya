using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer Instance;

    [SerializeField] private AudioSource bgAudioSource;
    [SerializeField] private AudioSource sfxAudioSource;
    
    #region PlayingAudio

    public void PlayBackgroundMusic(AudioClip clip)
    {
        var willPlay = GamerPrefs.GetMusicOn() == 1 ? true : false;
        if (!willPlay || bgAudioSource.isPlaying) return;
        bgAudioSource.clip = clip;
        bgAudioSource.Play();
    }

    public void ForcePlayBackgroundMusic(AudioClip clip)
    {
        StopBackgroundMusic();
        var willPlay = GamerPrefs.GetMusicOn() == 1 ? true : false;
        if (!willPlay) return;
        bgAudioSource.clip = clip;
        bgAudioSource.Play();
    }
    
    public void PlaySfx(AudioClip clip)
    {
        StopSfx();
        var willPlay = GamerPrefs.GetMusicOn() == 1 ? true : false;
        if (!willPlay) return;
        sfxAudioSource.clip = clip;
        sfxAudioSource.Play();
    }

    #endregion

    #region StoppingAudio

    public void StopAll()
    {
        StopBackgroundMusic();
        StopAll();
    }
    
    public void StopBackgroundMusic() => bgAudioSource.Stop();

    public void StopSfx() => sfxAudioSource.Stop();

    #endregion
    
    private void Awake() => MakeThisSingleton();

    private void MakeThisSingleton()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
    }

    private void Start() => CheckAndInitAudioSources();

    private void CheckAndInitAudioSources()
    {
        if (!sfxAudioSource) Debug.LogError("SoundFx AudioSource has not been set!");
        if (!bgAudioSource) Debug.LogError("BackgroundMusic AudioSource has not been set!");
        sfxAudioSource.loop = false;
        bgAudioSource.loop = true;
    }
    
}
