using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour {

    #region publics
    public Transform player;
    public NavMeshAgent agent;
    public int Health { get; private set; }
    #endregion

    #region private
    private bool hit;
    private float hitCooldown;
    #endregion

    void Start() {
        Health = 2;
    }

    void Update() {
        agent.SetDestination(player.position);
        if (hitCooldown < 0) {
            hitCooldown = 0;
            hit = false;
        }

        hitCooldown -= Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("PlayerAttack") && !hit) {
            decHealth();
            print("boss hit");
        }
    }

    public void decHealth() {
        Health--;
        hit = true;
        hitCooldown = 3f;
        if (Health == 0) {
            this.gameObject.SetActive(false);
        }
    }
}
