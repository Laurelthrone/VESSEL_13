using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastGroundDetector : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground") SendMessageUpwards("Flip"); 
    }   
}
