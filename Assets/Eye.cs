using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eye : MonoBehaviour
{

    GameObject player;
    Vector2 direction;
    float angle;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        direction = player.transform.position -gameObject.transform.position;
        angle = Mathf.Atan2(direction.y, direction.x);
        gameObject.transform.eulerAngles = new Vector3 (0, 0, angle * Mathf.Rad2Deg);
    }
}
