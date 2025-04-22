using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour {


    public static InputController Input { get; private set; }
    public static Game Instance { get; private set; }

    void Start() {
        Instance = this;
        Input = new InputController();

        // for testing
        Input.Enable();
    }


    void Update() {
        
    }
}
