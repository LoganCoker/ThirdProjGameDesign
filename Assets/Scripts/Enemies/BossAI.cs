using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.Android;

public class BossAI : MonoBehaviour {

    public int Health { get; private set; }
    public bool Dead { get; private set; }

    #region publics
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public int health;
    #endregion

    #region private
    private Transform attacks;
    private ParticleSystem blood;
    private float hitCooldown;
    private float distTolerance = .2f;
    private float attackTiming = 20f;
    private float changeDir;
    private float orbitRange;
    private float meleeRange = 1.5f;
    private bool hit;
    private bool canAttack;
    private bool inAttack;
    private bool inAni;
    private bool orbitClockwise;
    #endregion

    private void Start() {
        attacks = transform.GetChild(1);
        this.Health = health;
        blood = GetComponent<ParticleSystem>();
        agent.updateRotation = false;
        orbitRange = agent.stoppingDistance;
    }

    void Update() {
        // testing
        if (Input.GetKeyDown(KeyCode.T)) { StartCoroutine(Death()); }

        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (changeDir > 0) {
            // direction check
        }

        if (hitCooldown < 0) {
            hitCooldown = 0;
            hit = false;
        }

        if (!inAttack) {
            if (distToPlayer > agent.stoppingDistance + distTolerance) {
                agent.SetDestination(player.position);
                canAttack = false;
            } else if (distToPlayer >= agent.stoppingDistance - distTolerance && distToPlayer <= agent.stoppingDistance + distTolerance) {
                OrbitPlayer();
                canAttack = true;
            } else {
                Vector3 awayFromPlayer = (transform.position - player.position).normalized;
                Vector3 retreatPosition = transform.position + awayFromPlayer * 5;
                agent.SetDestination(retreatPosition);
            }
        }

        if (canAttack && attackTiming < 0) {
            inAttack = true;
            agent.SetDestination(player.position);
            agent.stoppingDistance = meleeRange;
            
            if (distToPlayer < agent.stoppingDistance) {
                agent.SetDestination(transform.position);
                StartCoroutine(AttackRight());
            }
        }

        // walking animation
        if (agent.velocity.magnitude != 0) {
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }

        // always looking at the player (except during attack)
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero && !inAni) {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }


        hitCooldown -= Time.deltaTime;
        attackTiming -= Time.deltaTime;
        changeDir -= Time.deltaTime;
    }

    private void OrbitPlayer() {
        Vector3 toEnemy = transform.position - player.position;
        toEnemy.y = 0;
        toEnemy.Normalize();

        // determins tan line to orbit/strafe
        Vector3 orbitDirection = orbitClockwise
                                ? Vector3.Cross(Vector3.up, toEnemy)
                                : Vector3.Cross(toEnemy, Vector3.up);

        Vector3 orbitTarget = transform.position + orbitDirection * 5f;
        agent.SetDestination(orbitTarget);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerAttack")) {
            DecHealth();
        }
    }

    public void DecHealth() {
        if (!hit) { 
            blood.Play();
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
        inAni = true;
        animator.SetTrigger("DraculaDeath");
        yield return new WaitForSeconds(8);
        this.gameObject.SetActive(false);
        Dead = true;
        inAni = false;
    }

    IEnumerator AttackLeft() {
        inAni = true;
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
        inAni = false;
    }
    
    IEnumerator AttackRight() {
        inAni = true;
        animator.SetTrigger("RightPunch");
        attackTiming = 20f;
        yield return new WaitForSeconds(1.2f);
        attacks.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        attacks.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        agent.stoppingDistance = orbitRange;
        canAttack = false;
        inAttack = false;
        inAni = false;
    }
}
