using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingPuzzle : MonoBehaviour {

    #region publics
    #endregion

    #region privates
    private Rigidbody rb;
    #endregion

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Player")) {
            print("pushing");
            Rigidbody playerRb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity.Set(playerRb.velocity.x * 1.5f, 0, playerRb.velocity.z * 1.5f);
        }
    }
}
