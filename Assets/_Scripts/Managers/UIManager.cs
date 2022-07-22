using System;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("UI Canvas")] 
    [SerializeField] private Animator _mainMenuAnimator;
    [SerializeField] private Animator _gameMenuAnimator;
    [SerializeField] private Animator _gameOverMenuAnimator;
    
    [Header("Game Menu -> Score")]
    [SerializeField] private TextMeshProUGUI _currentScoreText;
    [SerializeField] private TextMeshProUGUI _rewardMultiplierText;
    [SerializeField] private Slider _experienceSlider;
    [SerializeField] private float _sliderSmoothTime;
    private float _sliderVelocity;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChange -= OnGameStateChange;
    }

    public void SetScoreText(int score)
    {
        _currentScoreText.text = $"{score}";
    }

    public void SetRewardMultiplierText(int multiplier)
    {
        _rewardMultiplierText.text = $"×{multiplier}";
    }

    public void SetExperienceSliderValue(int value)
    {
        _experienceSlider.value = value;
    }
    
    public void SetExperienceSliderMaxValue(int value)
    {
        _experienceSlider.maxValue = value;
    }

    public void UpdateExperienceSlider(int targetValue, out bool isFull)
    {
        _experienceSlider.value = Mathf.SmoothDamp(_experienceSlider.value, targetValue, ref _sliderVelocity, _sliderSmoothTime);

        isFull = Math.Abs(_experienceSlider.value - _experienceSlider.maxValue) < 0.1f;
    }
    
    private async void OnGameStateChange(GameState newGameState)
    {
        switch (newGameState)
        {
            case GameState.Loading:
                break;
            case GameState.Menu:
                if (_gameOverMenuAnimator.gameObject.activeSelf)
                {
                    _gameOverMenuAnimator.CrossFade("FadeOut", 0);
                    await Task.Delay((int) (Math.Abs(_gameOverMenuAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / _gameOverMenuAnimator.GetCurrentAnimatorStateInfo(0).speed) * 1000));
                    _gameOverMenuAnimator.gameObject.SetActive(false);
                }
                _mainMenuAnimator.gameObject.SetActive(true);
                _mainMenuAnimator.CrossFade("FadeIn", 0);
                break;
            case GameState.PreGame:
                if (_gameOverMenuAnimator.gameObject.activeSelf)
                {
                    _gameOverMenuAnimator.CrossFade("FadeOut", 0);
                    await Task.Delay((int) (Math.Abs(_gameOverMenuAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / _gameOverMenuAnimator.GetCurrentAnimatorStateInfo(0).speed) * 1000));
                    _gameOverMenuAnimator.gameObject.SetActive(false);
                }
                if (_mainMenuAnimator.gameObject.activeSelf)
                {
                    _mainMenuAnimator.CrossFade("FadeOut", 0);
                    await Task.Delay(10);
                    await Task.Delay((int) (Math.Abs(_mainMenuAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / _mainMenuAnimator.GetCurrentAnimatorStateInfo(0).speed) * 1000));
                    _mainMenuAnimator.gameObject.SetActive(false);
                }
                _mainMenuAnimator.gameObject.SetActive(false);
                _gameOverMenuAnimator.gameObject.SetActive(false);
                _gameMenuAnimator.gameObject.SetActive(true);
                _gameMenuAnimator.CrossFade("FadeIn", 0);
                break;
            case GameState.Game:
                break;
            case GameState.GameOver:
                _gameMenuAnimator.CrossFade("FadeOut", 0);
                await Task.Delay((int) (Math.Abs(_gameMenuAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / _gameMenuAnimator.GetCurrentAnimatorStateInfo(0).speed) * 1000));
                _gameMenuAnimator.gameObject.SetActive(false);
                _gameOverMenuAnimator.gameObject.SetActive(true);
                _gameOverMenuAnimator.CrossFade("FadeIn", 0);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newGameState), newGameState, null);
        }
    }
}