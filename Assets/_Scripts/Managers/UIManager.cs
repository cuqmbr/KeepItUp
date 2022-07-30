using System;
using System.Linq;
using System.Threading.Tasks;
using DatabaseModels.DataTransferObjets;
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

    [Header("Game Over Menu")]
    [SerializeField] private TextMeshProUGUI _gameOverScoreText;

    [Header("Authentication Menu")] 
    [SerializeField] public GameObject _authenticationMenu;
    [SerializeField] private TMP_InputField _usernameInputField;
    [SerializeField] private TMP_InputField _passwordInputField;
    [SerializeField] private TextMeshProUGUI _authenticationErrorText;
    
    [Header("Scoreboard")]
    [SerializeField] private RectTransform _scoreboardScrollViewContent;
    [SerializeField] public GameObject _scoreboardLoadingScreen;
    [SerializeField] public GameObject _scoreboardLoginTip;
    [SerializeField] private GameObject _scoreboardRecordPrefab;
    [SerializeField] private Color _scoreboardRecordColor1;
    [SerializeField] private Color _scoreboardRecordColor2;
    [SerializeField] private Color _scoreboardPlayerRecordColor;


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

    public Task InstantiateScoreboardRecords(ScoreboardRecordDto[] records, int firstRecordIndex)
    {
        _scoreboardScrollViewContent.sizeDelta = new Vector2(0, records.Length * 100);

        float yPos = 0;

        if (records.Last().User.Username == SessionStore.UserData.Username)
        {
            yPos = _scoreboardScrollViewContent.sizeDelta.y / records.Length / 100;
        }
        else if (records.First().User.Username == SessionStore.UserData.Username)
        {
            yPos = 0;
        }
        else
        {
            yPos = _scoreboardScrollViewContent.sizeDelta.y / records.Length * 2f;
        }
        
        _scoreboardScrollViewContent.localPosition = new Vector3(0, yPos);

        for (int i = 0; i < records.Length; i++)
        {
            var record = Instantiate(_scoreboardRecordPrefab, Vector3.zero, Quaternion.identity, _scoreboardScrollViewContent.transform);
            record.GetComponent<RectTransform>().localPosition = new Vector2(218, -50 - 100 * i);

            if (records[i].User.Username == SessionStore.UserData.Username)
            {
                record.GetComponent<Image>().color = _scoreboardPlayerRecordColor;
            }
            else
            {
                record.GetComponent<Image>().color = i % 2 == 0 ? _scoreboardRecordColor1 : _scoreboardRecordColor2;
            }
            
            record.GetComponentInChildren<TextMeshProUGUI>().text = $"{firstRecordIndex + i + 1}. {records[i].User.Username}: {records[i].Score}";
        }

        return Task.CompletedTask;
    }

    public Task DestroyAllScoreboardRecords()
    {
        for (int i = 0; i < _scoreboardScrollViewContent.transform.childCount; i++)
        {
            Destroy(_scoreboardScrollViewContent.transform.GetChild(i).gameObject);
        }
        
        return Task.CompletedTask;
    }

    public void SetGameOverScore(int value)
    {
        _gameOverScoreText.text = value.ToString();
    }

    public (string username, string password) GetAuthenticationCredentials()
    {
        return (_usernameInputField.text, _passwordInputField.text);
    }

    public void SetAuthenticationErrorMessage(string message)
    {
        _authenticationErrorText.text = message;
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