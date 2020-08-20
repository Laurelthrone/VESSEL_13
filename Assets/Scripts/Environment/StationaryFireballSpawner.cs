using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class StationaryFireballSpawner : MonoBehaviour
{

    public Player player;
    public float delay;
    public GameObject prefab;
    public float range;
    public Light2D glow;

    private bool active = true;
    private BoxCollider2D trigger;
    private float dist;
    private bool coroutineActive;
    private Color FBSred, FBSblue, FBSgrey;

    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#D43D3D", out FBSred);
        ColorUtility.TryParseHtmlString("#9BE7FF", out FBSblue);
        ColorUtility.TryParseHtmlString("#555555", out FBSgrey);
        coroutineActive = false;
        active = true;
        toBlue();
    }

    void Update()
    {
        dist = Vector2.Distance(player.transform.position, gameObject.transform.position);
        if(!coroutineActive)
        {
            StartCoroutine(Spawner());
            coroutineActive = true;
        }
    }

    IEnumerator Spawner()
    {
        yield return new WaitForSeconds(1);

        if (active && dist <= range)
        {
            toBlue();
            Sounder.PlaySound("shoot");
            Instantiate(prefab, new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, 0), Quaternion.identity);
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
            glow.color = FBSgrey;
        }

        coroutineActive = false;
    }

    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.gameObject.tag.Equals("Player") && player.getState() == "slam" && active == true)
        {
            Sounder.PlaySound("break");
            active = false;
            toRed();
        }
    }

    void toRed() => glow.color = FBSred;
    void toBlue() => glow.color = FBSblue;
}
