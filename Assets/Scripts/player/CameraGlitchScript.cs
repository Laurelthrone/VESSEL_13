using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CameraGlitchScript : CameraScript
{
    void land()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Glitch");
    }

    void Update() => animator.SetFloat("Glitchiness", math.pow((player.transform.position.x * .001f),2));
}
