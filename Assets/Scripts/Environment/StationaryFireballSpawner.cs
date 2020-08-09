using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class StationaryFireballSpawner : MonoBehaviour
{

    public Player player;
    private bool active = true;
    public float delay;
    public GameObject prefab;
    private GameObject thisSpawner;
    private BoxCollider2D trigger;
    public Light2D glow;
    private bool subroutine;

    // Start is called before the first frame update
    void Start()
    {
        thisSpawner = gameObject;
        StartCoroutine(Spawner());
        subroutine = true;
        blue();
    }

    // Update is called once per frame
    void Awake()
    {
        StartCoroutine(Spawner());
    }

    IEnumerator Spawner()
    {
        float dist = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if (dist <= 20)
        {
        
            while (active)
            {
                Sounder.PlaySound("shoot");
                Instantiate(prefab, new Vector3(thisSpawner.transform.position.x, thisSpawner.transform.position.y, 0), Quaternion.identity);
                yield return new WaitForSeconds(delay);
            }
        
        float add = 0;
        if (delay > 1) add = 3;
        yield return new WaitForSeconds((delay * 2) + add);
        Sounder.PlaySound("restart");
        active = true;
        blue();
        }
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Player") && player.playerState == "pound" && active == true)
        {
            Debug.Log("oof");
            Sounder.PlaySound("break");
            active = false;
            red();
        }
    }

    void red() => glow.color = HexConvert.toColor("D43D3D");
    void blue() => glow.color = HexConvert.toColor("9BE7FF");
}
