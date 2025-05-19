using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public string sceneToLoad; // scene changer
    public Image blackScreen; // fade image
    private string doorCreakSoundName = "DoorCreak";
    private string doorSlamSoundName = "DoorSlam";

    [SerializeField] private float fadeSpeed = 0.7f; 
    [SerializeField] private float creakDuration = 3.5f; 

    bool loadIn = false;
    private bool isLoadingScene = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !loadIn)
        {
            AudioManager.Instance.Play(doorCreakSoundName);
            AudioManager.Instance.PlaySceneTransition(doorCreakSoundName, doorSlamSoundName);
            loadIn = true;
        }
    }

    void Update()
    {
        if (loadIn)
        {
            if (blackScreen.color.a < 1f)
            {
                Color temp = blackScreen.color;
                temp.a += Time.deltaTime * fadeSpeed;
                blackScreen.color = temp;
            }

            if (blackScreen.color.a > 0.95f && !isLoadingScene)
            {
                isLoadingScene = true;
                StartCoroutine(LoadSceneAfterDelay());
            }
        }
    }

    private IEnumerator LoadSceneAfterDelay()
    {
        float remainingTime = creakDuration - (1.0f / fadeSpeed);

        if (remainingTime > 0)
        {
            yield return new WaitForSeconds(remainingTime);
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}