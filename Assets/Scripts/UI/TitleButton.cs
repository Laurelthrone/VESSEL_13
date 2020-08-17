using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleButton : MonoBehaviour
{

    public Button button;
    public Scener scener;

    void Start()
    {
        button.onClick.AddListener(ReturnToTitle);
    }
        
    void ReturnToTitle()
    {
        string[] nopause = new string[4];
        nopause[0] = "titlescreen";
        nopause[1] = "levelselect_crystal";
        nopause[2] = "levelselect_lab";
        nopause[3] = "settings";
        scener.GoToScene("titlescreen");
        foreach (string x in nopause)
        {
            if (Scener.currentScene.Contains(x)) return;
        }   
        PauseMenu.unpause();
    }
}
