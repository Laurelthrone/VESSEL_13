using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedButton : MonoBehaviour
{
    public Button button;
    private Text text;
    private string[] texts = { "25%", "50%", "75%", "100%", "125%", "150%", "175%", "200%" };

    void Start()
    {
        text = GetComponentInChildren<Text>();
        button.onClick.AddListener(ChangeSpeed);
        updateText();
    }

    void ChangeSpeed()
    {
        Globals.speedSelected = (Globals.speedSelected + 1) % 8;
        Globals.timeScale = Globals.speeds[Globals.speedSelected];
        updateText();
    }

    void updateText()
    {
        text.text = "Game Speed: " + texts[Globals.speedSelected];
    }
}
