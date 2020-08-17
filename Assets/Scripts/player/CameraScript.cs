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
        Debug.Log("Received");
        if (Globals.shakeEnabled) animator.SetTrigger("Death");
    }

    void slamShake()
    {
        Debug.Log("Received");
        if (Globals.shakeEnabled)
        {
            Debug.Log("Sent");
            animator.SetTrigger("Slam");
        }
    }

    void land()
    {
        Debug.Log("Received");
        if (Globals.shakeEnabled) animator.SetTrigger("Land");
    }
}
