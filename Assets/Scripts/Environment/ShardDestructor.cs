using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardDestructor : MonoBehaviour
{
    void OnDestroy()
    {
        Globals.shardCounter--;
    }
}
