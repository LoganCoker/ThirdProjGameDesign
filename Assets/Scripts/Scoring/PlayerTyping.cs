using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerTyping : MonoBehaviour {

    public TMP_InputField letter0;
    public TMP_InputField letter1;
    public TMP_InputField letter2;

    private bool submited;

    public string FormulateName() {
        string res = "";

        res += letter0.text;
        res += letter1.text;
        res += letter2.text;

        return res;
    }

    public void Submit() {
        if (!submited) {
            submited = true;
            string name = FormulateName();

            if (name == "") {
                return;
            }
            
            HighScoreManager.Instance.SubmitScore(Game.GetFinalScore(), name);
        }
    }
}
