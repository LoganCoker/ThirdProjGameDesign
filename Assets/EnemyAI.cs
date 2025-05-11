using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour{
    public NavMeshAgent agent;

    public Transform player;

    public LayerMask whatisGround, whatisPlayer;

    public float health;
    public Animator animator;

    // patrolling

    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // attacking
    public float timeBetweenAtks;
    bool alreadyAttacked;
    public GameObject projectile;

    // states 
    public float sightRange, atkRange;
    public bool playerinSightRange, playerinAtkRange;

    private void Awake(){
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update(){
        // check for sight and atk range
        playerinSightRange = Physics.CheckSphere(transform.position, sightRange, whatisPlayer);
        playerinAtkRange = Physics.CheckSphere(transform.position, atkRange, whatisPlayer);
    
        if (!playerinSightRange && !playerinAtkRange) Patrolling();
        if (playerinSightRange && !playerinAtkRange) ChasePlayer();
        if (playerinAtkRange && playerinSightRange) AtkPlayer();

        // walking
        if (agent.velocity.magnitude != 0) {
            animator.SetBool("Walking", true);
        } else {
            animator.SetBool("Walking", false);
        }

    }

    private void Patrolling(){
        if (!walkPointSet) SearchWalkPoint();

        if(walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distancetoWalkPoint = transform.position - walkPoint;

        // reached walkpoint
        if (distancetoWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint(){
        // calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

       if (Physics.Raycast(walkPoint, -transform.up, 2f, whatisGround))
        walkPointSet = true;
    }


    private void ChasePlayer(){
        if(agent.isOnNavMesh)
        agent.SetDestination(player.position);
    }

    private void AtkPlayer(){
        // make sure enemy doesnt move
        if (agent.isOnNavMesh){
        agent.SetDestination(transform.position);
        }
        transform.LookAt(player);

        if(!alreadyAttacked){

            // attack sequence
            Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 8f, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAtk), timeBetweenAtks);
        }
    }

    private void ResetAtk(){
        alreadyAttacked = false;
    }

    public void TakeDmg(int dmg){
        health -= dmg;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);

    }

    private void DestroyEnemy(){
        Destroy(gameObject);
    }
}
