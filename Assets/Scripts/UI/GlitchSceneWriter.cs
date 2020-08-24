using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class GlitchSceneWriter : OnscreenWriterBase
{
    // Start is called before the first frame update
    void Start()
    {
        nlines = 12;
        prepForWriting();
        lines = new string[nlines];
        lines[0] = "/////////0x;;;;;;";
        lines[1] = "<><><>IT WHAT<><><>";
        lines[2] = "///%???%//5/???////";
        lines[3] = "-Rebooting comms-";
        lines[4] = "Where did it go?";
        lines[5] = "Is this the end?";
        lines[6] = "It's";
        lines[7] = "It's not fair.";
        lines[8] = "We won. Why is this happening?";
        lines[9] = "Can we still stop it?";
        lines[10] = "I... I think there's one more thing we can try.";
        lines[11] = "... but I suspect it was never meant to be.";
    }
}
