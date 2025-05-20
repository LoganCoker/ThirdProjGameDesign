using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject optionsPanel;


    public void Start()
    {
       AudioManager.Instance.FadeIn("TitleMusic", 2f); 
    }
    public void PlayGame(){
        AudioManager.Instance.FadeOut("TitleMusic", 2f);

        StartCoroutine(LoadSceneWithDelay("Graveyard", 0.2f));
    }

    public void Options()
    {
        AudioManager.Instance.FadeOut("MenuWind", 2f);
        mainMenuPanel.SetActive(false);
        optionsPanel.SetActive(true);
        
    }

    public void Back()
    {
        mainMenuPanel.SetActive(true);
        optionsPanel.SetActive(false);
        
    }
   
   public void QuitGame()
    {
        Application.Quit();
    }

   private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
   {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName); 
   }       
}