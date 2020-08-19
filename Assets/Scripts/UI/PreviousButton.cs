using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PreviousButton : MonoBehaviour
{

    public Button button;
    public Scener scener;

    void Start()
    {
        scener = Globals.scener;
        button.onClick.AddListener(previous);
    }

    void previous()
    {
        scener.prevScene();
        PauseMenu.unpause();
    }
}
