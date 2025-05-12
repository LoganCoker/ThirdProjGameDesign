using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour {

    #region publics
    public Transform player;
    public NavMeshAgent agent;
    public int Health { get; private set; } = 3;
    public Animator animator;
    #endregion

    #region private
    private bool hit;
    private float hitCooldown;
    private float distTolerance = .05f;
    private float attackTiming = .1f;
    private bool canAttack;
    private bool inAttack;
    #endregion

    void Start() {
        
    }

    void Update() {
        // testing
        if (Input.GetKeyDown(KeyCode.T)) { StartCoroutine(Death()); }
        
        agent.SetDestination(player.position);

        if (hitCooldown < 0) {
            hitCooldown = 0;
            hit = false;
        }

        if (Vector3.Distance(transform.position, player.position) < agent.stoppingDistance && !inAttack) {
            agent.Move(-transform.forward * Time.deltaTime);
            canAttack = true;
        }

        if (canAttack && attackTiming < 0) {
            inAttack = true;
            agent.SetDestination(player.position);
            if (Vector3.Distance(transform.position, player.position) < distTolerance) {
                //animator.SetTrigger("LeftPunch");
                animator.SetTrigger("RightPunch");
                attackTiming = 3f;
                canAttack = false;
                inAttack = false;
            }
        }

        // walking
        if (agent.velocity.magnitude != 0) {
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }


        hitCooldown -= Time.deltaTime;
        attackTiming -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerAttack")) {
            DecHealth();
            print("boss hit");
        }
    }

    public void DecHealth() {
        if (!hit) { 
            print("hurt");
            Health--;
            hit = true;
            hitCooldown = 3f;
            if (Health == 0) {
                StartCoroutine(Death());
            }
        }
    }

    IEnumerator Death() {
        animator.SetTrigger("DraculaDeath");
        yield return new WaitForSeconds(8);
        this.gameObject.SetActive(false);
    }
}
