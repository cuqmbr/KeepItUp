using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public int CurrentScore { get; protected set; }

    [SerializeField] private int _initialRewardMultiplier;
    [SerializeField] private int _maxRewardMultiplier;
    private int _currentRewardMultiplier;

    [SerializeField] private int _initialMaxExperience;
    private int _currentMaxExperience;
    private int _previousMaxExperience;
    private int _currentExperience;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _rewardMultiplierText;
    [SerializeField] private Slider _experienceSlider;
    [SerializeField] private float _sliderSmoothTime;
    [SerializeField] private float _sliderMaxSpeed;
    private float _sliderVelocity;

    private void Awake()
    {
        PlayerEvents.OnBallTouched += AddScore;
        PlayerEvents.OnBallTouched += AddExperience;
        PlayerEvents.OnWallTouched += ResetExperienceAndRewardMultiplier;
        GameStateManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void Update()
    {
        _experienceSlider.value = Mathf.SmoothDamp(_experienceSlider.value, _currentExperience - _previousMaxExperience, ref _sliderVelocity, _sliderSmoothTime, _sliderMaxSpeed);

        if (_currentRewardMultiplier >= _maxRewardMultiplier)
        {
            return;
        }
        
        if (Math.Abs(_experienceSlider.value - _experienceSlider.maxValue) < 0.1f)
        {
            LevelUp();
            _experienceSlider.maxValue = _currentMaxExperience;
            _experienceSlider.value = 0;
        }
    }

    private void AddScore()
    {
        CurrentScore += _currentRewardMultiplier;
        _currentScoreText.text = CurrentScore.ToString();
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
        _rewardMultiplierText.text = $"×{_currentRewardMultiplier}";
    }

    private void ResetExperienceAndRewardMultiplier()
    {
        _currentRewardMultiplier = _initialRewardMultiplier;

        _currentExperience = 0;
        _currentMaxExperience = _initialMaxExperience;
        _previousMaxExperience = 0;

        _rewardMultiplierText.text = "×1";
        _experienceSlider.maxValue = _currentMaxExperience;
    }
    
    private void ResetAllValues()
    {
        CurrentScore = 0;
        _currentScoreText.text = "0";
        
        ResetExperienceAndRewardMultiplier();
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