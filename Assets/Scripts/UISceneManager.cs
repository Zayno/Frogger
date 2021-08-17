using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISceneManager : MonoBehaviour
{
    public Text LevelNumberText;
    public Text WinText;
    public Text DeathText;

    private static UISceneManager _instance;

    public static UISceneManager Instance { get { return _instance; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    void Start()
    {
        UpdateTexts();
    }

    public void UpdateTexts()
    {
        LevelNumberText.text = "Level " + SaveData.CurrentLevelNumber.ToString();
        WinText.text = "Wins: " + SaveData.TotalGameWins.ToString();
        DeathText.text = "Deaths: " + SaveData.TotalDeaths.ToString();
    }

}
