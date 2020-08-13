using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsButton : MonoBehaviour
{

    public Button button;
    public Scener scener;

    void Start()
    {
        button.onClick.AddListener(SettingsMenu);
    }

    void SettingsMenu()
    {
        scener.GoToScene("settings");
    }
}