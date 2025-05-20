using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraveyardAmbienceTrigger : MonoBehaviour
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
                GraveyardAmbience ambience = mainCamera.GetComponent<GraveyardAmbience>();
                if (ambience != null)
                {
                    Debug.Log("Camera found stop ambience");
                    ambience.StopAmbience(fadeOutDuration);
                }
            }
        }
    }
}
