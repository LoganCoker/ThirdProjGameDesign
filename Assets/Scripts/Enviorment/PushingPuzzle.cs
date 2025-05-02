using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingPuzzle : MonoBehaviour {

    #region publics
    #endregion

    #region privates
    private bool allowMove;
    private Rigidbody rb;
    #endregion

    void Start() {
        allowMove = false;
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
    }

    private void OnCollisionStay(Collision collision) {
        if (collision.gameObject.CompareTag("Pushing")) {
            allowMove = true;
        }
        if (collision.gameObject.CompareTag("Player")) {
            if (allowMove) {
                collision.GetContact(0);
                rb.velocity.Set(collision.relativeVelocity.x * 1.5f, 0, collision.relativeVelocity.z * 1.5f);
            }
        }
    }

    private void OnCollisionExit(Collision collision) {
        if (collision.gameObject.CompareTag("Pushing")) {
            allowMove = false;
        }
    }
}
