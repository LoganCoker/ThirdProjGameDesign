using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatBoss : MonoBehaviour {

    public BossAI boss;


    void Update() {
        if (boss.Dead) {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene("GameOver - Win");
        }
    }  
}
