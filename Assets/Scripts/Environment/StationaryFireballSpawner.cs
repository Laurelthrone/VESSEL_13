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
    float dist;
    float add = 0;
    public float range;
    bool spawnerIEnumerator;

    // Start is called before the first frame update
    void Start()
    {
        thisSpawner = gameObject;
        spawnerIEnumerator = false;
        active = true;
        blue();
    }

    void Update()
    {
        dist = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if(!spawnerIEnumerator)
        {
            StartCoroutine(Spawner());
            spawnerIEnumerator = true;
        }
    }

    IEnumerator Spawner()
    {
        Debug.Log("Coroutine started");
        Debug.Log(active && dist <= range);

        yield return new WaitForSeconds(1);

        if (active && dist <= range)
        {
            blue();
            Sounder.PlaySound("shoot");
            Instantiate(prefab, new Vector3(thisSpawner.transform.position.x, thisSpawner.transform.position.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }

        else if (!active)
        {
            yield return new WaitForSeconds((delay * 2) + add);
            Sounder.PlaySound("restart");
            active = true;
            blue();
        }
        else glow.color = HexConvert.toColor("555555");

        spawnerIEnumerator = false;
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
