using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerPrefKeys
{
	public const string HighScore = "HighScore";
	public const string MusicOn = "MusicOn";
	public const string MaxLife = "MaxLife";
	public const string Money = "Money";
	public const string SlowTime = "SlowTime";
	public const string ShieldTime = "ShieldTime";
}

public static class GamerPrefs
{
    #region SettingPrefs
    
    public static void SetHighScore(int score) => PlayerPrefs.SetInt(PlayerPrefKeys.HighScore, score);

    public static void SetMusicOn(int truthValue) => PlayerPrefs.SetInt(PlayerPrefKeys.MusicOn, truthValue);

    public static void SetMaxLife(int life) => PlayerPrefs.SetInt(PlayerPrefKeys.MaxLife, life);

    public static void SetMoney(int money) => PlayerPrefs.SetInt(PlayerPrefKeys.Money, money);

    public static void SetSlowTimeDuration(float slowTime) => PlayerPrefs.SetFloat(PlayerPrefKeys.SlowTime, slowTime);

    public static void SetShieldDuration(float shieldTime) => PlayerPrefs.SetFloat(PlayerPrefKeys.ShieldTime, shieldTime);

    #endregion

    #region GettingPrefs

    public static int GetHighScore() => PlayerPrefs.GetInt(PlayerPrefKeys.HighScore, 0);

    public static int GetMusicOn() => PlayerPrefs.GetInt(PlayerPrefKeys.MusicOn, 1);

    public static int GetMaxLife() => PlayerPrefs.GetInt(PlayerPrefKeys.MaxLife, 3);

    public static int GetMoney() => PlayerPrefs.GetInt(PlayerPrefKeys.Money, 0);

    public static float GetSlowTimeDuration() => PlayerPrefs.GetFloat(PlayerPrefKeys.SlowTime, 5.0f);

    public static float GetShieldDuration() => PlayerPrefs.GetFloat(PlayerPrefKeys.ShieldTime, 5.0f);
    
    #endregion
}
