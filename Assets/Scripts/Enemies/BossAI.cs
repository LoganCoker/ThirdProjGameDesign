using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour {

    #region publics
    public Transform player;
    public NavMeshAgent agent;
    public int Health { get; private set; } = 3;
    public bool Dead { get; private set; }
    public Animator animator;
    #endregion

    #region private
    private Transform attacks;
    private bool hit;
    private float hitCooldown;
    private float distTolerance = 5f;
    private float attackTiming = .1f;
    private bool canAttack;
    private bool inAttack;
    #endregion

    private void Start() {
        attacks = transform.GetChild(1);
    }

    void Update() {
        // testing
        if (Input.GetKeyDown(KeyCode.T)) { StartCoroutine(Death()); }

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (!inAttack) {
            if (distToPlayer > 5) {
                agent.SetDestination(player.position); 
                canAttack = false;
            } else if (distToPlayer <= 5) {
                agent.SetDestination(transform.position);
                agent.Move(-transform.forward * 2 * Time.deltaTime);
                canAttack = true;
            }
        }

        if (hitCooldown < 0) {
            hitCooldown = 0;
            hit = false;
        }

        if (canAttack && attackTiming < 0) {
            inAttack = true;
            agent.SetDestination(player.position);
            
            if (distToPlayer < distTolerance) {
                agent.SetDestination(transform.position);
                StartCoroutine(AttackRight());
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
        }
    }

    public void DecHealth() {
        if (!hit) { 
            Health--;
            hit = true;
            hitCooldown = 3f;
            if (Health == 0) {
                inAttack = true;
                agent.SetDestination(transform.position);
                attackTiming = int.MaxValue;
                StartCoroutine(Death());
            }
        }
    }

    IEnumerator Death() {
        animator.SetTrigger("DraculaDeath");
        yield return new WaitForSeconds(8);
        this.gameObject.SetActive(false);
        Dead = true;
    }

    IEnumerator AttackLeft() {
        Vector3 fireBallHome = transform.GetChild(1).position;
        Transform fireBall = attacks.GetChild(1);
        animator.SetTrigger("LeftPunch");
        attackTiming = 5f;
        yield return new WaitForSeconds(1f);
        fireBall.gameObject.SetActive(true);
        fireBall.Translate(transform.forward, transform);
        yield return new WaitForSeconds(1f);
        fireBall.gameObject.SetActive(false);
        fireBall.position = fireBallHome;
        canAttack = false;
        inAttack = false;

    }
    
    IEnumerator AttackRight() {
        animator.SetTrigger("RightPunch");
        attackTiming = 5f;
        yield return new WaitForSeconds(1.2f);
        attacks.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        attacks.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        canAttack = false;
        inAttack = false;

    }
}
