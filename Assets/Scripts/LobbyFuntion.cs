using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class LobbyFuntion : MonoBehaviour
{
    private SaveAndLoad theSaveNLoad;

    public static LobbyFuntion instance;
    public GameObject selectWindow;
    public GameObject CoomingSoon;

    private InterstitialAd interstitial;

    public GameObject wait;

    public void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8084521993576912/2132888875";//real
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";//Sample
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Called when the ad is closed.
       // this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
    /*public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        
    }*/
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    public void Donate()
    {
        RequestInterstitial();

        wait.SetActive(true);

        StartCoroutine(showInterstitial());

        IEnumerator showInterstitial()
        {
            while (!this.interstitial.IsLoaded())
            {
                Debug.Log("Not yet. You nees wait.");
                yield return new WaitForSeconds(0.2f);
            }
            wait.SetActive(false);
            this.interstitial.Show();
        }
    }
    public void OpenCoominsoon()
    {
        CoomingSoon.SetActive(true);
        RequestInterstitial();
        //When you want call Interstitial show
        StartCoroutine(showInterstitial());

        IEnumerator showInterstitial()
        {
            while (!this.interstitial.IsLoaded())
            {
                Debug.Log("Not yet. You nees wait.");
                yield return new WaitForSeconds(0.2f);
            }
            this.interstitial.Show();
        }
    }
    public void CloseCoominsoon()
    {
        CoomingSoon.SetActive(false);
    }
    public void OpenSelectChapter() //PlayBT
    {
        selectWindow.SetActive(true);
        RequestInterstitial();
        //When you want call Interstitial show
        StartCoroutine(showInterstitial());

        IEnumerator showInterstitial()
        {
            while (!this.interstitial.IsLoaded())
            {
                Debug.Log("Not yet. You nees wait.");
                yield return new WaitForSeconds(0.2f);
            }
            this.interstitial.Show();
        }
    }
    public void CloseSelectChapter() //PlayBT
    {
        RequestInterstitial();
        //When you want call Interstitial show
        StartCoroutine(showInterstitial());

        IEnumerator showInterstitial()
        {
            while (!this.interstitial.IsLoaded())
            {
                Debug.Log("Not yet. You nees wait.");
                yield return new WaitForSeconds(0.2f);
            }
            this.interstitial.Show();
        }
        selectWindow.SetActive(false);

    }
    public void BridgeScene() //Select
    {
        SceneManager.LoadScene(1);
    }
    public void MansionScene()
    {
        SceneManager.LoadScene(3);
    }
    public void FactoryScene()
    {
        SceneManager.LoadScene(6);
    }
    public void AdventureScene()
    {
        SceneManager.LoadScene(8);
    }
    public void LoadOpenWorldScene()
    {
        Debug.Log("Load");
        StartCoroutine(LoadCoroutine());
    }
    IEnumerator LoadCoroutine()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(7);

        while (!operation.isDone)
        {
            yield return null;
        }
        theSaveNLoad = FindObjectOfType<SaveAndLoad>();
        theSaveNLoad.LoadData();
    }
}
