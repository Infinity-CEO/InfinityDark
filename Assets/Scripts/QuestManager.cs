using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GoogleMobileAds.Api;

public class QuestManager : MonoBehaviour
{
    public Text currentKillUI;
    int currentKill;
    public int MaxKill;
    public GameObject result;

    
    public void Start()
    {
        currentKill = 0;
        Time.timeScale = 1f;
    }
    public void Update()
    {
        if (currentKill == MaxKill)
        {
            EndGames();
        }
    }
    public void SetScore(int value)
    {
        currentKill = value;
        currentKillUI.text = "" + currentKill;
    }
    public int GetScore()
    {
        return currentKill;
    }
    public void EndGames()
    {
        result.SetActive(true);
        Time.timeScale = 0f;
    }
}
