using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public TextMeshProUGUI GemCount;
    public GameObject gameOverPrefab;
    public GameObject MenuPrefab;

    public TextMeshProUGUI multiplierText;
    public TextMeshProUGUI GemsCollectedText;
    public TextMeshProUGUI FinalScore;


    int _gemsCollected = 0;
    private void Awake()
    {
        instance = this;
        MenuPrefab.SetActive(true);
    }
    public void startGame()
    {
        PlayerController.instance.startGame = true;
        MenuPrefab.SetActive(false);

    }
    public void LoadNextLevel()
    {
        gameOverPrefab.SetActive(false);
        int curLevel = PlayerPrefs.GetInt("CurrentLevel", 0);
        curLevel++;
        if (curLevel >= 3)
            curLevel = 0;

        PlayerPrefs.SetInt("CurrentLevel", curLevel);
        PlayerController.instance.startGame = true;
        PlayerController.instance.isgameOver = false;
        LevelLoader.Instance.InitLevel();
    }
    public void resetLevel()
    {
        LevelLoader.Instance.InitLevel();
        gameOverPrefab.transform.GetChild(0).gameObject.SetActive(false);
        gameOverPrefab.transform.GetChild(1).gameObject.SetActive(false);
        PlayerController.instance.isgameOver = false;
        _gemsCollected = 0;
    }
    public void GemCollected()
    {
        _gemsCollected++;
        GemCount.text = _gemsCollected.ToString();
    }
    void SaveGems(int gems)
    {
        int prevGems = PlayerPrefs.GetInt("Gems", 0);
        PlayerPrefs.SetInt("Gems", gems + prevGems);
    }
    public void GameOver()
    {
        gameOverPrefab.SetActive(true);
        gameOverPrefab.transform.GetChild(0).gameObject.SetActive(false);
        gameOverPrefab.transform.GetChild(1).gameObject.SetActive(true);
        PlayerController.instance.isgameOver = true;
    }
    public void ResetGame()
    {
        Debug.Log("ResetGame");
        PlayerPrefs.SetInt("CurrentLevel", 0);
        PlayerController.instance.startGame = true;
        PlayerController.instance.isgameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        LevelLoader.Instance.InitLevel();
    }
    internal void LevelComplete(int multiplier)
    {
        gameOverPrefab.SetActive(true);
        gameOverPrefab.transform.GetChild(0).gameObject.SetActive(true);
        gameOverPrefab.transform.GetChild(1).gameObject.SetActive(false);

        Debug.Log("Multiplier::::" + multiplier);
        PlayerController.instance.isgameOver = true;

        GemsCollectedText.text = _gemsCollected.ToString();
        multiplierText.text = "x" + multiplier.ToString();
        FinalScore.text = (_gemsCollected * multiplier).ToString();

        Debug.Log("GemCollected" + _gemsCollected + " Multiplier ::" + multiplier);

        SaveGems(_gemsCollected * multiplier);
    }
}
