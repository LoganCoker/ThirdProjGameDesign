using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour{
    public string sceneToLoad; // scene changer
    public Image blackScreen; // fade image
    private string doorCreakSoundName = "DoorCreak";
    private string doorSlamSoundName = "DoorSlam";
    bool loadIn = false;

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.Play(doorCreakSoundName);

            AudioManager.Instance.PlaySceneTransition(doorCreakSoundName, doorSlamSoundName); 

            loadIn = true;
        }
    }


    void Update(){
        if (loadIn){
            if (blackScreen.color.a < 1f){
                Color temp = blackScreen.color;
                temp.a += Time.deltaTime * 0.7f;
                blackScreen.color = temp;
            }
            if (blackScreen.color.a > 0.95f)
                SceneManager.LoadScene(sceneToLoad);
        }
    }
}