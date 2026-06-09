using UnityEngine;
using UnityEngine.SceneManagement; 

public class MenuManager : MonoBehaviour
{
    public GameObject quitPopup; 


    public void OpenPopup()
    {
        quitPopup.SetActive(true);
        Time.timeScale = 0; 
    }


    public void ClosePopup()
    {
        quitPopup.SetActive(false);
        Time.timeScale = 1; 
    }

 
    public void QuitToMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
