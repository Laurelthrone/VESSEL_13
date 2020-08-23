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
        string name = SceneManager.GetActiveScene().name;

        Globals.nopause = new string[5];
        Globals.nopause[0] = "titlescreen";
        Globals.nopause[1] = "levelselect_crystal";
        Globals.nopause[2] = "levelselect_lab";
        Globals.nopause[3] = "settings";
        Globals.nopause[4] = "levelselect_hell";

        if (Globals.timeScale == 0)
        {
            Globals.timeScale = 1;
            Globals.speedSelected = 3;
        }

        for (int i = 0; i < 5; i++)
        {
            if (Globals.nopause[i] == name)
            {
                Time.timeScale = Globals.timeScale;
                break;
            }
        }
    }

    public static void ready()
    {
        transition.SetFloat("Speed", 1);
    }
        
    // Update is called once per frame
    void Update()
    {
        Debug.Log(Time.timeScale);
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
        if (currentScene == "61")
        {
            GoToScene("endscreen");
            return;
        }

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
        GameObject player;
        if((player = GameObject.Find("Player")) == null) Destroy(player);
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
