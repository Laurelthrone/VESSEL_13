using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class FireballCutsceneWriter : Writer
{
    private int nlines = 6;
    private string[] lines;
    private float waittime = 2;

    // Start is called before the first frame update
    void Start()
    {
        prepForWriting();
        lines = new string[nlines];
        lines[0] = "Ah, so it has awoken.";
        lines[1] = "Interesting.";
        lines[2] = "It's been a long time since the last.";
        lines[3] = "Too long.";
        lines[4] = "Allow it to proceed for now.";
        lines[5] = "Only step in if it becomes a problem.";
    }

    void Update()
    {
        if (!coroutineActive && currentLine < nlines)
        {
            processor.changeText(lines[currentLine]);
            StartCoroutine(WriteLineAsChars(waittime,"text"));
            currentLine++;
            coroutineActive = true;
        }
    }
}
