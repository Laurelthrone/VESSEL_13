using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FireballCutsceneWriter : OnscreenWriterBase
{
    // Start is called before the first frame update
    void Start()
    {
        nlines = 6;
        prepForWriting();
        lines = new string[nlines];
        lines[0] = "Ah, so it has awoken.";
        lines[1] = "Interesting.";
        lines[2] = "It's been a long time since the last.";
        lines[3] = "Too long.";
        lines[4] = "Allow it to proceed for now.";
        lines[5] = "Only step in if it becomes a problem.";
    }
}
