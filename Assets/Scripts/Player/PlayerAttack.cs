using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    #region publics
    public float attackSpeed;
    public Transform bodyRot;
    #endregion

    #region privates
    private InputController.PlayerActions input;
    private Transform attackPattern;
    private float attackTime;
    private int attackCount = 1;
    private bool inAttack;
    private float attackSpacing = 2;
    #endregion

    void Start() {
        attackSpeed *= 20;
        input = Game.Input.Player;
        attackPattern = transform.GetChild(0).GetChild(2);
    }

    void Update() {
        //transform.rotation = bodyRot.rotation;
        if (attackTime < 0) {
            attackTime = 0;
            attackCount = 1;
        }

        if (input.Attack.WasPressedThisFrame() && !inAttack && attackTime <= 0 && attackCount == 1) {
            StartCoroutine(FirstAttack());
        }

        if (input.Attack.WasPressedThisFrame() && !inAttack && attackTime > 0 && attackCount == 2) {
            StartCoroutine(SecondAttack());
        }

        if (input.Attack.WasPressedThisFrame() && !inAttack && attackTime > 0 && attackCount == 3) {
            StartCoroutine(ThirdAttack());
        }

        if (input.Attack.WasPressedThisFrame() && !inAttack && attackTime > 0 && attackCount == 4) {
            StartCoroutine(ForthAttack());
        }

        if (input.Attack.WasPressedThisFrame() && !inAttack && attackTime > 0 && attackCount == 5) {
            StartCoroutine(FifthAttack());
        }


        attackTime -= Time.deltaTime;
    }

    IEnumerator FirstAttack() {
        // rotate x 0 -> 180
        // rotate z 20 -> -20
        Transform attack = attackPattern.transform.GetChild(0);
        inAttack = true;

        float xRot = 0;
        float zRot = 20f;

        attack.rotation = Quaternion.Euler(0, 0, zRot);
        attack.gameObject.SetActive(true);
        while (xRot < 180) {
            attack.rotation = Quaternion.Euler(xRot, 0, zRot);

            xRot += attackSpeed * Time.deltaTime;
            zRot -= attackSpeed * 2/9 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator SecondAttack() {
        // rotate y 0 -> -180
        // rotate z -110 -> -70
        Transform attack = attackPattern.transform.GetChild(1);
        inAttack = true;

        float yRot = 0;
        float zRot = -110f;

        attack.rotation = Quaternion.Euler(0, yRot, zRot);
        attack.gameObject.SetActive(true);
        while (yRot > -180) {
            attack.rotation = Quaternion.Euler(0, yRot, zRot);

            yRot -= attackSpeed * Time.deltaTime;
            zRot += attackSpeed * 2/9 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator ThirdAttack() {
        // rotate x 180 -> 0
        // rotate z 20 -> -20
        Transform attack = attackPattern.transform.GetChild(2);
        inAttack = true;

        float xRot = 180;
        float zRot = 20f;

        attack.rotation = Quaternion.Euler(xRot, 0, zRot);
        attack.gameObject.SetActive(true);
        while (xRot > 0) {
            attack.rotation = Quaternion.Euler(xRot, 0, zRot);

            xRot -= attackSpeed * Time.deltaTime;
            zRot -= attackSpeed * 2 / 9 * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator ForthAttack() {
        // rotate y 0 -> 180 (R)
        // rotate y 180 -> 0 (L)
        Transform attack = attackPattern.transform.GetChild(3);
        inAttack = true;

        float yRot = 0;

        attack.rotation = Quaternion.Euler(0, yRot, 90);
        attack.gameObject.SetActive(true);
        while (yRot < 180) {
            attack.rotation = Quaternion.Euler(0, yRot, 90);

            yRot += attackSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        attackTime = attackSpacing;
        attackCount++;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
    
    IEnumerator FifthAttack() {
        // rotate x 0 -> 180
        Transform attack = attackPattern.transform.GetChild(4);
        inAttack = true;

        float xRot = 0;

        attack.rotation = Quaternion.Euler(xRot, 0, 0);
        attack.gameObject.SetActive(true);
        while (xRot < 180) {
            attack.rotation = Quaternion.Euler(xRot, 0, 0);

            xRot += attackSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        attackTime = .3f;
        attackCount = 1;
        inAttack = false;
        attack.gameObject.SetActive(false);
    }
}
