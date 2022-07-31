using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;
    public TextMeshProUGUI GemCount;


    private void Awake()
    {
        instance = this;
    }
    public void resetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GemCollected()
    {
        int Gems = PlayerPrefs.GetInt("Gems", 0);
        Gems++;
        GemCount.text = Gems.ToString();
        PlayerPrefs.SetInt("Gems", Gems);
    }
}
