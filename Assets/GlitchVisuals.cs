using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class GlitchVisuals : MonoBehaviour
{
    Material glitchShader;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        glitchShader = GetComponent<Renderer>().material;
        glitchShader.renderQueue = 2000;
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.eulerAngles = new Vector3(-10 * (gameObject.transform.position.y - player.transform.position.y), 2*(gameObject.transform.position.x - player.transform.position.x), 0);
        glitchShader.SetVector("Pos", player.transform.position);
    }
}
