using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFireBall : MonoBehaviour {


    void Start() {
        transform.SetParent(null);
    }

    void Update() {
        transform.Rotate(0,1,0);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerAttack")) {
            Destroy(gameObject);
            Game.AddScore(50);
        }
    }
}
