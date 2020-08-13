using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenCrateShard : MonoBehaviour
{
    public GameObject holder;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in holder.transform)
        {
            if (child.name != "Collider")
                GameObject.Destroy(child.gameObject, Random.Range(0f, 2.0f));
        }
        GameObject.Destroy(holder, 3f);
    }
}
