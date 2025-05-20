using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle2AmbienceTrigger : MonoBehaviour
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
                Castle2Ambience ambience = mainCamera.GetComponent<Castle2Ambience>();
                if (ambience != null)
                {
                    Debug.Log("Camera found stop ambience");
                    ambience.StopAmbience(fadeOutDuration);
                }
            }
        }
    }
}
