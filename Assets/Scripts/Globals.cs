using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{

    //Data for graphic settings
    public static bool shakeEnabled = true;
    public static bool postProcessing = true;

    //Data for level management
    public static string currentScene = "1";
    public static int unlocked = 1;
    public static Scener scener;

    //Data for crate shards
    public static int shardCounter;

    //Data for timescale selector
    public static float timeScale;
    public static int speedSelected = 3;
    public static readonly float[] speeds = { .25f, .50f, .75f, 1, 1.25f, 1.5f, 1.75f, 2 };

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        if (!PlayerPrefs.HasKey("shakeEnabled")) return;
        shakeEnabled = PlayerPrefs.GetInt("shakeEnabled") == 1;
        postProcessing = PlayerPrefs.GetInt("postProcessing") == 1;
        unlocked = PlayerPrefs.GetInt("unlocked");
        timeScale = PlayerPrefs.GetFloat("timeScale");
    }
}
