using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScreenShake : MonoBehaviour
{

    public Button button;
    private Text text;

    void Start()
    {
        text = GetComponentInChildren<Text>();
        button.onClick.AddListener(toggleShake);
        updateText();
    }

    void toggleShake()
    {
        Globals.shakeEnabled = !Globals.shakeEnabled;
        updateText();
    }
    
    void updateText()
    {
        if (Globals.shakeEnabled == true)
        {
            text.text = "Screen Shake: On";
        }
        else text.text = "Screen Shake: Off";
    }
}