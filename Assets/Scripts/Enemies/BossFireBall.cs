using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossFireBall : MonoBehaviour {

    private float timeElapsed;

    void Start() {
        transform.SetParent(null);
    }

    void Update() {
        transform.Rotate(0,1,0);
        timeElapsed += Time.deltaTime;
        
        // insurance to destory if doesn't for whatever reason
        if (timeElapsed > 10f) {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerAttack")) {
            Destroy(gameObject);
            Game.AddScore(50);
        }
    }
}
