using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraScript : MonoBehaviour
{

    Animator animator;
    UniversalAdditionalCameraData thisCamera;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        thisCamera = GetComponent<UniversalAdditionalCameraData>();
        thisCamera.renderPostProcessing = Globals.postProcessing;
    }

    void deathShake()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Death");
    }

    void slamShake()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Slam");
    }

    void land()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Land");
    }
}
