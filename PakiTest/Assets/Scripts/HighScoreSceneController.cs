using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HighScoreSceneController : MonoBehaviour
{
    [SerializeField] private AudioClip highScoreBgMusic;
    
    private Text highScore;

    public void PlayGame() => SceneManager.LoadScene("Loading_Scene");

    public void QuitGame() => SceneManager.LoadScene("Main_Menu");
    
    
    private void Start()
    {
        HandleHighScoreText();
        AudioPlayer.Instance.ForcePlayBackgroundMusic(highScoreBgMusic);
    }

    private void HandleHighScoreText()
    {
        var score = GamerPrefs.GetHighScore(); 
        highScore = GameObject.Find("High Score").GetComponent<Text>();
        highScore.text = "High Score: " + score;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) QuitGame();
    }
    
}
