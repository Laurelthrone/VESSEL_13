using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wallbounce : MonoBehaviour
{
    public GameObject player;
    Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent(typeof(Player)) as Player;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag.Equals("Player") && playerScript.getState() == "pound")
        {
            player.SendMessage("wallbounce");
        }
    }
}
