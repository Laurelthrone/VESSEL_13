using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastGroundDetector : MonoBehaviour
{
    int a = 0;
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground") SendMessageUpwards("Flip"); 
    }   
}
