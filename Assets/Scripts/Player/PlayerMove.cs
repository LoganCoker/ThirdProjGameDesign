using System.Collections;
using System.Collections.Generic;
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
    #endregion

    void Start() {
        maxHealth = health;
        cam = transform.GetChild(0);
        body = transform.GetChild(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        var input = Game.Input.Player;
        Vector2 movement2D = input.Movement.ReadValue<Vector2>();
        Vector3 movement3D = new(movement2D.x, 0, movement2D.y);

        Vector2 look2D = input.Look.ReadValue<Vector2>();
        Vector3 look3D = new(look2D.y, look2D.x, 0);

        // move
        transform.Translate(movement3D * moveSpeed * Time.deltaTime);
        // sprinting
        if (input.Sprint.IsPressed() && GroundCheck()) {
            transform.Translate(movement3D * runSpeed * Time.deltaTime);
        }

        // jumping
        if (input.Jump.IsPressed() && GroundCheck()){
            //transform.Translate(Vector3.up * jumpHeighth * Time.deltaTime);
            Rigidbody rb = GetComponent<Rigidbody>();
            rb.velocity = Vector3.up * jumpHeighth;

        }

        // crouch
        if (input.Crouch.IsPressed() && GroundCheck() && !crouched) {
            body.Rotate(Vector3.right, -50);
            //transform.Rotate();
            crouched = true;
        }
        if (input.Crouch.WasReleasedThisFrame()) {
            body.rotation = new Quaternion(0,0,0,0);
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
        return Physics.Raycast(transform.position, Vector3.down, 1.01f, ground);
    }
}

