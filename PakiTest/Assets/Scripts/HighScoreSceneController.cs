using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreSceneController : MonoBehaviour
{
    private Text highScore;
    private bool willPlay = false;

    private void Start()
    {
        HandleHighScoreText();
        HandleHighSoreSceneBGMusic();
    }

    private void HandleHighScoreText()
    {
        // スコア
        int score = GamerPrefs.GetHighScore(); // 悪い
        highScore = GameObject.Find("High Score").GetComponent<Text>();
        highScore.text = "High Score: " + score;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            QuitGame();
        }
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Loading_Scene");
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void HandleHighSoreSceneBGMusic()
    {
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
