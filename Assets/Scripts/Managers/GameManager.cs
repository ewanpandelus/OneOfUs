using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private GameState gameState;
    public enum GameState
    {
        Standard,
        Rhythm,

    }
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        gameState = GameState.Standard;
    }

    public void SetGameState(GameState _gameState)
    {
        gameState = _gameState;
        EvaluateGameState();
    }
    private void EvaluateGameState()
    {
        switch (gameState)
        {
            case GameState.Standard:
                PostProcessManager.instance.StandardPostProcess();
                break;
            case GameState.Rhythm:
                PostProcessManager.instance.RhythmPostProcess();
                break;

        }
    }
}
