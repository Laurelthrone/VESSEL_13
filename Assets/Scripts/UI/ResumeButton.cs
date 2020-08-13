using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResumeButton : MonoBehaviour
{

    public Button button;
    
    void Start()
    {
        button.onClick.AddListener(ResumeGame);
    }
        
    void ResumeGame()
    {
        PauseMenu.unpause();
    }
}
