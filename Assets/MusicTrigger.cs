using UnityEngine;

public class MusicTrigger : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Collide");
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                SceneGraveyardAmbience ambience = mainCamera.GetComponent<SceneGraveyardAmbience>();
                if (ambience != null)
                {
                    Debug.Log("Camera found stop ambience");
                    ambience.StopAmbience(fadeOutDuration);
                }
            }
        }
    }
}
