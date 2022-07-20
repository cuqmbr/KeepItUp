using System;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Canvas")] 
    [SerializeField] private Animator _mainMenuAnimator;
    [SerializeField] private Animator _gameMenuAnimator;
    [SerializeField] private Animator _gameOverMenuAnimator;

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChange += OnGameStateChange;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChange -= OnGameStateChange;
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
                _mainMenuAnimator.CrossFade("FadeOut", 0);
                await Task.Delay((int) (Math.Abs(_mainMenuAnimator.GetCurrentAnimatorClipInfo(0)[0].clip.length / _mainMenuAnimator.GetCurrentAnimatorStateInfo(0).speed) * 1000));
                _mainMenuAnimator.gameObject.SetActive(false);
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