using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraScript : MonoBehaviour
{

    protected Animator animator;
    protected UniversalAdditionalCameraData UACD;
    protected Camera thisCamera;
    protected GameObject player;

    // Start is called before the first frame update
    protected void Start()
    {
        animator = GetComponent<Animator>();
        UACD = GetComponent<UniversalAdditionalCameraData>();
        player = GameObject.Find("Player");
        UACD.renderPostProcessing = Globals.postProcessing;
        thisCamera = GetComponent<Camera>();
    }

    protected void deathShake()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Death");
    }

    protected void slamShake()
    {
        if (Globals.shakeEnabled)
        {
            animator.SetTrigger("Slam");
        }
    }

    protected void revive()
    {
        if (Globals.shakeEnabled)
        {
            animator.SetTrigger("Revive");
        }
    }

    void land()
    {
        if (Globals.shakeEnabled) animator.SetTrigger("Land");
    }
}
