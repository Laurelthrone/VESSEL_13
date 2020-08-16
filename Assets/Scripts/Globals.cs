using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{

    public static bool shakeEnabled = true;
    public static bool postProcessing = true;
    public static string currentScene = "1";
    public static int unlocked = 1;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
