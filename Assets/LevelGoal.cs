using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelGoal : MonoBehaviour
{
    public int levelToUnlock = 2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UnlockNextLevel();
            SceneManager.LoadScene("AdventureMenu");
        }
    }

    void UnlockNextLevel()
    {
        int currentReached = PlayerPrefs.GetInt("ReachedLevel", 1);
        if (levelToUnlock > currentReached)
        {
            PlayerPrefs.SetInt("ReachedLevel", levelToUnlock);
            PlayerPrefs.Save();
            Debug.Log("Level " + levelToUnlock + " Unlocked!");
        }
    }
}