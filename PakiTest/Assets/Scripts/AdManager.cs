using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using admob;

public class AdManager : MonoBehaviour {

    public static AdManager Instance { get; set; }

    [SerializeField] private string bannerID;
    [SerializeField] private string interstetialID;
    
    // Use this for initialization
    void Start ()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
#if UNITY_EDITOR
#elif UNITY_ANDROID
        Admob.Instance().initAdmob(bannerID, interstetialID);
#endif
    }

    public void ShowBannerAd()
    {
#if UNITY_EDITOR
#elif UNITY_ANDROID
        Admob.Instance().showBannerRelative(AdSize.Banner, AdPosition.TOP_CENTER, 0);
#endif
    }

}
