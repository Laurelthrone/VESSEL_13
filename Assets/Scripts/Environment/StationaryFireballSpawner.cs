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
    public float range;
    bool spawnerIEnumerator;

    // Start is called before the first frame update
    void Start()
    { 
        thisSpawner = gameObject;
        spawnerIEnumerator = false;
        active = true;
        toBlue();
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
        yield return new WaitForSeconds(1);

        if (active && dist <= range)
        {
            toBlue();
            Sounder.PlaySound("shoot");
            Instantiate(prefab, new Vector3(thisSpawner.transform.position.x, thisSpawner.transform.position.y, 0), Quaternion.identity);
            yield return new WaitForSeconds(delay);
        }

        else if (!active)
        {
            yield return new WaitForSeconds(5);
            Sounder.PlaySound("restart");
            active = true;
            toBlue();
        }
        else
        {
            ColorUtility.TryParseHtmlString("555555", out Color color);
            glow.color = color;
        }

        spawnerIEnumerator = false;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Player") && player.getState() == "pound" && active == true)
        {
            Sounder.PlaySound("break");
            active = false;
            toRed();
        }
    }

    void toRed()
    {
        glow.color = Globals.FBSred;
    }
    void toBlue()
    {
        glow.color = Globals.FBSblue;
    }
}
