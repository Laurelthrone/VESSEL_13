using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public Button button;
    private Scener scener;
    public int num;
    private Text text;

    void Start()
    {
        scener = Globals.scener;
        text = gameObject.GetComponentInChildren<Text>();
        button.onClick.AddListener(startLevel);
        text.text = num.ToString();
        if (Globals.unlocked < num)
        {
            text.color = new Color(.7f, .3f, .7f);
        }
    }

    private void startLevel()
    {
        if (Globals.unlocked >= num)
        {
            scener.GoToScene(num.ToString());
        }
    }

}
