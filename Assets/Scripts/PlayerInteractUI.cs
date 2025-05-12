using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractUI : MonoBehaviour{
    [SerializeField] private GameObject containerGameObject;
    
    private void Show(){
        containerGameObject.SetActive(true);
    }

    private void Hide(){
        containerGameObject.SetActive(false);
    }

    
}
