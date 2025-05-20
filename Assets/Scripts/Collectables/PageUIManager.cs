using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUIManager : MonoBehaviour
{
    public static PageUIManager Instance { get; private set; }

    public int numOfPages;

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

        collectedPages = new bool[numOfPages];

        for (int i = 0; i < pageOverlays.Length; i++)
        {
            if (pageOverlays[i] != null)
            {
                // show uncollected pages first
                Transform colored = pageOverlays[i].transform.Find("ColoredPage");
                Transform black = pageOverlays[i].transform.Find("BlackPage");

                if (colored != null) colored.gameObject.SetActive(false);
                if (black != null) black.gameObject.SetActive(true);
            }
        }
    }
    public void CollectPage(int id)
    {
        if (id < 0 || id >= numOfPages) return;

        // mark and show UI
        if (!collectedPages[id])
        {
            collectedPages[id] = true;

            // hide black page and colored page
           Transform colored = pageOverlays[id].transform.Find("ColoredPage");
            Transform black = pageOverlays[id].transform.Find("BlackPage");

            if (colored != null) colored.gameObject.SetActive(true);
            if (black != null) black.gameObject.SetActive(false);
        }
    }

    public int TotalCollected() {
        int total = 0;
        foreach (bool page in collectedPages) {
            if (page) total++;
        }
        return total;
    }
}

