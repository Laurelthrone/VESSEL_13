using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{

    public GameObject player;
    public GameObject prefab;
    
    private GameObject thisCrate;
    private Rigidbody2D body;
    private BoxCollider2D box;
    Player playerScript;

    // Start is called before the first frame update
    void Start()
    {
        thisCrate = gameObject;
        playerScript = player.GetComponent(typeof(Player)) as Player;
        body = GetComponent<Rigidbody2D>();
        box = GetComponent<BoxCollider2D>();
    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag.Equals("Player") && playerScript.getState() == "pound")
        {
            Sounder.PlaySound("box");
            player.SendMessage("crateBroken");
            Instantiate(prefab, new Vector3(thisCrate.transform.position.x, thisCrate.transform.position.y, 0), Quaternion.identity);
            Destroy(thisCrate);
        }
    }
}
