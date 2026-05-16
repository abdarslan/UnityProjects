using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    // Start is called before the first frame update
    public TextMeshProUGUI TotalScoreText;
    public TextMeshProUGUI ActiveDriftText;
    public TextMeshProUGUI MultiplierText;
    public Image StreakTimerBar;
    public Image batteryBar;
    public bool isDrifting = false;
    ScoreManager scoreManager;
    void Start()
    {
        instance = this;
        TotalScoreText.text = "Score: 0";
        ActiveDriftText.text = "";
        MultiplierText.text = "";
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isDrifting)
        {
            ActiveDriftText.text = $"Drift: {Mathf.RoundToInt(scoreManager.scoreChunk)}";
            MultiplierText.text = $"x{scoreManager.streak}";
        } else if (!isDrifting && scoreManager.scoreChunk == 0)
        {
            ActiveDriftText.text = "";
            MultiplierText.text = "";
            TotalScoreText.text = $"Score: {scoreManager.score}";
        }
    }
    private void OnEnable() {
        ScoreManager.OnStreakChanged += HandleStreakChanged;
        Controller.OnPlayerDriftStart += HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd += HandlePlayerDriftEnd;
    }
    private void HandlePlayerDriftStart()
    {
        isDrifting = true;
    }
    private void HandlePlayerDriftEnd()
    {
        isDrifting = false;
    }
    private void HandleStreakChanged(int newStreak)
    {
        MultiplierText.text = $"x{newStreak}";
    }
    private void OnDisable() {
        ScoreManager.OnStreakChanged -= HandleStreakChanged;
        Controller.OnPlayerDriftStart -= HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd -= HandlePlayerDriftEnd;
    }
}
