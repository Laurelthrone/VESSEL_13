using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{

    public Button button;
    public Scener scener;

    // Update is called once per frame
    void Start()
    {
        button.onClick.AddListener(ReturnToTitle);
    }
        
    void ReturnToTitle()
    {
        scener.GoToScene("titlescreen");

        if (Scener.currentScene != "settings") PauseMenu.unpause();
    }
}
