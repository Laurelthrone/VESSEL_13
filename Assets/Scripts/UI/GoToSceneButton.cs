using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToSceneButton : MonoBehaviour
{

    public Button button;
    public Scener scener;
    public string target;

    void Start()
    {
        scener = Globals.scener;
        button.onClick.AddListener(GoTo);
    }

    void GoTo() => scener.GoToScene(target);
}