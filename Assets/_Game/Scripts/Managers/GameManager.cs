using System;
using UnityEngine;

public enum GameState
{
    GamePlay = 0,
    Victory = 1,
    Fail = 2,
    MainMenu = 3,

}
public class GameManager : Singleton<GameManager>
{
    private GameState currentGameState;
    public static Action openCanvas2dEvent;
    public static Action closeCanvas2dEvent;


    private void Start()
    {
        ChangeState(GameState.MainMenu);
    }
    public void ChangeState(GameState newState)
    {
        currentGameState = newState;
        switch (newState)
        {
            case GameState.GamePlay:
                GamePlayState();
                break;
            case GameState.Victory:
                VictoryState();
                break;
            case GameState.Fail:
                FailState();
                break;
            case GameState.MainMenu:
                MainMenuState();
                break;
        }
    }

    private void MainMenuState()
    {
        Time.timeScale = 0;
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<MainMenu_Canvas>();
        closeCanvas2dEvent?.Invoke();
    }
    
    private void VictoryState()
    {
        Time.timeScale = 0;
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<Victory_Canvas>();
        closeCanvas2dEvent?.Invoke();

    }

    private void FailState()
    {
        Time.timeScale = 0;
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<Fail_Canvas>();
        closeCanvas2dEvent?.Invoke();

    }

    private void GamePlayState()
    {
        Time.timeScale = 1;
        UIManager.Instance.CloseAll();
        UIManager.Instance.OpenUI<GamePlay_Canvas>();
        openCanvas2dEvent?.Invoke();
    }

}
