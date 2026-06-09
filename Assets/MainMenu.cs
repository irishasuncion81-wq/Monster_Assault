using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public Image fadeImage; 
    public float fadeSpeed = 1f;

    void Start()
    {
    
        if (fadeImage != null)
        {
            fadeImage.gameObject.SetActive(true);
            fadeImage.canvasRenderer.SetAlpha(0f);
        }
    }

    public void OnClassicClick() { StartCoroutine(FadeAndLoad("classic")); }
    public void OnAdventureClick() { StartCoroutine(FadeAndLoad("adventure")); }
    public void OnShopClick() { StartCoroutine(FadeAndLoad("shop")); }

    IEnumerator FadeAndLoad(string sceneName)
    {
        if (fadeImage != null)
        {
       
            float alpha = 0f;
            while (alpha < 1f)
            {
                alpha += Time.deltaTime * fadeSpeed;
                fadeImage.canvasRenderer.SetAlpha(alpha);
                yield return null;
            }
        }

        SceneManager.LoadScene(sceneName);
    }

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}