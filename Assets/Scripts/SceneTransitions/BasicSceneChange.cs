using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BasicSceneChange : MonoBehaviour {

    public string sceneName;

    void Start() {

    }

    void Update() {

    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            SceneManager.LoadScene(sceneName);
        }
    }
}
