using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Score UI")]
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private Slider _experienceSlider;
    [SerializeField] private TextMeshProUGUI _multiplierText;

    public void SetScoreText(int value) => _scoreText.text = value.ToString();

    public void SetExperienceSliderValue(int value) => _experienceSlider.value = value;

    public void SetExperienceSliderMaxValue(int value) => _experienceSlider.maxValue = value;

    public void SetMultiplierText(int value) => _multiplierText.text = $"×{value}";
}
