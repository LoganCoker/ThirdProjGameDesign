using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInMove : MonoBehaviour {

    [SerializeField] private CharacterController controller;
    [SerializeField] private CinemachineFreeLook playerCam;
    public bool InAction { get; set; }
    public bool UpdateSens { get; set; }
    public Vector3 WasHit { get; set; } = Vector3.zero;

    #region publics
    public Animator animator;
    public LayerMask ground;
    [Header("Movement")]
    public float acc; 
    public float speed;
    public float drag;
    public float grav;
    public float jumpHeighth;
    [Header("Mouse Sens (0-1)")]
    public float sensX = 1f;
    public float sensY = 1f;
    #endregion

    #region privates
    private PlayerInput playerInput;
    private Transform body;
    private Transform orient;
    private float sprintMult = 1.5f;
    private bool running;
    private int jumpCnt = 2;
    private float vertVelo;
    private float initXSens;
    private float initYSens;
    #endregion

    
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        initXSens = playerCam.m_XAxis.m_AccelTime;
        initYSens = playerCam.m_YAxis.m_AccelTime;

        playerInput = Game.PlayerInput;
        body = transform.GetChild(0);
        orient = transform.GetChild(1);
    }

    public void Update() {
        if (Game.Paused) return;
        if (UpdateSens) {
            playerCam.m_XAxis.m_AccelTime = initXSens;
            playerCam.m_YAxis.m_AccelTime = initYSens;
        }

        // sets camera looking direction
        orient.rotation = Quaternion.Euler(0, playerCam.m_XAxis.Value, 0);

        #region mid air
        // falling
        if (!GroundCheck()) {
            vertVelo -= grav * Time.deltaTime;
            if (vertVelo > grav/2 ) { 
                vertVelo = grav/2; 
            }
            // falling ani
        }
        // jumping & jump reset
        if (GroundCheck()) {
            jumpCnt = 2;
            if (vertVelo < 0) {
                vertVelo = 0f;
            }
        }
        if ((GroundCheck() || jumpCnt > 0) && playerInput.Jumped) {
            vertVelo = 0;
            vertVelo += Mathf.Sqrt(jumpHeighth * 3 * grav);
            jumpCnt--;
            animator.SetTrigger("Jump");
        }
        #endregion

        #region movement
        // make forward direction of camera
        Vector3 camForward = new Vector3(orient.forward.x, 0f, orient.forward.z).normalized;
        Vector3 camRight = new Vector3(orient.right.x, 0f, orient.right.z).normalized;
        Vector3 move = camRight * playerInput.MoveInput.x + camForward * playerInput.MoveInput.y;
        
        // sprint check
        running = playerInput.Sprinting || running;
        float moveSpeed = running ? acc * sprintMult : acc;
        float speedClamp = running ? speed * sprintMult : speed;
        if (running) { CamAlaign(); }

        // determin move + drag 
        Vector3 moveDelta =  move * moveSpeed;
        Vector3 velo = controller.velocity + moveDelta;
        Vector3 dragVect = velo.normalized * drag; 

        // drag check and move application
        velo = (velo.magnitude > drag) ? velo - dragVect : Vector3.zero;
        velo = Vector3.ClampMagnitude(velo, speedClamp);
        velo.y += vertVelo;
        // only move if not preforming a special movement action
        if (WasHit != Vector3.zero) {
            velo = 10 * WasHit;
            velo.y = 50f;
            WasHit = Vector3.zero;   
        }
        if (!InAction) { controller.Move(velo * Time.deltaTime); }
            
        // body rotaion based on movement
        if (isMoving() && !InAction) {
            body.rotation = Quaternion.LookRotation(new Vector3(velo.x, 0, velo.z));
        } else { 
            running = false;
        }
        
        // animations
        animator.SetFloat("Walking",Mathf.Sqrt(Mathf.Pow(controller.velocity.x, 2) + Mathf.Pow(controller.velocity.z, 2)));
        #endregion

        // hitting it
        animator.SetBool("HittingIt", (playerInput.Dance) && !isMoving());
    }

    #region checks
    private bool isMoving() {
        Vector3 lateralVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.z);
        return lateralVelocity.magnitude > 0.01f;
    }

    private bool GroundCheck() {
        return Physics.Raycast(transform.position, Vector3.down, 1.05f, ground);
    }

    private bool WallCheck(Vector3 dir) {
        bool res = false;
        for (int i = 2; i >= -2; i--) {
            Vector3 playerScan = new(transform.position.x, transform.position.y + i / 2, transform.position.z);
            res = Physics.Raycast(playerScan, dir, .4f, ground);
            if (res) { break; }
        }
        return res;
    }
    #endregion

    #region special moves
    IEnumerator Vault() {
        animator.SetTrigger("Climb");
        InAction = true;
        float up = 0f;
        while (up < .1f) {
            controller.Move(new(0, 10 * Time.deltaTime, 0));
            up += Time.deltaTime;
            yield return new WaitForEndOfFrame();
            animator.SetFloat("Walking", 10f);
        }
        controller.Move(body.forward / 2);
        controller.Move(-body.up / 2);
        InAction = false;
        running = true;
    }
    IEnumerator WallClimb() {
        animator.SetTrigger("Climb");
        InAction = true;
        float up = 0f;
        while (up < .25f) {
            controller.Move(new(0, 10 * Time.deltaTime, 0));
            up += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        controller.Move(body.forward);
        InAction = false;
    }
    #endregion

    private void CamAlaign() {
        playerCam.m_XAxis.Value += controller.velocity.x * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("VaultWall") && running && !InAction) {
            StartCoroutine(Vault());
        }
        if (other.CompareTag("WallClimb") && !GroundCheck() && WallCheck(body.forward) && !InAction) {
            StartCoroutine(WallClimb());
        }
    }
}
