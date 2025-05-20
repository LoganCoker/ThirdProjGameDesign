using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle2MusicTrigger : MonoBehaviour
{
    [SerializeField] private float fadeOutDuration = 1.5f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Find the camera and tell it to stop ambient sound
            Camera mainCamera = Camera.main;
            if (mainCamera != null)
            {
                SceneCastle2Ambience ambience = mainCamera.GetComponent<SceneCastle2Ambience>();
                if (ambience != null)
                {
                    ambience.StopAmbience(fadeOutDuration);
                }
            }
        }
    }
}
