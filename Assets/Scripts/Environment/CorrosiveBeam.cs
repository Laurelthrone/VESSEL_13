using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CorrosiveBeam : MonoBehaviour
{
    void OnCollisionEnter2D(Collider2D collider)
    {
        Debug.Log(1);
        if (collider.gameObject.tag == "Player")
        {
            Debug.Log(2);
            collider.gameObject.SendMessage("die");
        }
    }
}
