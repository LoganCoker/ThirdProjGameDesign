using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Cinemachine.Utility;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour {

    #region publics
    public CinemachineFreeLook cam;
    public float Health { get; private set; }
    public float moveSpeed;
    public float runSpeed;
    public LayerMask ground;
    public float cameraSens;
    public Animator animator;
    #endregion

    #region privates
    private float maxHealth;
    private float jumpHeighth = 7;
    private bool crouched;
    private Transform body;
    private Transform orient;
    private int jumpCnt;
    private Rigidbody self;
    private bool running;
    private InputController.PlayerOtherActions input;
    private bool climbing;
    #endregion

    void Start() {
        maxHealth = Health;
        body = transform.GetChild(0);
        orient = transform.GetChild(1);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        self = GetComponent<Rigidbody>();
        input = Game.Input.PlayerOther;
    }

    void Update() {
        // camera player positioning
        orient.rotation = Quaternion.Euler(0, cam.m_XAxis.Value, 0);
        
        // moving
        Vector2 movement2D = input.Movement.ReadValue<Vector2>();
        Vector3 movement3D = new(movement2D.x, 0, movement2D.y);
        movement3D = orient.rotation * movement3D;
        movement3D.Normalize();

        Vector3 sanatizedMove = movement3D;
        #region wallChecks
        // checks left wall
        if (WallCheck(Vector3.left)) {
            if (movement3D.x < 0) {
                sanatizedMove.x = 0;
            }
        }
        // checks right wall
        if (WallCheck(Vector3.right)) { 
            if (movement3D.x > 0) {
                sanatizedMove.x = 0;
            }
        }
        // checks forward wall
        if (WallCheck(Vector3.forward)) {
            if (movement3D.z > 0) {
                sanatizedMove.z = 0;
            }
        }
        // checks back wall
        if (WallCheck(Vector3.back)) {
            if (movement3D.z < 0) {
                sanatizedMove.z = 0;
            }
        }
        #endregion

        if (self.velocity.magnitude < moveSpeed) {
            self.velocity += sanatizedMove * 100 * Time.deltaTime;
        }
        // sprinting
        if (self.velocity.magnitude < runSpeed) {
            if (input.Sprint.IsPressed() || running) {
                self.velocity += sanatizedMove * 150 * Time.deltaTime;
                running = true;
            }
        }
        // body rotation
        if (input.Movement.IsPressed()) { 
            body.rotation = Quaternion.LookRotation(movement3D);
            animator.SetFloat("Walking", 7f);
        }
        // dampen movements if not actively walking/running
        if (input.Movement.WasReleasedThisFrame()) {
            if (GroundCheck()) {
                 self.velocity -= self.velocity *.9f;
                 running = false;
            }
            animator.SetFloat("Walking", 10f);
        }

        // jumping
        if (input.Jump.WasPressedThisFrame() && (GroundCheck() || jumpCnt > 0)) {
            self.velocity += Vector3.up * jumpHeighth;
        }
        // jump counter (double jump)
        if (GroundCheck()) {
            jumpCnt = 2;
        }
        // double jump check
        if (input.Jump.WasReleasedThisFrame()) {
            jumpCnt--;
        }

        // Dance
        if (input.Crouch.IsPressed() && GroundCheck() && !crouched) {
            animator.SetBool("HittingIt", true);
            crouched = true;
        }
        if (input.Crouch.WasReleasedThisFrame()) {
            animator.SetBool("HittingIt", false);
            crouched = false;
        }
    }

    IEnumerator Vault() {
        running = false;
        self.velocity += Vector3.up * jumpHeighth*2f;
        self.velocity += body.forward;
        yield return new WaitForSeconds(.15f);
        self.velocity -= Vector3.up * jumpHeighth *2;
        running = true;
    }

    IEnumerator WallClimb() {
        climbing = true;
        input.Disable();
        self.velocity = Vector3.zero;
        body.GetChild(0).Rotate(Vector3.right, -160);
        self.velocity += Vector3.up * jumpHeighth * 2f;
        yield return new WaitForSeconds(.2f);
        self.velocity += body.forward * 3f;
        self.velocity -= Vector3.up * jumpHeighth;
        body.GetChild(0).Rotate(Vector3.right, 160);
        input.Enable();
        climbing = false;
    }

    private bool GroundCheck() {
        bool down = Physics.Raycast(transform.position, Vector3.down, 1.01f, ground);
        return down;
    }

    private bool WallCheck(Vector3 dir) {
        bool res = false;
        for (int i = 2; i >= -2; i--) {
            Vector3 playerScan = new(transform.position.x, transform.position.y + i/2, transform.position.z);
            res = Physics.Raycast(playerScan, dir, .4f, ground);
            if (res) { break; }
        }
        return res;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("VaultWall") && running) {
            StartCoroutine(Vault());
        }
        if (other.CompareTag("WallClimb") && !GroundCheck() && !climbing && WallCheck(body.forward)) {
            StartCoroutine(WallClimb());
        }
    }
}

