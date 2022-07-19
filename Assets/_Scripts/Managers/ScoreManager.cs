using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _currentScore = 0;
    private int _bestSessionScore = 0;
    private int _bestAllTimeScore;
    
    [Header("Reward")]
    [SerializeField] private int _initialReward = 1;

    [Header("Experience")]
    [SerializeField] private int _initialMaxExperience = 3;
    private int _currentMaxExperience = 3;
    private int _currentExperience = 0;
    
    [Header("Reward multiplier")]
    [SerializeField] private int _initialRewardMultiplier = 1;
    [SerializeField] private int _maxRewardMultiplier;
    private int _currentRewardMultiplier = 1;

    [Header("UI")] 
    [SerializeField] private UIManager uiManager;

    private async void Awake()
    {
        // Get _bestAllTimeScore if available or set it to 0

        PlayerEvents.OnBallTouched += AddScore;
        PlayerEvents.OnBallTouched += AddExperience;
        PlayerEvents.OnWallTouched += ResetMultiplierAndReward;

        GameStateManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void OnDestroy()
    {
        PlayerEvents.OnBallTouched -= AddScore;
        PlayerEvents.OnBallTouched -= AddExperience;
        PlayerEvents.OnWallTouched -= ResetMultiplierAndReward;
        
        GameStateManager.Instance.OnGameStateChange -= OnGameStateChange;
    }

    private void AddScore()
    {
        _currentScore += _initialReward * _currentRewardMultiplier;
        
        uiManager.SetScoreText(_currentScore);
        
        #if UNITY_EDITOR
            Debug.Log($"XP: {_currentExperience} / {_currentMaxExperience}. " +
                      $"SCORE: {_currentScore}. " +
                      $"LVL: {_currentRewardMultiplier}. " +
                      $"REWARD: {_initialReward * _currentRewardMultiplier}");
        #endif
    }

    private void AddExperience()
    {
        if (_currentRewardMultiplier < _maxRewardMultiplier) 
            _currentExperience++;
        
        if (_currentExperience == _currentMaxExperience)
            IncreaseMultiplier();
        
        uiManager.SetExperienceSliderValue(_currentExperience);
    }

    private void IncreaseMultiplier()
    {
        _currentRewardMultiplier *= 2;
        _currentMaxExperience = (int)Math.Ceiling(_currentMaxExperience * 1.5f);
        _currentExperience = 0;
        
        uiManager.SetMultiplierText(_currentRewardMultiplier);
        uiManager.SetExperienceSliderMaxValue(_currentMaxExperience);

        #if UNITY_EDITOR
            Debug.Log($"Multiplier Up!");
        #endif
    }

    private void ResetMultiplierAndReward()
    {
        _currentRewardMultiplier = _initialRewardMultiplier;
        _currentExperience = 0;
        _currentMaxExperience = _initialMaxExperience;

        uiManager.SetMultiplierText(_currentRewardMultiplier);
        uiManager.SetExperienceSliderValue(_currentExperience);
        uiManager.SetExperienceSliderMaxValue(_currentMaxExperience);
        
        #if UNITY_EDITOR
        Debug.Log($"Multiplier is reseted" +
                  $"XP: {_currentExperience} / {_currentMaxExperience}. " +
                  $"SCORE: {_currentScore}. " +
                  $"LVL: {_currentRewardMultiplier}. " +
                  $"REWARD: {_initialReward * _currentRewardMultiplier}");
        #endif
    }

    private void ResetAllValues()
    {
        ResetMultiplierAndReward();
        
        _currentScore = 0;
        
        uiManager.SetScoreText(_currentScore);
        
        #if UNITY_EDITOR
            Debug.Log("SCORE MANAGER: All values reseted");
        #endif
    }

    private bool IsHighScore => PlayerPrefs.GetInt("HighScore", 0) < _currentScore;

    private void OnGameStateChange(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Enter:
                break;
            case GameState.Menu:
                ResetAllValues();
                break;
            case GameState.Game:
                break;
            case GameState.GameOver:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }
    }
}
