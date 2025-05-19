using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Start()
    {
       AudioManager.Instance.FadeIn("MenuWind", 2f); 
    }
    public void PlayGame(){
        AudioManager.Instance.FadeOut("MenuWind", 2f);

        StartCoroutine(LoadSceneWithDelay("Graveyard", 0.2f));
    }

    public void Options(){
        
    }
   
   public void QuitGame(){
    Application.Quit();
   }

   private IEnumerator LoadSceneWithDelay(string sceneName, float delay)
   {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadSceneAsync(sceneName); 
   }       
}