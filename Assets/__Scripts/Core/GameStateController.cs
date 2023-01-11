using UnityEngine;
using System;

public class GameStateController : MonoBehaviour
{
    private static GameState _currentState;
    public static GameState CurrentState => _currentState;

    public static event Action<GameState> OnStateChanged;
    public static event Action OnGameStarted;
    public static event Action OnLevelCompleted;
    public static event Action OnGameOver;

    private void Start()
    {
        ChangeState(GameState.Intro);
    }

    private void OnEnable()
    {
        WaveSpawner.OnAllWavesEnded += () => ChangeState(GameState.LevelCompleted);
    }

    private void OnDisable()
    {
        WaveSpawner.OnAllWavesEnded -= () => ChangeState(GameState.LevelCompleted);
    }

    public static void ChangeState(GameState newState)
    {
        _currentState = newState;
        print("Game state changed to " + _currentState.ToString());

        OnStateChanged?.Invoke(_currentState);

        switch (_currentState)
        {
            case GameState.Start:
                OnGameStarted?.Invoke();
                ChangeState(GameState.Playing);
                break;
            case GameState.GameOver:
                OnGameOver?.Invoke();
                break;
            case GameState.LevelCompleted:
                OnLevelCompleted?.Invoke();
                break;
        }
    }
}