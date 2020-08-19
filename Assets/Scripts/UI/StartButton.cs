using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartButton : MonoBehaviour
{

    public Button button;
    public Scener scener;

    void Start()
    {
        scener = Globals.scener;
        button.onClick.AddListener(StartGame);
    }
        
    void StartGame()
    {
        scener.GoToScene("levelselect_crystal");
    }
}
