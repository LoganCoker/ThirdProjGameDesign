using UnityEngine;

public class PushingPuzzle : MonoBehaviour {

    #region publics
    public LayerMask player;
    public Transform center;
    public float widthZ;
    public float lenghtX;
    #endregion

    #region privates
    private Rigidbody rb;
    private float playerPush = 0.1f;
    #endregion

    void Start() {
        rb = GetComponent<Rigidbody>();
    }

    void Update() {
        if (WallCheckZ(Vector3.back)) {
            rb.velocity = Vector3.forward * 7;
        }
        if (WallCheckZ(Vector3.forward)) {
            rb.velocity = Vector3.back * 7;
        }
        if (WallCheckX(Vector3.right)) {
            rb.velocity = Vector3.left * 7;
        }
        if (WallCheckX(Vector3.left)) {
            rb.velocity = Vector3.right * 7;
        }
    }

    private bool WallCheckZ(Vector3 dir) {
        bool res = false;
        float inc = -lenghtX / 2;
        while (inc <= lenghtX / 2) {
            Vector3 pushBlock = new(center.position.x + inc, center.position.y, center.position.z);
            res = Physics.Raycast(pushBlock, dir, (widthZ / 2) + playerPush, player);
            Debug.DrawRay(pushBlock, ((widthZ / 2) + playerPush) * dir, Color.red);
            inc += lenghtX / 8;
            if (res) { break; }
        }
        return res;
    }

     private bool WallCheckX(Vector3 dir) {
        bool res = false;
        float inc = -widthZ / 2;
        while (inc <= widthZ / 2) {
            Vector3 pushBlock = new(center.position.x, center.position.y, center.position.z + inc);
            res = Physics.Raycast(pushBlock, dir, (lenghtX / 2) + playerPush, player);
            Debug.DrawRay(pushBlock, ((lenghtX / 2) + playerPush) * dir, Color.red);
            inc += widthZ / 16;
            if (res) { break; }
        }
        return res;
     }
}
