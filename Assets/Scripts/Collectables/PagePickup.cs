using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PagePickup : MonoBehaviour
{
    public int pageID; 
    public string pageText; 

    private bool collected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;
        if (other.CompareTag("Player"))
        {
            collected = true;
            Debug.Log($"Collected page {pageID}");
            PageUIManager.Instance.CollectPage(pageID);
            gameObject.SetActive(false);
        }
    }
}


