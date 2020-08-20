using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraScript : MonoBehaviour
{

    Animator animator;
    UniversalAdditionalCameraData UACD;
    Camera thisCamera;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        UACD = GetComponent<UniversalAdditionalCameraData>();
        UACD.renderPostProcessing = Globals.postProcessing;
        thisCamera = GetComponent<Camera>();
    }

    void deathShake()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Death");
    }

    void slamShake()
    {
        if (Globals.shakeEnabled)
        {
            animator.SetTrigger("Slam");
        }
    }

    void land()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Land");
    }
}
