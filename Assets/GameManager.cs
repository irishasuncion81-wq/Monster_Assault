using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro; // REQUIRED: Para sa text ng kills at highscore

public class GameManager : MonoBehaviour
{
    public GameObject gameOverPanel;
    public GameObject backButton;

    [Header("Score Settings")]
    public TextMeshProUGUI killCountText;     
    public TextMeshProUGUI highHighScoreText;
    private int currentKills = 0;
    public TextMeshProUGUI hudKillText;

    void Start()
    {
        Time.timeScale = 1f; 
    }


    public void AddKill()
    {
        currentKills++;
        Debug.Log("Current Kills in Manager: " + currentKills);
        UpdateHUD();
    }

    void UpdateHUD()
    {
        if (hudKillText != null)
        {
            hudKillText.text = "Kills: " + currentKills;
        }
    }

    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);

        if (backButton != null)
        {
            backButton.SetActive(false);
        }


        int highScore = PlayerPrefs.GetInt("HighScore", 0);

        if (currentKills > highScore)
        {
            highScore = currentKills;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        if (killCountText != null) killCountText.text = "Kills: " + currentKills;
        if (highHighScoreText != null) highHighScoreText.text = "High Score: " + highScore;
  

        Time.timeScale = 0f;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}