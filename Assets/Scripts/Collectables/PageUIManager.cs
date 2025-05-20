using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PageUIManager : MonoBehaviour {
    public static PageUIManager Instance { get; private set; }

    public int numOfPages;

    public GameObject[] pageOverlays;

    private void Awake() {
        Instance = this;

        UpdateUI();
    }

    public void CollectPage(int id) {
        if (id == 69) {
            // gabe cube bonus
            Game.AddScore(2000);
        }
        if (id < 0 || id >= numOfPages) return;

        Game.Notes[id] = true;
        UpdateUI();
    }

    public void UpdateUI() {
        for (int i = 0; i < Game.Notes.Length; i++) {
            if (Game.Notes[i]) {
                pageOverlays[i].transform.GetChild(1).gameObject.SetActive(false);
            }
        }
    }
}

