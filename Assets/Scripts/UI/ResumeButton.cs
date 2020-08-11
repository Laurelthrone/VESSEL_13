using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{

    public Button button;
    
    // Update is called once per frame
    void Start()
    {
        button.onClick.AddListener(ResumeGame);
    }
        
    void ResumeGame()
    {
        PauseMenu.unpause();
    }
}
