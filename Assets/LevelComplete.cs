using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour
{
    public int levelToUnlock;
    public GameObject levelCompletePanel;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (activeEnemies.Length > 0)
            {
                Debug.Log("Enemy left" + activeEnemies.Length);
                return; 
            }


            int reached = PlayerPrefs.GetInt("ReachedLevel", 1);
            if (levelToUnlock > reached)
            {
                PlayerPrefs.SetInt("ReachedLevel", levelToUnlock);
                PlayerPrefs.Save();
            }

            if (levelCompletePanel != null)
            {
                levelCompletePanel.SetActive(true);
            }
        }
    }

    public void GoToMenu()
    {
        SceneManager.LoadScene("AdventureMenu");
    }
}