using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{

    Rigidbody2D fireballBody;
    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        fireballBody = GetComponent<Rigidbody2D>();
    }   

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2 (velocity, 0);
        fireballBody.velocity = movement;
    }
}
