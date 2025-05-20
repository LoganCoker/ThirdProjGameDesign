using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

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
    private float distTolerance = .5f;
    private float attackTiming = 5f;
    private float orbitRange;
    private float meleeRange = 1f;
    private bool hit;
    private bool canAttack;
    private bool inAttack;
    private bool inAni;
    #endregion

    void Start() {
        Health = health;
        attacks = transform.GetChild(1);
        blood = GetComponent<ParticleSystem>();
        agent.updateRotation = false;
        orbitRange = agent.stoppingDistance;
    }

    void Update() {
      
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
                canAttack = true;
                agent.SetDestination(transform.position);
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

            if (distToPlayer < agent.stoppingDistance + distTolerance) {
                agent.SetDestination(transform.position);
                StartCoroutine(AttackRight());
                agent.stoppingDistance = orbitRange;
            }
        }

        // always looking at the player (except during attack)
        Vector3 directionToPlayer = player.position - transform.position;
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero && !inAni) {
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // walking
        if (agent.velocity.magnitude != 0) {
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }


        hitCooldown -= Time.deltaTime;
        attackTiming -= Time.deltaTime;

        // keep off, counteract the spawning
        if (Dead) {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerAttack")) {
            DecHealth();
        }
    }

    public void DecHealth() {
        if (!hit) {
            AudioManager.Instance.Play("SkeleHurt");
            Health--;
            hit = true;
            blood.Play();
            hitCooldown = 0.5f;
            StopCoroutine(AttackRight());
            if (Health == 0) {
                inAttack = true;
                attackTiming = int.MaxValue;
                Game.AddScore(500);
                StartCoroutine(Death());
            } else {
                StartCoroutine(Knockback());
            } 
        }
    }

    IEnumerator Death() {
        inAni = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.enabled = false;
        animator.SetTrigger("Death");
        yield return new WaitForSeconds(1.5f);
        Dead = true;
    }

    IEnumerator Knockback() {
        inAni = true;
        agent.isStopped = true;
        animator.SetTrigger("CancelAni");
        attacks.GetChild(0).gameObject.SetActive(false);
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

    IEnumerator AttackRight() {
        inAni = true;
        animator.SetTrigger("RightPunch");
        attackTiming = Random.Range(2f, 7f);
        yield return new WaitForSeconds(1f);

        attacks.GetChild(0).gameObject.SetActive(true);
        yield return new WaitForSeconds(.2f);

        attacks.GetChild(0).gameObject.SetActive(false);
        canAttack = false;
        inAttack = false;
        inAni = false;
    }
}
