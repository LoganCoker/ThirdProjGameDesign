using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingPuzzle : MonoBehaviour {

    #region publics
    public LayerMask player;
    public float size;
    #endregion

    #region privates
    private Rigidbody rb;
    #endregion

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (WallCheckZ(Vector3.back)) {
            rb.velocity = Vector3.forward * 5;
        }
        if (WallCheckZ(Vector3.forward)) {
            rb.velocity = Vector3.back * 5;
        }
        if (WallCheckX(Vector3.right)) {
            rb.velocity = Vector3.left * 5;
        }
        if (WallCheckX(Vector3.left)) {
            rb.velocity = Vector3.right * 5;
        }
    }

    private bool WallCheckZ(Vector3 dir) {
        bool res = false;
        for (float i = 4; i >= -4; i--) {
            Vector3 pushBlock = new(transform.position.x + i/8, transform.position.y, transform.position.z);
            res = Physics.Raycast(pushBlock, dir, size, player);
            Debug.DrawRay(pushBlock, dir, Color.red);
            if (res) { break; }
        }
        return res;
    }
     private bool WallCheckX(Vector3 dir) {
        bool res = false;
        for (float i = 4; i >= -4; i--) {
            Vector3 pushBlock = new(transform.position.x, transform.position.y, transform.position.z + i/8);
            res = Physics.Raycast(pushBlock, dir, size, player);
            Debug.DrawRay(pushBlock, dir, Color.red);
            if (res) { break; }
        }
        return res;
    }

    
}
