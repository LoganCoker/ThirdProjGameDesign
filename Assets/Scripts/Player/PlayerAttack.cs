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
    private PlayerInMove playerMovement;
    private float attackTime;
    private int attackCount = 1;
    private bool inAttack;
    private float attackSpacing = 1.5f;

    private string swing1SoundName = "SwordSwing1";
    private string swing2SoundName = "SwordSwing2";
    private string swing3SoundName = "SwordSwing3";
    #endregion

    void Start() {
        playerInput = Game.PlayerInput;
        attackPattern = transform.GetChild(0).GetChild(0);
        playerMovement = GetComponent<PlayerInMove>();
    }

    void Update() {
        if (attackTime < 0) {
            attackTime = 0;
            attackCount = 1;
        }

        if (playerInput.Attack && !inAttack && attackTime <= 0 && attackCount == 1) {
            playerMovement.InAction = true;
            StartCoroutine(FirstAttack());
            animator.SetTrigger("Attack1");
        }
        if (playerInput.Attack && !inAttack && attackTime > 0 && attackCount == 2) {
            playerMovement.InAction = true;
            StartCoroutine(SecondAttack());
            animator.SetTrigger("Attack2");
        }

        if (playerInput.Attack && !inAttack && attackTime > 0 && attackCount == 3) {
            playerMovement.InAction = true;
            StartCoroutine(ThirdAttack());
            animator.SetTrigger("Attack3");
        }

        attackTime -= Time.deltaTime;
    }

    #region Attact 'animations'
    IEnumerator FirstAttack() {
        AudioManager.Instance.Play(swing1SoundName);

        Transform attack = attackPattern.transform.GetChild(0);
        inAttack = true;
        yield return new WaitForSeconds(.1f);

        Vector3 initPos = new(0.4f, 0.4f, 0.4f);
        Vector3 finalPos = new(0f, -0.3f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.1f);
        attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        playerMovement.InAction = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator SecondAttack() {
        AudioManager.Instance.Play(swing2SoundName); 

        Transform attack = attackPattern.transform.GetChild(1);
        inAttack = true;
        yield return new WaitForSeconds(.2f);

        Vector3 initPos = new(0.4f, 0.2f, 0.4f);
        Vector3 finalPos = new(-0.2f, 0.25f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.4f);
        attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        playerMovement.InAction = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator ThirdAttack() {
        AudioManager.Instance.Play(swing3SoundName); 

        Transform attack = attackPattern.transform.GetChild(2);
        inAttack = true;
        yield return new WaitForSeconds(.45f);

        Vector3 initPos = new(-0.25f, 0.5f, 0.4f);
        Vector3 finalPos = new(0.4f, -0.2f, 0.4f);

        attack.localPosition = initPos;
        attack.gameObject.SetActive(true);
        while (attack.localPosition != finalPos) {
            attack.localPosition = Vector3.MoveTowards(attack.localPosition, finalPos, attackSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(.2f);
        attackTime = .3f;
        attackCount = 1;
        inAttack = false;
        playerMovement.InAction = false;
        attack.gameObject.SetActive(false);
    }
    #endregion
}
