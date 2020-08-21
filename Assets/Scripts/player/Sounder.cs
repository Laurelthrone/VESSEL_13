using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounder : MonoBehaviour
{

    public static AudioClip jumpSound, dropSound, landSound, deathSound, orbSound, loadSound, boxSound, breakSound, restartSound, shootSound, reviveSound, textSound;
    static AudioSource audioSrc;
    static IDictionary<string, AudioClip> clipNames = new Dictionary<string, AudioClip>();

    // Start is called before the first frame update
    void Start()
    {
        shootSound = Resources.Load<AudioClip>("shoot");
        jumpSound = Resources.Load<AudioClip>("jump");
        dropSound = Resources.Load<AudioClip>("drop");
        landSound = Resources.Load<AudioClip>("land");
        deathSound = Resources.Load<AudioClip>("death");
        orbSound = Resources.Load <AudioClip>("orb");
        boxSound = Resources.Load<AudioClip>("box");
        breakSound = Resources.Load<AudioClip>("break");
        restartSound = Resources.Load<AudioClip>("restart");
        reviveSound = Resources.Load<AudioClip>("revive");
        textSound = Resources.Load<AudioClip>("text");
        audioSrc = GetComponent<AudioSource>();
        audioSrc.volume = .5f;

        if (!clipNames.ContainsKey(("jump")))
        {
            clipNames.Add("jump", jumpSound);
            clipNames.Add("drop", dropSound);
            clipNames.Add("land", landSound);
            clipNames.Add("death", deathSound);
            clipNames.Add("orb", orbSound);
            clipNames.Add("box", boxSound);
            clipNames.Add("break", breakSound);
            clipNames.Add("restart", restartSound);
            clipNames.Add("shoot", shootSound);
            clipNames.Add("revive", reviveSound);
            clipNames.Add("text", textSound);
        }
    }

    public static void PlaySound (string clip)
    {
        if (!audioSrc.isPlaying)
        {
            if (clip != "orb") audioSrc.pitch = (Random.Range(.9f, 1.1f));
            else audioSrc.pitch = 1;
        }
        audioSrc.PlayOneShot(clipNames[clip]);
    }

}
