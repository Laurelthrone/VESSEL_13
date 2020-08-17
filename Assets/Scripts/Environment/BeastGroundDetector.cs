using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class BeastGroundDetector : MonoBehaviour
{
    int a = 0;
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground") SendMessageUpwards("Flip"); 
    }   
}
