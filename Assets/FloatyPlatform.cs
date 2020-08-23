using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatyPlatform : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Physics2D.IgnoreLayerCollision(8, 16, true);
    }
}
