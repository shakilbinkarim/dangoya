using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour {

    [SerializeField] private Player player; // Reference to player
    [SerializeField] private Text scoreText; // Text that shows player's current score
    [SerializeField] private Text okaneText; // Text that shows player's current Okane
    [SerializeField] private Text lifeText; // Text that shows players's current ife
    [SerializeField] private Text waveText; // Text that store messages to be displayed to player while ingame
    [SerializeField] private Image hungerBar; // The bar that shows player loosing time
    [SerializeField] private GameObject deathPanel; // Panel shown to player after death
    [SerializeField] private Text deathScoreText; // To show player final score after death
    [SerializeField] private Text deathSmallMessageText; // Stores message for player to be shown on death
    [SerializeField] private float hungerRate = 2.0f; // Stores the current rate at which player looses time
    [SerializeField] private AudioClip deathClip; // Stores death sound
    [SerializeField] private AudioClip explosionClip; // Stores explosion sound
    [SerializeField] private GameObject[] indicators; // Array to store indicator game objects
    [SerializeField] private Sprite[] indicatorSprites; // Array to store sprites for indicators
    [SerializeField] private string[] indicatorTags; // Array to store taggs of indicator sprites
    // Number of orders the plaayer needs to complete in order to get to the next wave
    [SerializeField] private int numberOfOrders;
    // The wave from which Time Slow powerup will start appearing from
    [SerializeField] private int timeSlowWave = 3;
    // The wave from which Advance 3 waves powerup will start appearing from
    [SerializeField] private int advanceSanWave = 4;
    
    private int score; // To keep track of players current score
    private float maxHP = 10; // Maximum time the player can have
    private float currentHP; // To keep track of current time left to the player
    private bool gameOn; // Flag to see if the game is live
    private AudioSource audioSource; // Audiosource of background music
    private bool willPlay; // Flag to see if music shall be played or not
    private int currentWave; // To keep track of the wave the player is currently in
    private float previousHungerRate; // Used to store current hunger rate b4 hunger rate is slowed down
    private bool isTimeSlowed; // flag to see if player is currently on slowed time
    private int life; // Stores the current number of lives the player has
    private int maxLife; // Stores the max number of lives the player can have -- gets it from GamePrefs
    private float timeSlow; // Duration during which player will loose time slowly
    private int okane;

    // Use this for initialization
    void Start ()
    {
        gameOn = true;
        score = 0;
        scoreText.text = "点　: " + score.ToString();
        okane = 0;
        okaneText.text = "金　：　" + okane.ToString();
        currentHP = maxHP;
        currentWave = 1;
        InitLife();
        InitMusic();
        timeSlow = GamerPrefs.GetSlowTimeDuration();
        MovingTingsFactory.DUNGO_FACTORY_WORKING = true;
        MovingTingsFactory.CAN_JUMP_WAVES = false;
        MovingTingsFactory.CAN_SLOW_HUNGER = false;
        isTimeSlowed = false;
        ShowCurrentWaveToPlayer();
        ResetIndicators();
    }

    private void InitLife()
    {
        maxLife = GamerPrefs.GetMaxLife();
        life = maxLife;
        lifeText.text = "心　：　" + life.ToString();
    }

    private void InitMusic()
    {
        audioSource = GetComponent<AudioSource>();
        int music = GamerPrefs.GetMusicOn(); // 悪い
        willPlay = (music == 1) ? true : false;
    }

    public void ResetIndicators()
    {
        int randomIndicatorIndex = UnityEngine.Random.Range(0, indicatorSprites.Length);
        for (int i = 0; i < indicators.Length; i++)
        {
            indicators[i].GetComponent<SpriteRenderer>().sprite = indicatorSprites[randomIndicatorIndex];
            indicators[i].tag = indicatorTags[randomIndicatorIndex];
            randomIndicatorIndex = UnityEngine.Random.Range(0, indicatorSprites.Length);
        }
    }

    internal void AddOkane()
    {
        okane += 1;
        okaneText.text = "金　：　" + okane.ToString();
    }

    internal void IncreaseLife()
    {
        if (life < maxLife)
        {
            life++;
            lifeText.text = "心　：　" + life.ToString();
        }
    }

    internal void DeductLife()
    {
        life--;
        lifeText.text = "心　：　" + life.ToString();
        if (life < 1)
        {
            GameOver();
        }
    }

    private void Update()
    {
        if (gameOn)
        {
            currentHP = currentHP - (hungerRate * Time.deltaTime);
            hungerBar.transform.localScale = new Vector3(1.0f, currentHP / maxHP, 1.0f);
            if (score >= numberOfOrders)
            {
                InitiateNextWave();
            }
            if (currentHP >= 6)
            {
                hungerBar.color = Color.green;
            }
            if (currentHP < 6)
            {
                hungerBar.color = Color.yellow;
            }
            if (currentHP < 3)
            {
                hungerBar.color = Color.red;
            }
            if (currentHP < 0)
            {
                GameOver();
            } 
        }
    }

    internal void AdvanceSanLevel()
    {
        gameOn = false;
        currentHP = maxHP;
        hungerBar.transform.localScale = new Vector3(1.0f, currentHP / maxHP, 1.0f);
        MovingTingsFactory.DUNGO_FACTORY_WORKING = false;
        for (int i = 0; i < 3; i++)
        {
            currentWave++;
            numberOfOrders = numberOfOrders + ((currentWave - 1) * numberOfOrders);
            if (!isTimeSlowed)
            {
                hungerRate += (hungerRate * ((currentWave - 1) / 4));
            }
            else
            {
                previousHungerRate += (previousHungerRate * ((currentWave - 1) / 4));
            }
        }
        if (hungerRate > 3.0f)
        {
            hungerRate = 3.0f;
        }
        player.EmptyDungoStick();
        ShowCurrentWaveToPlayer();
        StartCoroutine(AtarashiWaveGaHajimarimasu());
    }

    internal void SlowTime()
    {
        if (!isTimeSlowed)
        {
            isTimeSlowed = true;
            previousHungerRate = hungerRate;
            hungerRate = 0.3f;
            MovingTingsFactory.CAN_JUMP_WAVES = false;
            MovingTingsFactory.CAN_SLOW_HUNGER = false;
            ShowMessage("Slow Timer!");
            StartCoroutine(ReturnHugerRateToNormal(timeSlow)); 
        }
    }

    private IEnumerator ReturnHugerRateToNormal(float timeSlow)
    {
        yield return new WaitForSeconds(timeSlow);
        ShowMessage("No More Slow Timer!");
        hungerRate = previousHungerRate;
        isTimeSlowed = false;
        if (currentWave >= advanceSanWave) 
        {
            MovingTingsFactory.CAN_JUMP_WAVES = true; 
        }
        MovingTingsFactory.CAN_SLOW_HUNGER = true;
    }

    internal bool DungoMatch(string tag, int dangos)
    {
        if (tag == indicators[dangos].tag)
        {
            return true;
        }
        return false;
    }

    private void InitiateNextWave()
    {
        // TODO: Clear Level condition
        gameOn = false;
        currentHP = maxHP;
        hungerBar.transform.localScale = new Vector3(1.0f, currentHP / maxHP, 1.0f);
        MovingTingsFactory.DUNGO_FACTORY_WORKING = false;
        currentWave++;
        CheckIfFactoryCanProduceClocksAndArrows();
        numberOfOrders = numberOfOrders + ((currentWave - 1) * numberOfOrders);
        if (!isTimeSlowed)
        {
            hungerRate += (hungerRate * ((currentWave - 1) / 4)); 
        }
        else
        {
            previousHungerRate += (previousHungerRate * ((currentWave - 1) / 4));
        }
        if (hungerRate > 3.0f)
        {
            hungerRate = 3.0f;
        }
        player.EmptyDungoStick();
        ShowCurrentWaveToPlayer();
        StartCoroutine(AtarashiWaveGaHajimarimasu());
    }

    private void CheckIfFactoryCanProduceClocksAndArrows()
    {
        if (currentWave > advanceSanWave)
        {
            // To avoid changing static values all the time
        }
        else if (currentWave >= advanceSanWave)
        {
            MovingTingsFactory.CAN_JUMP_WAVES = true;
        }
        else if (currentWave >= timeSlowWave)
        {
            MovingTingsFactory.CAN_SLOW_HUNGER = true;
        }
    }

    private IEnumerator AtarashiWaveGaHajimarimasu()
    {
        yield return new WaitForSeconds(1.5f);
        //ResetIndicators();
        gameOn = true;
        MovingTingsFactory.DUNGO_FACTORY_WORKING = true;
    }

    // このコードは悪いですが便利です。今度時間があったらchangeしてください。
    private void ShowMessage(string message)
    {
        waveText.text = message;
        waveText.gameObject.GetComponent<Animator>().SetTrigger("newWave");
    }

    private void ShowCurrentWaveToPlayer()
    {
        waveText.text = "Wave " + currentWave;
        waveText.gameObject.GetComponent<Animator>().SetTrigger("newWave");
    }

    public void AddScore(int score)
    {
        this.score += score;
        scoreText.text = "点　: " + this.score;
    }

    public void AddHP(float hp)
    {
        currentHP += hp;
        if (currentHP > maxHP)
        {
            currentHP = maxHP;
        }
    }

    /// <summary>
    /// このコードでゲームが終わります
    /// </summary>
    public void GameOver()
    {
        if (willPlay)
        {
            audioSource.clip = deathClip;
            audioSource.Play(); 
        }
        Destroy(player.gameObject);
        gameOn = false;
        SetFinalScore(this.score);
    }

    internal void GameOverWithBang()
    {
        if (willPlay)
        {
            audioSource.clip = explosionClip; // TODO: change with explosion sound
            audioSource.Play();
        }
        gameOn = false;
        MovingTingsFactory.DUNGO_FACTORY_WORKING = false;
        GameObject bgMusicPlayer = GameObject.Find("BG Music Player");
        bgMusicPlayer.GetComponent<AudioSource>().Stop();
        StartCoroutine(WaitAndEndGame());
    }

    private IEnumerator WaitAndEndGame()
    {
        yield return new WaitForSeconds(0.8f);
        SetFinalScore(this.score);
    }

    public void QuitGame()
    {
        SceneManager.LoadScene("Main_Menu");
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetFinalScore(int score)
    {
        int waveScore = currentWave * 2;
        int servings = score;
        this.score = servings + waveScore;
        UpdateBankBalance();
        if (this.score < GamerPrefs.GetHighScore())
        {
            deathScoreText.text = "点　: " + this.score.ToString();
            deathSmallMessageText.text = "Servings: " + servings + "\n+\nWave Bonus: " + waveScore;
            deathPanel.SetActive(true);
        }
        else
        {
            // TODO: Test if this high score scene shit works
            GamerPrefs.SetHighScore(this.score);
            SceneManager.LoadScene("High_SCORE");
        }
    }

    private void UpdateBankBalance()
    {
        int bankBalance = GamerPrefs.GetOkane();
        bankBalance += okane;
        GamerPrefs.SetOkane(bankBalance);
    }
}
