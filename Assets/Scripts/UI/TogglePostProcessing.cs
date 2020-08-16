using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TogglePostProcessing : MonoBehaviour
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
        Globals.postProcessing = !Globals.postProcessing;
        updateText();
    }

    void updateText()
    {
        if (Globals.postProcessing == true)
        {
            text.text = "Postprocessing: On";
        }
        else text.text = "Postproccessing: Off";
    }
}