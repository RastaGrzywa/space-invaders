using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIController : MonoBehaviour
{
    [SerializeField] private UIScreen loadingScreen;
    [SerializeField] private UIScreen mainMenuScreen;
    [SerializeField] private UIScreen gamePlayScreen;
    [SerializeField] private UIScreen resultScreen;
    [SerializeField] private UIScreen highScoresScreen;

    private EUIState _uiState = EUIState.None;

    [Inject] private GameManager _gameManager;

    private void Start()
    {
        ShowLoading();
    }

    public void ShowLoading()
    {
        ShowUI(EUIState.Loading, loadingScreen, null);
    }
    
    public void ShowMainMenu()
    {
        ShowUI(EUIState.Menu, mainMenuScreen, null);
    }
    
    public void ShowGame()
    {
        ShowUI(EUIState.Game, gamePlayScreen, () =>
        {
            _gameManager.SetupPlayMode();
        });
    }
    
    public void ShowResult()
    {
        ShowUI(EUIState.Result, resultScreen, null);
    }

    public void ShowHighScores()
    {
        ShowUI(EUIState.HighScores, highScoresScreen, null);
    }

    private void ShowUI(EUIState newState, UIScreen screenObject, Action onScreenShowed)
    {
        if (newState == _uiState)
        {
            return;
        }
        screenObject.gameObject.SetActive(true);
        screenObject.Show(onScreenShowed);
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
