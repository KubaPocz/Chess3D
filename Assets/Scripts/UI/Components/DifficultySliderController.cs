using TMPro;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySliderController : MonoBehaviour
{
    public static DifficultySliderController Instance;
    [SerializeField] private TextMeshProUGUI difficultyLabel;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    private void Start()
    {
        Slider slider = GetComponent<Slider>();
        slider.value = 1f;
        UpdateDifficultyText((int)slider.value);
        GameEvents.RequestChangeGameDifficulty((int)slider.value);
        slider.onValueChanged.AddListener((float value) => GameEvents.RequestChangeGameDifficulty((int)value));
        GameEvents.OnGameDifficultyChangeRequested += UpdateDifficultyText;        
    }
    public void UpdateDifficultyText(int value)     
    {
        string difficulty;
        if (value <= 2) difficulty = "Very Easy";
        else if (value <= 4) difficulty = "Easy";
        else if (value <= 7) difficulty = "Medium";
        else if (value <= 10) difficulty = "Challenging";
        else if (value <= 13) difficulty = "Hard";
        else if (value <= 16) difficulty = "Very Hard";
        else if (value <= 18) difficulty = "Expert";
        else difficulty = "Master";

        difficultyLabel.text = difficulty;
    }
}
