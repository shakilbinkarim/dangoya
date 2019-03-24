using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamerPrefs
{
	public static int HighScore = 0;
	public static int MusicOn = 1;

    // 下のはｾﾀﾙｽです。
	public static void SetHighScore(int score)
	{
		PlayerPrefs.SetInt("HighScore", score);
	}

	public static void SetMusicOn(int truthValue)
	{
		PlayerPrefs.SetInt("MusicOn", truthValue);
	}

    public static void SetMaxLife(int life)
    {
        PlayerPrefs.SetInt("MaxLife", life);
    }

    public static void SetOkane(int okane) // お金はMoneyの日本語です。
    {
        PlayerPrefs.SetInt("Okane", okane);
    }

    public static void SetSlowTimeDuration(float slowTime)
    {
        PlayerPrefs.SetFloat("SlowTime", slowTime);
    }

    public static void SetShieldDuration(float shieldTime)
    {
        PlayerPrefs.SetFloat("ShieldTime", shieldTime);
    }

    // これからｹﾞﾀﾙｽだけです。

    public static int GetHighScore()
	{
		if (PlayerPrefs.HasKey("HighScore"))
		{
			return PlayerPrefs.GetInt("HighScore");
		}
		else
		{
			PlayerPrefs.SetInt("HighScore", 0);
			return PlayerPrefs.GetInt("HighScore");
		}
	}

	public static int GetMusicOn()
	{
		if (PlayerPrefs.HasKey("MusicOn"))
		{
			return PlayerPrefs.GetInt("MusicOn");
		}
		else
		{
			PlayerPrefs.SetInt("MusicOn", 1);
			return PlayerPrefs.GetInt("MusicOn");
		}
	}

    public static int GetMaxLife()
    {
        if (PlayerPrefs.HasKey("MaxLife"))
        {
            return PlayerPrefs.GetInt("MaxLife");
        }
        else
        {
            PlayerPrefs.SetInt("MaxLife", 3);
            return PlayerPrefs.GetInt("MaxLife");
        }
    }

    public static int GetOkane()
    {
        if (PlayerPrefs.HasKey("Okane"))
        {
            return PlayerPrefs.GetInt("Okane");
        }
        else
        {
            PlayerPrefs.SetInt("Okane", 0);
            return PlayerPrefs.GetInt("Okane");
        }
    }

    public static float GetSlowTimeDuration()
    {
        if (PlayerPrefs.HasKey("SlowTime"))
        {
            return PlayerPrefs.GetFloat("SlowTime");
        }
        else
        {
            PlayerPrefs.SetFloat("SlowTime", 5.0f);
            return PlayerPrefs.GetFloat("SlowTime");
        }
    }

    public static float GetShieldDuration()
    {
        if (PlayerPrefs.HasKey("ShieldTime"))
        {
            return PlayerPrefs.GetFloat("ShieldTime");
        }
        else
        {
            PlayerPrefs.SetFloat("ShieldTime", 5.0f);
            return PlayerPrefs.GetFloat("ShieldTime");
        }
    }

}
