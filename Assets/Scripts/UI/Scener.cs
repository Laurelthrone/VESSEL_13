using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scener : MonoBehaviour
{

    public static string currentScene;
    public Scene scene;
    public Animator transition;
    public static bool transitionActive;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Init());
    }
        
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && SceneManager.GetActiveScene().name != "titlescreen")
        {
            if (Input.GetKeyDown("r")) reloadScene();
            if (Input.GetKeyDown("1")) nextScene();
        }
    }

    public void reloadScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().name));
    }

    public void nextScene()
    {
        /* if (currentScene == "35")
        {
            GoToScene("endscreen");
            return;
        } */

        int sceneNum;
        sceneNum = int.Parse(currentScene) + 1;
        StartCoroutine(LoadLevel(sceneNum.ToString()));
    }

    public void prevScene()
    {
        if (currentScene == "1")
        {
            GoToScene("titlescreen");
            return;
        }
        int sceneNum;
        sceneNum = int.Parse(currentScene) - 1;
        StartCoroutine(LoadLevel(sceneNum.ToString()));
    }


    IEnumerator LoadLevel(string sceneNum)
    {
        transitionActive = true;
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(1.5f);
        SceneManager.LoadScene(sceneNum);
    }

    IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        scene = SceneManager.GetActiveScene();
        currentScene = scene.name;
        if (int.TryParse(currentScene, out int a)) Globals.currentScene = currentScene;
        transitionActive = false;
    }

    public void GoToScene(string target)
    {
        StartCoroutine(LoadLevel(target));
    }
}
