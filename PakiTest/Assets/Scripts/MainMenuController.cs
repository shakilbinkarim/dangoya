using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
public class MainMenuController : MonoBehaviour 
{

    [SerializeField] private GameObject optionsPanel;
    [SerializeField] private GameObject soundButton;

	private Text highScore, soundText;
	private bool willPlay = false;
    private bool optionsPanelVisible = false;

	private void Start()
	{
        optionsPanel.SetActive(false);
		HandleHighScoreText();
		HandleMusicButtonText();
#if UNITY_ANDROID
        AdManager.Instance.ShowBannerAd(); 
#endif
    }

    private void HandleMusicButtonText()
	{
		var music = GamerPrefs.GetMusicOn();
		soundText = soundButton.GetComponent<Text>();
		soundText.text = (music == 1) ? "Sound On" : "Sound Off";
		willPlay = (music == 1) ? true : false;
        HandleMainMenuBgMusic();
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
			Application.Quit();
		}
	}

	public void PlayGame()
	{
		SceneManager.LoadScene("Loading_Scene");
	}

    public void Credits()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=qCHT_lazERc"); // TODO: make it more civilized
    }

    public void HandleOptionsButton()
    {
        if (!optionsPanelVisible)
        {
            optionsPanel.SetActive(true);
            optionsPanelVisible = true;
        }
        else
        {
            optionsPanelVisible = false;
            optionsPanel.SetActive(false);
        }
    }

    public void HandleSound()
	{
        if (optionsPanelVisible)
        {
            optionsPanelVisible = false;
            optionsPanel.SetActive(false);
        }
		int music = GamerPrefs.GetMusicOn();
		if (music == 1)
		{
			music = 0;
			GamerPrefs.SetMusicOn(0);
		}
		else
		{
			music = 1;
			GamerPrefs.SetMusicOn(1);
		}
		Text soundText = soundButton.GetComponent<Text>();
		soundText.text = (music == 1) ? "Sound On" : "Sound Off";
		willPlay = (music == 1) ? true : false;
        HandleMainMenuBgMusic();
    }

	public void HandleMainMenuBgMusic()
	{
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

    public void HandleGooglePlay()
    {
        if (optionsPanelVisible)
        {
            optionsPanelVisible = false;
            optionsPanel.SetActive(false);
        }
        // TODO: Implement Google play leader 
        Debug.Log("Google Play!");
    }

    public void HandleMarketButton()
    {
        if (optionsPanelVisible)
        {
            optionsPanelVisible = false;
            optionsPanel.SetActive(false);
        }
        SceneManager.LoadScene("Market");
    }

}
