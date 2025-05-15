using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {
    public int Health { get; private set; }
    public int Dead {get; private set; }

    #region publics
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public int health;
    #endregion

    #region private
    private bool hit;
    private float hitCooldown;
    private float distTolerance = .05f;
    private float attackTiming = 3f;
    private bool canAttack;
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

        if (Vector3.Distance(transform.position, player.position) < agent.stoppingDistance) {
            agent.Move(-transform.forward * Time.deltaTime);
            canAttack = true;
        }

        if (canAttack) {
            if (attackTiming < 0) {
                //animator.SetTrigger("LeftPunch");
                animator.SetTrigger("RightPunch");
                attackTiming = 3f;
                canAttack = false;
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
            print("enemy hit");
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
        animator.SetTrigger("SkeletonDeath");
        yield return new WaitForSeconds(8);
        this.gameObject.SetActive(false);
    }
}
