using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; // Import ito para sa Scene loading

public class LevelManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject levelCompletePanel;
    public TextMeshProUGUI rewardText;

    void Start()
    {
     
        Time.timeScale = 1f;

        if (levelCompletePanel != null)
            levelCompletePanel.SetActive(false);
    }

    public void ShowLevelComplete()
    {
        if (levelCompletePanel != null)
        {
            levelCompletePanel.SetActive(true);
            if (rewardText != null)
            {
                rewardText.text = "Level Completed!\nNext Acquired!";
            }


            int currentLevel = SceneManager.GetActiveScene().buildIndex;


            int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);


            if (reachedLevel <= currentLevel)
            {
                PlayerPrefs.SetInt("ReachedLevel", currentLevel + 1);
                PlayerPrefs.Save();
            }

            Time.timeScale = 0f;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

  

    public void RestartLevel()
    {
        Time.timeScale = 1f; 

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;

        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;


        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("End of Levels!");
     
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit(); 
    }

    public LevelManager levelManager; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish")) 
        {
            levelManager.ShowLevelComplete();
        }
    }


}