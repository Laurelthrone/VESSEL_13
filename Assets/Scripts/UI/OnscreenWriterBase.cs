using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class OnscreenWriterBase : MonoBehaviour
{
    protected StringToCharStream processor;
    
    protected Text textfield;
    
    protected int nlines;
    protected int currentLine = 0;
    
    protected string[] lines;
    
    protected float waittime = 2;

    protected bool runLoop = true;
    protected bool coroutineActive = false;


    protected void prepForWriting()
    {
        textfield = gameObject.GetComponent<Text>();
        textfield.text = "";
        processor = new StringToCharStream("");
    }

    protected IEnumerator WriteLineAsChars(float waittime, string textsound)
    {
        yield return new WaitForSeconds(waittime);
        textfield.text = "";
        for (int i = 0; i < processor.getLength(); i++)
        {
            if (!runLoop) break;
            char write = processor.returnText();
            textfield.text += write;
            if (write != ' ') Sounder.PlaySound(textsound);
            yield return new WaitForSeconds(.1f);
        }
        currentLine++;
        UnityEngine.Debug.Log(currentLine);
        UnityEngine.Debug.Log(nlines);
        if (currentLine >= nlines) yield return new WaitForSeconds(waittime);
        coroutineActive = false;
    }

    void Update()
    {
        if (!runLoop) return;
        if (Player.playerState == "dead")
        {
            endDialog();
            return;
        }   

        if (!coroutineActive)
        {
            if (currentLine < nlines)
            {
                processor.changeText(lines[currentLine]);
                StartCoroutine(WriteLineAsChars(waittime, "text"));
                coroutineActive = true;
            } else
            {
                textfield.text = "";
                runLoop = false;
                SendMessage("dialogTrigger", gameObject);
            }
        }
    }

    protected void endDialog()
    {
        textfield.text = "";
        runLoop = false;
    }
}
