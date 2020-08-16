using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class DarkLabCutsceneWriter : OnscreenWriterBase
{
    public GameObject holdingCell;

    // Start is called before the first frame update
    void Start()
    {
        prepForWriting();
        nlines = 6;
        lines = new string[nlines];
        lines[0] = "Oh dear.";
        lines[1] = "It made it through the old lab?";
        lines[2] = "We might have a problem on our hands.";
        lines[3] = "Now, don't panic yet.";
        lines[4] = "If I remember correctly... it should be at";
        lines[5] = "Just the right spot for us to slow it down.";
    }

    private void dialogTrigger()
    {
        Destroy(holdingCell);
        Destroy(gameObject);
    }
}
