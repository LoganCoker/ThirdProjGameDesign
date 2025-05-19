using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudio : MonoBehaviour
{
    [Header("Footstep Settings")]
    [SerializeField] private string[] footstepSounds = { "Footstep1", "Footstep2", "Footstep3", "Footstep4", "Footstep5" };

    // Called from animation events
    public void PlayFootstepSound()
    {
        if (footstepSounds.Length > 0)
        {
            // Select a random footstep sound
            string randomSound = footstepSounds[Random.Range(0, footstepSounds.Length)];

            // Play through AudioManager
            AudioManager.Instance.Play(randomSound);
        }
    }
}

