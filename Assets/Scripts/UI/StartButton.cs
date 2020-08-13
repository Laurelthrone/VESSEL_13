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
        button.onClick.AddListener(StartGame);
    }
        
    void StartGame()
    {
        scener.GoToScene(Globals.currentScene);
    }
}
