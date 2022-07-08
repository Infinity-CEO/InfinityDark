using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private SaveAndLoad theSaveNLoad;

    public GameObject dead;

    public static GameManager gm;
    private void Awake()
    {
        if (gm == null)
        {
            gm = this;
        }
    }
    public enum GameState
    {
        Ready,
        Start,
        Pause,
        GameOver
    }
    public GameState gState;
    public GameObject gameLabel;
    Text gameText;

    public Canvas mc;
    Controller player;
    public GameObject gameOption;
    public GameObject wait;
    public GameObject settingButton;

    private InterstitialAd interstitial;
    private void RequestInterstitial()
    {
#if UNITY_ANDROID
        string adUnitId = "ca-app-pub-8084521993576912/2132888875";//Real
        //string adUnitId = "ca-app-pub-3940256099942544/1033173712";//Sample
#elif UNITY_IPHONE
        string adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
        string adUnitId = "unexpected_platform";
#endif

        // Initialize an InterstitialAd.
        this.interstitial = new InterstitialAd(adUnitId);
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the interstitial with the request.
        this.interstitial.LoadAd(request);
    }
    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        SceneManager.LoadScene(0);
    }
    void Start()
    {
        gState = GameState.Ready;
        gameText = gameLabel.GetComponent<Text>();
        gameText.text = "Are you ready?";
        gameText.color = new Color32(255, 185, 0, 255);
        StartCoroutine(ReadyToStart());
        player = GameObject.Find("Player").GetComponent<Controller>();
    }
    IEnumerator ReadyToStart()
    {
        yield return new WaitForSeconds(2f);
        gameText.text = "Mission : Clean up all zombies";
        yield return new WaitForSeconds(2f);
        gameLabel.SetActive(false);
        gState = GameState.Start;
    }
    void Update()
    {
        if (player.hp <= 0)
        {
            player.GetComponentInChildren<Animator>().SetFloat("Movement", 0f);
            gameLabel.SetActive(true);
            gameText.text = "The End";
            gameText.color = new Color32(255, 0, 0, 255);
            Transform buttons = gameText.transform.GetChild(0);
            buttons.gameObject.SetActive(true);
            gState = GameState.GameOver;
        }
    }
    public void Dead()
    {
        if (player.hp == 0)
        {
            dead.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void WaitClose()
    {
        wait.SetActive(false);
        Time.timeScale = 1f;
    }
    public void OpenOptionWindow()
    {
        settingButton.SetActive(false);
        gameOption.SetActive(true);
        Time.timeScale = 0f;
        gState = GameState.Pause;
    }
    public void CloseOptionWindow()
    {
        settingButton.SetActive(true);
        gameOption.SetActive(false);
        Time.timeScale = 1f;
        gState = GameState.Start;
    }
    public void RestartBRGGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
    }
    public void RestartHUSGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(4);
    }
    public void RestartFTR1Game()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(6);
    }
    public void RestartFTR2Game()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(8);
    }
    public void QuitGame()
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
}
