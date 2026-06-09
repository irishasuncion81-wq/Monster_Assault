using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class adventure_menu : MonoBehaviour
{
    private string selectedSceneName = "";
    public float smoothSpeed = 10f;

    public GameObject[] startButtons;

    void Start()
    {
        int reachedLevel = PlayerPrefs.GetInt("ReachedLevel", 1);

        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject child = transform.GetChild(i).gameObject;
            Button btn = child.GetComponent<Button>();

            if (i < startButtons.Length && startButtons[i] != null)
            {
                startButtons[i].SetActive(false);
            }

            int levelNum = i + 1;

            if (levelNum > reachedLevel)
            {
                btn.interactable = false;
                child.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.4f);
            }
            else
            {
                btn.interactable = true;
                child.GetComponent<Image>().color = Color.white;
            }
        }
    }
    public void ZoomLevel(GameObject selectedLevel)
    {
        Button btn = selectedLevel.GetComponent<Button>();

        if (btn != null && !btn.interactable)
        {
            return;
        }

        int selectedIndex = selectedLevel.transform.GetSiblingIndex();

        if (selectedSceneName == selectedLevel.name)
        {
            selectedSceneName = "";
     
            if (selectedIndex < startButtons.Length && startButtons[selectedIndex] != null)
            {
                startButtons[selectedIndex].SetActive(false);
            }
            return;
        }

        selectedSceneName = selectedLevel.name;

 
        for (int i = 0; i < startButtons.Length; i++)
        {
            if (startButtons[i] != null)
            {
          
                startButtons[i].SetActive(i == selectedIndex);
            }
        }
    }
    public void StartTheGame(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }  


}