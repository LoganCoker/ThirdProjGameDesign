using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossAI : MonoBehaviour {

    public int Health { get; private set; }
    public bool Dead { get; private set; }

    #region publics
    public Transform player;
    public NavMeshAgent agent;
    public Animator animator;
    public LayerMask ground;
    public Slider healthBar;
    public int health;
    #endregion

    #region private
    private Transform attacks;
    private ParticleSystem blood;
    private float hitCooldown;
    private float distTolerance = .5f;
    private float attackTiming = 10f;
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
                agent.Move(Time.deltaTime * awayFromPlayer);
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
                agent.stoppingDistance = orbitRange;
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

        // health bar
        if (healthBar.value != (float)Health/health) {
            healthBar.value = (float)Health/health;
        }

        hitCooldown -= Time.deltaTime;
        attackTiming -= Time.deltaTime;
        changeDir -= Time.deltaTime;
    }

    private void OrbitPlayer() {
        Vector3 toEnemy = transform.position - player.position;
        toEnemy.y = 0;
        toEnemy.Normalize();
        DirectionCheck();

        // determins tan line to orbit/strafe
        Vector3 orbitDirection = orbitClockwise
                                ? Vector3.Cross(toEnemy, Vector3.up)    // clock
                                : Vector3.Cross(Vector3.up, toEnemy);   // counter clock

        Vector3 orbitTarget = transform.position + orbitDirection * 4f;
        agent.SetDestination(orbitTarget);
    }

    private void DirectionCheck() {
        Physics.Raycast(transform.position, -transform.right, out RaycastHit hitL, 5f, ground);
        Physics.Raycast(transform.position, transform.right, out RaycastHit hitR, 5f, ground);
        if (hitL.distance < 1f) {
            orbitClockwise = false;
        } else if (hitR.distance < 1f) {
            orbitClockwise = true;
        } else if (changeDir < 0) {
            orbitClockwise = !orbitClockwise;
            changeDir = 10f;
        }
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
            hitCooldown = .2f;
            if (Health == 0) {
                inAttack = true;
                attackTiming = int.MaxValue;
                StartCoroutine(Death());
            }
            StartCoroutine(Knockback());
        }
    }

    IEnumerator Death() {
        inAni = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
        animator.SetTrigger("DraculaDeath");
        yield return new WaitForSeconds(8);
        this.gameObject.SetActive(false);
        Dead = true;
    }

    IEnumerator Knockback() {
        inAni = true;
        agent.isStopped = true;
        animator.SetTrigger("CancelAni");
        Vector3 awayFromPlayer = (transform.position - player.position).normalized;

        float timer = 0f;
        while (timer < 0.3f) {
            transform.position += awayFromPlayer * 5f * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }
        
        agent.isStopped = false;
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
        attackTiming = 10f;
        yield return new WaitForSeconds(1.2f);
        attacks.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(.5f);
        attacks.GetChild(0).gameObject.SetActive(false);
        yield return new WaitForSeconds(2f);
        canAttack = false;
        inAttack = false;
        inAni = false;
    }
}
