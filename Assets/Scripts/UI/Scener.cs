using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scener : MonoBehaviour
{

    public static string currentScene;
    public Scene scene;
    public static Animator transition;
    public static bool transitionActive;

    // Start is called before the first frame update
    void Start()
    {
        Globals.shardCounter = 0;
        Globals.scener = this;
        if ((transition = GetComponent<Animator>()) == null)
        {
            transition = gameObject.transform.Find("Image").GetComponent<Animator>();
        }
        transition.SetFloat("Speed", 0);
        StartCoroutine(Init());
        if (GameObject.Find("Player") == null) ready();
    }

    public static void ready()
    {
        transition.SetFloat("Speed", 1);
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
        /* if (currentScene == "49")
        {
            GoToScene("endscreen");
            return;
        } */

        int sceneNum;
        sceneNum = int.Parse(currentScene) + 1;
        if (sceneNum > Globals.unlocked) Globals.unlocked = sceneNum;
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
        PlayerPrefs.SetInt("unlocked", Globals.unlocked);
        PlayerPrefs.SetInt("postProcessing", Globals.postProcessing ? 1 : 0);
        PlayerPrefs.SetInt("shakeEnabled", Globals.shakeEnabled ? 1 : 0);
        PlayerPrefs.SetFloat("timeScale", Globals.timeScale);
        PlayerPrefs.Save();
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
        Physics.gravity = scene.name == "56" ? new Vector3(-3, -15) : new Vector3(0, -9.81f) + Vector3.left;
        Debug.Log(Physics.gravity);
        currentScene = scene.name;
        if (int.TryParse(currentScene, out int a)) Globals.currentScene = currentScene;
        transitionActive = false;
    }

    public void GoToScene(string target)
    {
        StartCoroutine(LoadLevel(target));
    }
}
