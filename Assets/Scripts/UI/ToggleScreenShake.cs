using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleScreenShake : MonoBehaviour
{

    public Button button;
    private Text text;

    // Update is called once per frame
    void Start()
    {
        text = GetComponentInChildren<Text>();
        button.onClick.AddListener(toggleShake);
        if (Globals.shakeEnabled == true)
        {
            text.text = "Screen Shake: On";
        }
        else text.text = "Screen Shake: Off";
    }

    void toggleShake()
    {
        Globals.shakeEnabled = !Globals.shakeEnabled;
        if (Globals.shakeEnabled == true)
        {
            text.text = "Screen Shake: On";
        }
        else text.text = "Screen Shake: Off";
    }
}