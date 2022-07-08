using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class AdmobManager : MonoBehaviour
{
    public bool isTestMode;
    //public Text LogText;
    //public Button FrontAdsBtn;, RewardAdsBtn;


    void Start()
    {
        var requestConfiguration = new RequestConfiguration
           .Builder()
           .SetTestDeviceIds(new List<string>() { "" }) // test Device ID
           .build();

        MobileAds.SetRequestConfiguration(requestConfiguration);

        LoadBannerAd();
        //LoadFrontAd();
        //LoadRewardAd();
    }

    void Update()
    {
        //FrontAdsBtn.interactable = frontAd.IsLoaded();
        //RewardAdsBtn.interactable = rewardAd.IsLoaded();
    }

    AdRequest GetAdRequest()
    {
        return new AdRequest.Builder().Build();
    }



    #region ��� ����
    const string bannerTestID = "ca-app-pub-3940256099942544/6300978111";
    const string bannerID = "ca-app-pub-8084521993576912/2516032251";
    BannerView bannerAd;


    public void LoadBannerAd()
    {
        bannerAd = new BannerView(isTestMode ? bannerTestID : bannerID,
            AdSize.SmartBanner, AdPosition.BottomLeft);
        bannerAd.LoadAd(GetAdRequest());
        ToggleBannerAd(false);
    }

    public void ToggleBannerAd(bool b)
    {
        if (b) bannerAd.Show();
        else bannerAd.Hide();
    }
    #endregion



    /*#region ���� ����
    const string frontTestID = "ca-app-pub-3940256099942544/8691691433";
    const string frontID = "ca-app-pub-8084521993576912/2132888875";
    InterstitialAd frontAd;


    void LoadFrontAd()
    {
        frontAd = new InterstitialAd(isTestMode ? frontTestID : frontID);
        frontAd.LoadAd(GetAdRequest());
        frontAd.OnAdClosed += (sender, e) =>
        {
            LogText.text = "���鱤�� ����";
        };
    }

    public void ShowFrontAd()
    {
        frontAd.Show();
        LoadFrontAd();
    }
    #endregion*/



    #region ������ ����
    /*const string rewardTestID = "ca-app-pub-3940256099942544/5224354917";
    const string rewardID = "";
    RewardedAd rewardAd;


    void LoadRewardAd()
    {
        rewardAd = new RewardedAd(isTestMode ? rewardTestID : rewardID);
        rewardAd.LoadAd(GetAdRequest());
        rewardAd.OnUserEarnedReward += (sender, e) =>
        {
            LogText.text = "������ ���� ����";
        };
    }

    public void ShowRewardAd()
    {
        rewardAd.Show();
        LoadRewardAd();
    }*/
    #endregion
}