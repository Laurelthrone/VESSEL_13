using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DesktopButton : MonoBehaviour
{

    public Button button;

    // Update is called once per frame
    void Start()
    {
        button.onClick.AddListener(Desktop);
    }
        
    void Desktop()
    {
       Application.Quit();
    }
}
