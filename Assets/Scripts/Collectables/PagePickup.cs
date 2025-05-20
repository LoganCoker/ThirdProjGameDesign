using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PagePickup : MonoBehaviour {

    public int pageID;
    public Canvas msg;

    private bool collected;

    private void Update() {
        if ((Input.GetKeyUp(KeyCode.Escape) || Input.GetMouseButtonDown(0)) && collected) PageOff();
       
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            //Debug.Log($"Collected page {pageID}");
            PageUIManager.Instance.CollectPage(pageID);
            ShowPage();
        }
    }

    private void ShowPage() {
        collected = true;
        msg.enabled = true;
        Game.PauseGame();
    }

    private void PageOff() {
        msg.enabled = false;
        Game.ResumeGame();
        gameObject.SetActive(false);
    }
}


