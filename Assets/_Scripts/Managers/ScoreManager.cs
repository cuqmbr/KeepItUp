using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _currentScore;
    private int _highScore;

    [SerializeField] private int _initialRewardMultiplier;
    [SerializeField] private int _maxRewardMultiplier;
    private int _currentRewardMultiplier;

    [SerializeField] private int _initialMaxExperience;
    private int _currentMaxExperience;
    private int _previousMaxExperience;
    private int _currentExperience;

    [Header("Dependencies")] 
    [SerializeField] private UIManager _uiManager;
    
    private void Awake()
    {
        PlayerEvents.OnBallTouched += AddScore;
        PlayerEvents.OnBallTouched += AddExperience;
        PlayerEvents.OnWallTouched += ResetExperienceAndRewardMultiplier;
        PlayerEvents.OnDeath += SaveHighScore;
        GameStateManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void Start()
    {
        LoadHighScore();
    }

    private void Update()
    {
        if (GameStateManager.Instance.CurrentGameState != GameState.Game)
        {
            return;
        }
        
        _uiManager.UpdateExperienceSlider(_currentExperience - _previousMaxExperience, out bool isFullExperienceSlider);

        if (_currentRewardMultiplier >= _maxRewardMultiplier)
        {
            return;
        }

        if (isFullExperienceSlider)
        {
            LevelUp();
        }
    }

    private void AddScore()
    {
        _currentScore += _currentRewardMultiplier;
        _uiManager.SetScoreText(_currentScore);
    }

    private void AddExperience()
    {
        _currentExperience++;
    }

    private void LevelUp()
    {
        _previousMaxExperience = _currentExperience;
        _currentMaxExperience = (int) Math.Ceiling(_currentMaxExperience * 1.75f);
        _currentRewardMultiplier *= 2;
        
        _uiManager.SetRewardMultiplierText(_currentRewardMultiplier);
        _uiManager.SetExperienceSliderValue(0);
        _uiManager.SetExperienceSliderMaxValue(_currentMaxExperience);
    }

    private void ResetExperienceAndRewardMultiplier()
    {
        _currentRewardMultiplier = _initialRewardMultiplier;

        _currentExperience = 0;
        _currentMaxExperience = _initialMaxExperience;
        _previousMaxExperience = 0;

        _uiManager.SetRewardMultiplierText(1);
        _uiManager.SetExperienceSliderValue(0);
        _uiManager.SetExperienceSliderMaxValue(_currentMaxExperience);
    }
    
    private void ResetAllValues()
    {
        _currentScore = 0;
        _uiManager.SetScoreText(0);
        
        ResetExperienceAndRewardMultiplier();
    }

    private void SaveHighScore()
    {
        if (_currentScore < _highScore)
        {
            return;
        }

        _highScore = _currentScore;
        SessionStore.HighScore = _highScore;
    }

    private void LoadHighScore()
    {
        _highScore = SessionStore.HighScore;
    }
    
    private void OnGameStateChange(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Loading:
                break;
            case GameState.Menu:
                break;
            case GameState.PreGame:
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