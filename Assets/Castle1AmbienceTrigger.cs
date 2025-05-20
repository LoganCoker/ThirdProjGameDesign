using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle1AmbienceTrigger : MonoBehaviour
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
                Castle1Ambience ambience = mainCamera.GetComponent<Castle1Ambience>();
                if (ambience != null)
                {
                    Debug.Log("Camera found stop ambience");
                    ambience.StopAmbience(fadeOutDuration);
                }
            }
        }
    }
}
