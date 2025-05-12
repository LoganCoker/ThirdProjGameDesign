using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    #region publics
    public float attackSpeed;
    public Animator animator;
    #endregion

    #region privates
    private PlayerInput playerInput;
    private Transform attackPattern;
    private float attackTime;
    private int attackCount = 1;
    private bool inAttack;
    private float attackSpacing = 1;
    #endregion

    void Start() {
        playerInput = Game.PlayerInput;
        attackPattern = transform.GetChild(0).GetChild(0);
    }

    void Update() {
        if (attackTime < 0) {
            attackTime = 0;
            attackCount = 1;
        }

        if (playerInput.Attack && !inAttack && attackTime <= 0 && attackCount == 1) {
            StartCoroutine(FirstAttack());
            animator.SetTrigger("Attack1");
            attackTime = 4f;
        }
        if (playerInput.Attack && !inAttack && attackTime > 0 && attackCount == 2) {
            StartCoroutine(SecondAttack());
            animator.SetTrigger("Attack2");
            attackTime = 4f;
        }

        if (playerInput.Attack && !inAttack && attackTime > 0 && attackCount == 3) {
            StartCoroutine(ThirdAttack());
            animator.SetTrigger("Attack3");
            attackTime = 4f;
        }

        // nodfw
        if (playerInput.Attack && !inAttack && attackTime > 0 && attackCount == 4) {
            StartCoroutine(ForthAttack());
        }

        if (playerInput.Attack && !inAttack && attackTime > 0 && attackCount == 5) {
            StartCoroutine(FifthAttack());
        }

        attackTime -= Time.deltaTime;
    }

    #region Attact 'animations'
    IEnumerator FirstAttack() {
        Transform attack = attackPattern.transform.GetChild(0);
        inAttack = true;

        Vector3 initPos = new(-0.4f, 0.6f, 0.4f);
        Vector3 finalPos = new(0.4f, -0.4f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        //attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator SecondAttack() {
        Transform attack = attackPattern.transform.GetChild(1);
        inAttack = true;

        Vector3 initPos = new(0.4f, -0.2f, 0.4f);
        Vector3 finalPos = new(-0.4f, 0.2f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        //attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator ThirdAttack() {
        Transform attack = attackPattern.transform.GetChild(0);
        inAttack = true;

        Vector3 initPos = new(-0.4f, -0.4f, 0.4f);
        Vector3 finalPos = new(0.4f, 0.6f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        //attackTime = .3f;
        attackCount = 1;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator ForthAttack() {
        Transform attack = attackPattern.transform.GetChild(0);
        inAttack = true;

        Vector3 initPos = new(-0.7f, 0f, 0.4f);
        Vector3 finalPos = new(0.7f, 0f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator FifthAttack() {
        Transform attack = attackPattern.transform.GetChild(0);
        inAttack = true;

        Vector3 initPos = new(0f, 0.7f, 0.4f);
        Vector3 finalPos = new(0f, -0.5f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        attackTime = .3f;
        attackCount = 1;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    #endregion
}
