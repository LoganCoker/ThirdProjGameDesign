using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour{
    public GameObject SwordOnPlayer;

    void Start(){
        SwordOnPlayer.SetActive(false);
    }

    private void OnTriggerStay(Collider other){
        if(other.gameObject.tag == "Player"){
            if(Input.GetKey(KeyCode.E)){
                this.gameObject.SetActive(false);
                SwordOnPlayer.SetActive(true);
            }
        }
    }
}