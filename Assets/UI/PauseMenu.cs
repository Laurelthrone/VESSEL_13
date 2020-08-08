using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public static bool isPaused = false;
    public static Animator pauseanim;
    public static float deltatime;

    // Start is called before the first frame update
    void Start()
    {
        pauseanim = GetComponent<Animator>();
        deltatime = Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                unpause();
            }
            else pause();
            return;
        }
    }

    public static void unpause()
    {
        pauseanim.SetTrigger("Unpause");
        Time.timeScale = 1;
        Time.fixedDeltaTime = deltatime;
        isPaused = false;
        return;
    }

    public static void pause()
    {
        pauseanim.SetTrigger("Pause");
        Time.timeScale = 0;
        Time.fixedDeltaTime = 0;
        isPaused = true;
        return;
    }
}
