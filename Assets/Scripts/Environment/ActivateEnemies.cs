using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateEnemies : MonoBehaviour {

    void Start() {
        foreach (Transform obj in transform) {
            obj.gameObject.SetActive(false);
        }
    }


    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            foreach (Transform obj in transform) {
                obj.gameObject.SetActive(true);
            }
        }
    }
}
