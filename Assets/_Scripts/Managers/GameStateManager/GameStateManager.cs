using System;
using UnityEngine;

public class GameStateManager   
{
    // Make a singleton to be able to easily access game state manager from any script
    private static GameStateManager _instance;
    
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameStateManager();

            return _instance;
        }
    }

    // Current game state
    public GameState CurrentGameState { get; private set; }

    // Create an event to be able to easily react on change of game state from any script
    public event Action<GameState> OnGameStateChange;
    
    public void ChangeState(GameState newGameState)
    {
        if (newGameState == CurrentGameState) return;

        CurrentGameState = newGameState;
        OnGameStateChange?.Invoke(newGameState);
        
        Debug.Log($"Game state changed to {CurrentGameState}");
    }
}
