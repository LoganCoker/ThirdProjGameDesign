using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle1MusicTrigger : MonoBehaviour
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
                SceneCastle1Ambience ambience = mainCamera.GetComponent<SceneCastle1Ambience>();
                if (ambience != null)
                {
                    ambience.StopAmbience(fadeOutDuration);
                }
            }
        }
    }
}
