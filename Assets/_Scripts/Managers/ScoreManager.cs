using System;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private int _currentScore = 0;
    private int _bestSessionScore = 0;
    private int _bestAllTimeScore;

    private int _initialRewardMultiplier = 1;
    private int _currentRewardMultiplier = 1;
    private int _currentExperience = 0;
    private int _initialMaxExperience = 3;
    private int _currentMaxExperience = 3;
    [SerializeField] private int _maxMultiplier;
    
    private int _initialReward = 1;
    private int _currentReward = 1;

    private void Awake()
    {
        // Get _bestAllTimeScore if available or set it to 0

        PlayerEvents.OnScreenTouched += AddScore;
        PlayerEvents.OnScreenTouched += AddExperience;
        PlayerEvents.OnWallTouched += ResetMultiplierAndReward;
    }

    public void AddScore()
    {
        _currentScore += _currentReward;
        Debug.Log($"XP: {_currentExperience} / {_currentMaxExperience}. SCORE: {_currentScore}. LVL: {_currentRewardMultiplier}. REWARD: {_currentReward}");
    }

    public void AddExperience()
    {
        if (_currentRewardMultiplier >= _maxMultiplier) return;
        _currentExperience++;
        
        if (_currentExperience != _currentMaxExperience) return;
        IncreaseMultiplier();
    }

    private void IncreaseMultiplier()
    {
        _currentExperience = 0;
        _currentRewardMultiplier++;
        _currentMaxExperience = (int) Math.Ceiling(_currentMaxExperience * 1.5f);
        _currentReward = (int)Mathf.Pow(2, _currentRewardMultiplier - 1);
        Debug.Log($"Multiplier Up!");
    }

    public void ResetMultiplierAndReward()
    {
        _currentRewardMultiplier = _initialRewardMultiplier;
        _currentExperience = 0;
        _currentMaxExperience = _initialMaxExperience;

        _currentReward = _initialReward;
        
        Debug.Log($"Multiplier and reward is reseted!");
        Debug.Log($"XP: {_currentExperience} / {_currentMaxExperience}. SCORE: {_currentScore}. LVL: {_currentRewardMultiplier}. REWARD: {_currentReward}");
    }

    private void OnDestroy()
    {
        PlayerEvents.OnScreenTouched -= AddScore;
        PlayerEvents.OnScreenTouched -= AddExperience;
        PlayerEvents.OnWallTouched -= ResetMultiplierAndReward;
    }
}
