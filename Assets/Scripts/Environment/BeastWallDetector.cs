using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeastWallDetector : MonoBehaviour
{
    int a = 0;
    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground") SendMessageUpwards("Flip");
    }
}