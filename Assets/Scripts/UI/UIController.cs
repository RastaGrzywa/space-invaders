using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIScreen loadingScreen;
    [SerializeField] private UIScreen mainMenuScreen;
    [SerializeField] private UIScreen gamePlayScreen;
    [SerializeField] private UIScreen resultScreen;
    [SerializeField] private UIScreen highScoresScreen;

    private EUIState _uiState = EUIState.None;
    
    void Start()
    {
        ShowLoading();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            HideUI();
            ShowMainMenu();
        }
    }

    public void ShowLoading()
    {
        ShowUI(EUIState.Loading, loadingScreen);
    }
    
    public void ShowMainMenu()
    {
        ShowUI(EUIState.Menu, mainMenuScreen);
    }
    
    public void ShowGame()
    {
        ShowUI(EUIState.Game, gamePlayScreen);
    }
    
    public void ShowResult()
    {
        ShowUI(EUIState.Result, resultScreen);
    }

    public void ShowHighScores()
    {
        ShowUI(EUIState.HighScores, highScoresScreen);
    }

    private void ShowUI(EUIState newState, UIScreen screenObject)
    {
        if (newState == _uiState)
        {
            return;
        }
        screenObject.gameObject.SetActive(true);
        screenObject.Show();
        _uiState = newState;
    }

    public void HideUI()
    {
        switch (_uiState)
        {
            case EUIState.Loading: loadingScreen.Hide(); break;
            case EUIState.Menu: mainMenuScreen.Hide(); break;
            case EUIState.Game: gamePlayScreen.Hide(); break;
            case EUIState.Result: resultScreen.Hide(); break;
            case EUIState.HighScores: highScoresScreen.Hide(); break;
        }
    }
}
