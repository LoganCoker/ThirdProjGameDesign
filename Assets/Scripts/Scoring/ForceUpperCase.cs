using TMPro;
using UnityEngine;

public class ForceUpperCase : MonoBehaviour {
    public TMP_InputField inputField;

    void Start() {
        inputField.characterLimit = 1;
        inputField.onValueChanged.AddListener(OnValueChanged);
    }

    void OnValueChanged(string text) {
        if(string.IsNullOrEmpty(text)) return;

        char c = char.ToUpper(text[0]);

        if (char.IsLetter(c)) {
            inputField.text = c.ToString(); 
        } else {
            inputField.text = ""; 
        }
    }
}
