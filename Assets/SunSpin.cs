using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunSpin : MonoBehaviour
{
    float rotation;

    void Start()
    {
        rotation = gameObject.transform.rotation.z;
    }

    // Update is called once per frame
    void Update()
    {
        rotation += 1f;
    }
}
