using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{

    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
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
