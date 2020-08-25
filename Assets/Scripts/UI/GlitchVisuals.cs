using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlitchVisuals : MonoBehaviour
{
    Material glitchShader;
    public GameObject player;
    public bool enableRotate = true;
    public bool isScreenCover = false;
    SpriteRenderer sprite;

    // Start is called before the first frame update
    void Start()
    {
        if (isScreenCover) sprite = GetComponent<SpriteRenderer>();
        glitchShader = GetComponent<Renderer>().material;
        glitchShader.renderQueue = 2000;
    }

    // Update is called once per frame
    void Update()
    {
        if (enableRotate) gameObject.transform.eulerAngles = new Vector3(-10 * (gameObject.transform.position.y - player.transform.position.y), 2 * (gameObject.transform.position.x - player.transform.position.x), 0);
        if (isScreenCover) sprite.color = new Color(1, 1, 1, ((400 - (400 - player.transform.position.x)) / 1200) -.25f);
        glitchShader.SetVector("Pos", player.transform.position);
    }
}
