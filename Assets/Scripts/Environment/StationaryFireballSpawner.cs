using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryFireballSpawner : MonoBehaviour
{
    public Player player;
    private bool active = true;
    public float delay;
    public GameObject prefab;
    private GameObject thisSpawner;
    private BoxCollider2D trigger;

    // Start is called before the first frame update
    void Start()
    {
        thisSpawner = gameObject;
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(delay);
        while (true)
        {
            while (active)
            {
                Sounder.PlaySound("shoot");
                Instantiate(prefab, new Vector3(thisSpawner.transform.position.x, thisSpawner.transform.position.y, 0), Quaternion.identity);
                yield return new WaitForSeconds(delay);
            }
            yield return new WaitForSeconds(delay*2);
            Sounder.PlaySound("restart");
            active = true;
        }
    }

    void OnTriggerEnter2D()
    {
        if(player.playerState == "pound" && active == true)
        {
            Sounder.PlaySound("break");
            active = false;
        }
    }
}
