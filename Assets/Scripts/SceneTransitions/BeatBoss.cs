using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeatBoss : MonoBehaviour {

    public BossAI boss;


    void Update() {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (boss.Dead) {
            SceneManager.LoadScene("GameOver");
        }
    }  
}
