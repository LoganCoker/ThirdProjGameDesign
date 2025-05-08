using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerInMove : MonoBehaviour {

    [SerializeField] private CharacterController controller;
    [SerializeField] private CinemachineFreeLook playerCam;

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
    private int jumpCnt = 1;
    private float vertVelo;
    #endregion

    
    private void Awake() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // finicky, needs more testing
        playerCam.m_XAxis.m_MaxSpeed *= sensX;
        playerCam.m_YAxis.m_MaxSpeed *= sensY;

        playerInput = GetComponent<PlayerInput>();
        body = transform.GetChild(0);
        orient = transform.GetChild(1);
    }

    public void Update() {
        // sets camera looking direction
        orient.rotation = Quaternion.Euler(0, playerCam.m_XAxis.Value, 0);

        // falling
        if (!GroundCheck()) {
            vertVelo -= grav/2 * Time.deltaTime;
            // falling ani
        }

        // jumping/falling
        if (GroundCheck()) {
            jumpCnt = 1;
            if (vertVelo < 0) {
                vertVelo = 0f;
            }
        }
        if ((GroundCheck() || jumpCnt > 0) && playerInput.Jumped) {
            vertVelo = 0;
            vertVelo += Mathf.Sqrt(jumpHeighth * 3 * grav);
            jumpCnt--;
            // jump animation
        }

        #region movement
        // make forward direction of camera
        Vector3 camForward = new Vector3(orient.forward.x, 0f, orient.forward.z).normalized;
        Vector3 camRight = new Vector3(orient.right.x, 0f, orient.right.z).normalized;
        Vector3 move = camRight * playerInput.MoveInput.x + camForward * playerInput.MoveInput.y;
        
        // sprint check
        running = playerInput.Sprinting || running;
        float moveSpeed = running ? acc * sprintMult : acc;
        float speedClamp = running ? sprintMult * speed : speed;

        // kinematic movement equations + drag 
        Vector3 moveDelta = GroundCheck() ? move * moveSpeed * Time.deltaTime : controller.velocity; // maintain air move
        Vector3 velo = controller.velocity + moveDelta;
        Vector3 dragVect = velo.normalized * drag * Time.deltaTime;

        // drag check and move application
        velo = (velo.magnitude > drag * Time.deltaTime) ? velo - dragVect : Vector3.zero;
        velo = Vector3.ClampMagnitude(velo, speedClamp);
        velo.y += vertVelo;
        controller.Move(velo * Time.deltaTime);

        // body rotaion based on movement
        if (isMoving()) {
            body.rotation = Quaternion.LookRotation(new Vector3(velo.x, 0, velo.z));
        } else { 
            running = false;
        }
        
        // animations
        animator.SetFloat("Walking", Mathf.Abs(controller.velocity.x) + Mathf.Abs(controller.velocity.z));
        #endregion
    }

    private bool isMoving() {
        Vector3 lateralVelocity = new Vector3(controller.velocity.x, 0f, controller.velocity.y);
        return lateralVelocity.magnitude > 0.01f;
    }

    private bool GroundCheck() {
        return Physics.Raycast(transform.position, Vector3.down, 1.05f, ground);
    }
}
