using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{

    public static bool shakeEnabled = true;
    public static string currentScene = "1";
    public static Color FBSred, FBSblue;

    // Start is called before the first frame update
    void Start()
    {
        ColorUtility.TryParseHtmlString("#D43D3D", out FBSred);
        ColorUtility.TryParseHtmlString("#9BE7FF", out FBSblue);
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
