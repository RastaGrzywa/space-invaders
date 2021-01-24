using System;
using UnityEngine;
using UnityEngine.UI;


public class MainMenuScreen : UIScreen
{

    public void OnStartGame()
    {
        
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
