using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAudio : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private float volumeMultiplier = 1.0f;
    [SerializeField] private float minDistance = 1.0f; 
    [SerializeField] private float maxDistance = 15.0f; 

    private Transform playerTransform;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
    }

    public void PlaySkeletonWalk()
    {
        int soundIndex = Random.Range(1, 5);

        string soundName = "SkeleStep" + soundIndex;

        float volume = volumeMultiplier;
        if (playerTransform != null)
        {
            float distance = Vector3.Distance(transform.position, playerTransform.position);

            if (distance > minDistance)
            {
                volume *= Mathf.Clamp01(1.0f - ((distance - minDistance) / (maxDistance - minDistance)));
            }
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayAtPosition(soundName, transform.position);
        }
        else
        {
            Debug.LogWarning("AudioManager instance not found!");
        }
    }
}
