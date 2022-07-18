using System;
using DatabaseModels.Requests;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Canvas")] 
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _gameMenu;
    [SerializeField] private GameObject _gameOverMenu;
    
    [Header("Score Menu")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Slider _experienceSlider;
    [SerializeField] private TextMeshProUGUI _multiplierText;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChange -= OnGameStateChange;
    }

    public void SetScoreText(int value) => _scoreText.text = value.ToString();

    public void SetExperienceSliderValue(int value) => _experienceSlider.value = value;

    public void SetExperienceSliderMaxValue(int value) => _experienceSlider.maxValue = value;

    public void SetMultiplierText(int value) => _multiplierText.text = $"×{value}";

    private void OnGameStateChange(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Enter:
                break;
            case GameState.Menu:
                _gameOverMenu.SetActive(false);
                _mainMenu.SetActive(true);
                break;
            case GameState.Game:
                _mainMenu.SetActive(false);
                _gameMenu.SetActive(true);
                break;
            case GameState.GameOver:
                _gameMenu.SetActive(false);
                _gameOverMenu.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }
    }
}
