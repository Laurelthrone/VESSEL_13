using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class crate : MonoBehaviour
{

    //Public
    public GameObject player;
    public GameObject prefab;
    
    //Private
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

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D target)
    {
        if (target.gameObject.tag.Equals("Player") && playerScript.playerState == "pound")
        {
            Sounder.PlaySound("box");
            Instantiate(prefab, new Vector3(thisCrate.transform.position.x, thisCrate.transform.position.y, 0), Quaternion.identity);
            Destroy(thisCrate);
        }
    }
}
