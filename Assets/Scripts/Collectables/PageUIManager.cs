using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUIManager : MonoBehaviour
{
    public static PageUIManager Instance;

    [Tooltip("Drag in your 4 paper overlays here in order (Page 1 at index 0, Page 2 at index 1, etc).")]
    public GameObject[] pageOverlays;  

    private bool[] collectedPages;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        
        int n = pageOverlays.Length;
        collectedPages = new bool[n];

        for (int i = 0; i < n; i++)
            if (pageOverlays[i] != null)
                pageOverlays[i].SetActive(false);
    }

    public void CollectPage(int id)
    {
        int idx = id - 1;
        if (idx < 0 || idx >= pageOverlays.Length) return;
        if (collectedPages[idx]) return;

        collectedPages[idx] = true;
        pageOverlays[idx].SetActive(true);
    }
}

