using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballRight : MonoBehaviour
{

    Rigidbody2D fireball;
    public float velocity;

    // Start is called before the first frame update
    void Start()
    {
        fireball = GetComponent<Rigidbody2D>();
    }   

    // Update is called once per frame
    void Update()
    {
        Vector2 movement = new Vector2 (velocity, 0);
        fireball.velocity = movement;
    }
}
