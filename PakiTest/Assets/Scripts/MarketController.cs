using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketController : MonoBehaviour
{
    private const float TimeSlowCap = 15.0f;
    private const float ShieldDurationCap = 15.0f;
    private const int MaxLifeCap = 8;
    
    private float timeSlow;
    private int timeSlowUpgrCost;
    [SerializeField] Text timeSlowAmountText;
    [SerializeField] Text timeSlowCostText;
    [SerializeField] Text timeSlowButtonText;
    private float shieldDuration;
    private int shildDurationUpgrCost;
    [SerializeField] Text shieldDurationAmountText;
    [SerializeField] Text shieldDurationCostText;
    [SerializeField] Text shieldDurationButtonText;
    private int maxLife;
    private int maxLifeUpgrCost;
    [SerializeField] Text maxLifeAmountText;
    [SerializeField] Text maxLifeCostText;
    [SerializeField] Text maxLifeButtonText;
    private int balance;
    private Text okaneBalance;
    private bool willPlay = false;

    private void Start()
    {
        HandleTimeSlow();
        HandleShield();
        HandleMaxLife();
        HandleOkaneBalance();
        HandleMarketSceneBgMusic();
    }

    private void HandleMaxLife()
    {
        maxLife = GamerPrefs.GetMaxLife();
        maxLifeAmountText.text = "心 : " + maxLife.ToString();
        if (maxLife < MaxLifeCap)
        {
            maxLifeUpgrCost = ((maxLife * 2) * (maxLife * 2)) / 2;
            maxLifeCostText.text = "Upgrade Cost 金: " + maxLifeUpgrCost.ToString();
        }
        else
        {
            maxLifeUpgrCost = int.MaxValue;
            maxLifeCostText.text = "Fully Upgraded";
            maxLifeButtonText.text = "X";
        }
    }

    private void HandleShield()
    {
        shieldDuration = GamerPrefs.GetShieldDuration();
        shieldDurationAmountText.text = "強 : " + shieldDuration.ToString();
        if (shieldDuration < ShieldDurationCap)
        {
            shildDurationUpgrCost = (int)((shieldDuration * shieldDuration) / 2);
            shieldDurationCostText.text = "Upgrade Cost 金: " + shildDurationUpgrCost.ToString();
        }
        else
        {
            shildDurationUpgrCost = int.MaxValue;
            shieldDurationCostText.text = "Fully Upgraded";
            shieldDurationButtonText.text = "X";
        }
    }

    private void HandleTimeSlow()
    {
        timeSlow = GamerPrefs.GetSlowTimeDuration();
        timeSlowAmountText.text = "時 : " + timeSlow.ToString();
        if (timeSlow < TimeSlowCap)
        {
            timeSlowUpgrCost = (int)((timeSlow * timeSlow) / 2);
            timeSlowCostText.text = "Upgrade Cost 金: " + timeSlowUpgrCost.ToString();
        }
        else
        {
            timeSlowUpgrCost = int.MaxValue;
            timeSlowCostText.text = "Fully Upgraded";
            timeSlowButtonText.text = "X";
        }
    }

    private void HandleOkaneBalance()
    {
        balance = GamerPrefs.GetOkane(); // 悪い
        okaneBalance = GameObject.Find("Balance").GetComponent<Text>();
        okaneBalance.text = "Coin 金 :  " + balance;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape)) QuitGame();
    }

    public void QuitGame() => UnityEngine.SceneManagement.SceneManager.LoadScene("Main_Menu");

    public void HandleMarketSceneBgMusic()
    {
        var music = GamerPrefs.GetMusicOn(); // 悪い
        willPlay = (music == 1) ? true : false;
        var audioSource = GetComponent<AudioSource>();
        if (willPlay)
        {
            if (!audioSource.isPlaying) audioSource.Play();
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
        }
    }

    public void UpgradeTimeSlow()
    {
        var canUpgradeSlowTime = timeSlow >= TimeSlowCap || balance < timeSlowUpgrCost;
        if (canUpgradeSlowTime) return;
        balance -= timeSlowUpgrCost;
        timeSlow += 2;
        GamerPrefs.SetSlowTimeDuration(timeSlow);
        GamerPrefs.SetOkane(balance);
        HandleOkaneBalance();
        HandleTimeSlow();
    }

    public void UpgradeMaxLife()
    {
        var canUpgradeLife = maxLife >= MaxLifeCap || balance < maxLifeUpgrCost;
        if (canUpgradeLife) return;
        balance -= maxLifeUpgrCost;
        maxLife += 1;
        GamerPrefs.SetMaxLife(maxLife);
        GamerPrefs.SetOkane(balance);
        HandleOkaneBalance();
        HandleMaxLife();
    }

    public void UpgradeShield()
    {
        var canUpgradeShield = shieldDuration >= ShieldDurationCap || balance < shildDurationUpgrCost;
        if (canUpgradeShield) return;
        balance -= shildDurationUpgrCost;
        shieldDuration += 2;
        GamerPrefs.SetShieldDuration(shieldDuration);
        GamerPrefs.SetOkane(balance);
        HandleOkaneBalance();
        HandleShield();
    }

}
