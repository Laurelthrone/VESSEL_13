using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounder : MonoBehaviour
{

    public static AudioClip jumpSound, dropSound, landSound, deathSound, orbSound, loadSound, boxSound;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        jumpSound = Resources.Load<AudioClip>("jump");
        dropSound = Resources.Load<AudioClip>("drop");
        landSound = Resources.Load<AudioClip>("land");
        deathSound = Resources.Load<AudioClip>("death");
        orbSound = Resources.Load <AudioClip>("orb");
        boxSound = Resources.Load<AudioClip>("box");
        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = .5f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void PlaySound (string clip)
    {
        switch (clip)
        {
            case "jump":
                audioSrc.PlayOneShot(jumpSound);
                break;
            case "drop":
                audioSrc.PlayOneShot(dropSound);
                break;
            case "land":
                audioSrc.PlayOneShot(landSound);
                break;
            case "death":
                audioSrc.PlayOneShot(deathSound);
                break;
            case "orb":
                audioSrc.PlayOneShot(orbSound);
                break;
            case "box":
                audioSrc.PlayOneShot(boxSound);
                break;
        }
    }

}
