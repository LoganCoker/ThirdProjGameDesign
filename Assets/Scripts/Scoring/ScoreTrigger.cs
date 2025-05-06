using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public GameObject nameEntryUI;     
    public int scoreToSubmit = 12345;  

    private bool triggered = false;    

    void OnTriggerEnter(Collider other)
    {
        if (triggered) return;

        if (other.CompareTag("Player")) 
        {
            triggered = true;

            nameEntryUI.SetActive(true);
            var nameEntry = nameEntryUI.GetComponent<NameEntryUI>();
            if (nameEntry != null)
            {
                nameEntry.scoreToSubmit = scoreToSubmit;
            }

            Debug.Log("Player triggered score UI");
        }
    }
}
