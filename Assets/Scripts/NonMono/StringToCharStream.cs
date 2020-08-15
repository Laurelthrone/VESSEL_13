using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StringToCharStream
{
    private string toWrite;
    private int length;
    private int step;

    public StringToCharStream(string a)
    {
        toWrite = a;
        length = toWrite.Length;
        step = 0;
    }

    public void changeText(string a)
    {
        toWrite = a;
        length = toWrite.Length;
        step = 0;
    }

    public int getLength()
    {
        return length;
    }

    public int getStep()
    {
        return step;
    }

    public char returnText()
    {
        char output = toWrite[step];
        step++;
        return output;
    }

}
