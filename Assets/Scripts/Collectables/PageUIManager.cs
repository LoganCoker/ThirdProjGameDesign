using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUIManager : MonoBehaviour
{
    public static PageUIManager Instance { get; private set; }

    public int numOfPages;

    private bool[] collectedPages;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        
        collectedPages = new bool[numOfPages];
    }

    public void CollectPage(int id)
    {
        if (id < 0 || id >= numOfPages) return;
        
        collectedPages[id] = true;
    }

    public int TotalCollected() {
        int total = 0;
        foreach (bool page in collectedPages) {
            if (page) total++;
        }
        return total;
    }
}

