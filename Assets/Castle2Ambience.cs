using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle2Ambience : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private string ambienceSoundName = "CreepyPiano";
    [SerializeField] private float fadeInDuration = 2.0f;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.FadeIn(ambienceSoundName, fadeInDuration);
        }
    }

    public void StopAmbience(float fadeOutDuration = 1.5f)
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.FadeOut(ambienceSoundName, fadeOutDuration);
        }
    }
}
