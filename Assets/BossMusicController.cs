using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMusicController : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private string bossMusicName = "BossMusic"; 
    [SerializeField] private float fadeInDuration = 2.0f;
    [SerializeField] private float fadeOutDuration = 3.0f;

    [Header("Boss Reference")]
    [SerializeField] private BossAI bossAI; 

    private bool musicPlaying = false;
    private bool fadeOutStarted = false;

    void Start()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.FadeIn(bossMusicName, fadeInDuration);
            musicPlaying = true;

            if (bossAI == null)
            {
                bossAI = FindObjectOfType<BossAI>();
            }
        }
        else
        {
            Debug.LogError("AudioManager instance not found!");
        }
    }

    void Update()
    {
        if (musicPlaying && !fadeOutStarted && bossAI != null && bossAI.Dead)
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.FadeOut(bossMusicName, fadeOutDuration);
                fadeOutStarted = true;
            }
        }
    }
}
