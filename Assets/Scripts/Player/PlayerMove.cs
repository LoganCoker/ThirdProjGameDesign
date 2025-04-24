using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    #region publics
    public float health { get; private set; }
    public float moveSpeed;
    public float runSpeed;
    public LayerMask ground;
    public float cameraSense;
    #endregion

    #region privates
    private float maxHealth;
    private float jumpHeighth = 7;
    private bool crouched;
    private Transform body;
    private Transform cam;
    private int jumpCnt;
    private Rigidbody self;
    private Quaternion bodyRot;
    private bool running;
    #endregion

    void Start() {
        maxHealth = health;
        cam = transform.GetChild(0);
        body = transform.GetChild(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        self = GetComponent<Rigidbody>();
    }

    void Update() {
        var input = Game.Input.Player;
        Vector2 movement2D = input.Movement.ReadValue<Vector2>();
        Vector3 movement3D = new(movement2D.x, 0, movement2D.y);
        
        Vector2 look2D = input.Look.ReadValue<Vector2>();
        Vector3 look3D = new(look2D.y, look2D.x, 0);

        if (GroundCheck()) {
            jumpCnt = 2;
        }

        #region wallChecks
        // checks left wall
        if (WallCheck(Vector3.left)) {
            if (movement3D.x < 0) {
                movement3D.x = 0;
            }
        }
        // checks right wall
        if (WallCheck(Vector3.right)) { 
            if (movement3D.x > 0) {
                movement3D.x = 0;
            }
        }
        // checks forward wall
        if (WallCheck(Vector3.forward)) {
            if (movement3D.z > 0) {
                movement3D.z = 0;
            }
        }
        // checks back wall
        if (WallCheck(Vector3.back)) {
            if (movement3D.z < 0) {
                movement3D.z = 0;
            }
        }
        #endregion

        // move
        if (self.velocity.magnitude < moveSpeed) {
            self.velocity += movement3D * 100 * Time.deltaTime;
        }
        // sprinting
        if (self.velocity.magnitude < runSpeed) {
            if (input.Sprint.IsPressed() || running) {
                self.velocity += movement3D * 150 * Time.deltaTime;
                running = true;
            }
        }
        // body rotation
        if (input.Movement.WasPerformedThisFrame()) { 
            bodyRot = Quaternion.LookRotation(movement3D);
            body.rotation = bodyRot; 
        }
        // dampen movements if not actively walking/running
        if (input.Movement.WasReleasedThisFrame() && GroundCheck()) {
            self.velocity -= self.velocity *.9f;
            running = false;
        }

        // jumping
        if (input.Jump.WasPressedThisFrame() && (GroundCheck() || jumpCnt > 0)) {
            self.velocity += Vector3.up * jumpHeighth;
        }
        // double jump check
        if (input.Jump.WasReleasedThisFrame()) {
            jumpCnt--;
        }

        // crouch
        if (input.Crouch.IsPressed() && GroundCheck() && !crouched) {
            body.Rotate(Vector3.right, -50);
            //transform.Rotate();
            crouched = true;
        }
        if (input.Crouch.WasReleasedThisFrame()) {
            body.rotation = bodyRot;
            crouched = false;
        }

        // slide
        if (input.Sprint.IsPressed() && input.Crouch.IsPressed() && GroundCheck()) {
            // implement slide (coroutine)
        }

        // Camera movement        
        //cam.transform.Rotate(look3D * cameraSense * Time.deltaTime);

    }

    private bool GroundCheck() {
        bool down = Physics.Raycast(transform.position, Vector3.down, 1.01f, ground);
        return down;
    }

    private bool WallCheck(Vector3 dir) {
        bool res = false;
        for (int i = 2; i >= -2; i--) {
            Vector3 playerScan = new(transform.position.x, transform.position.y + i/2, transform.position.z);
            res = Physics.Raycast(playerScan, dir, 1f, ground);
            if (res) { break; }
        }
        return res;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Wall")) {
            self.velocity = Vector3.down;
        }
    }
}

