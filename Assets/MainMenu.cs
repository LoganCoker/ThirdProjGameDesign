using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float fadeOutDuration = 1.5f;

    public void PlayGame(){
        StartCoroutine(FadeOutAndLoadScene("Graveyard"));
    }

    public void Options(){
        
    }
   
   public void QuitGame(){
    Application.Quit();
   }

    private IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        if (audioSource != null && audioSource.clip != null && audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;
            float timer = 0; 

            while (timer < fadeOutDuration)
            {
                timer += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0, timer / fadeOutDuration);
                yield return null;
            }

            audioSource.volume = 0;
            audioSource.Stop(); 
        }

        SceneManager.LoadSceneAsync(sceneName); 
    }
}