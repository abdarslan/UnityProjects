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
    public TextMeshProUGUI StreakTimerText;
    public TextMeshProUGUI FinalScoreText;

    public GameObject gameStartPanel;
    public GameObject gameOverPanel;
    public GameObject gamePlayPanel;

    public bool isDrifting = false;
    public bool isGameStarted = false;
    ScoreManager scoreManager;


    void Start()
    {
        instance = this;
        TotalScoreText.text = "Score: 0";
        ActiveDriftText.text = "";
        MultiplierText.text = "";
        StreakTimerText.text = "";
        scoreManager = FindObjectOfType<ScoreManager>();
        gameOverPanel.SetActive(false);
        gamePlayPanel.SetActive(false);
        gameStartPanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameStarted)
        {
            if (Input.GetAxis("Vertical") > 0.1f) // start the game when the player presses the gas pedal
            {
                isGameStarted = true;
                gameStartPanel.SetActive(false);
                gamePlayPanel.SetActive(true);
            }
        }
        if (isDrifting)
        {
            ActiveDriftText.text = $"Drift: {Mathf.RoundToInt(scoreManager.scoreChunk)}";
            MultiplierText.text = $"x{scoreManager.streak}";
        } else if (scoreManager.scoreChunk == 0)
        {
            ActiveDriftText.text = "";
            MultiplierText.text = "";
            TotalScoreText.text = $"Score: {scoreManager.score}";
            StreakTimerText.text = "";
        } else {
            StreakTimerText.text = $"{scoreManager.streakTimer:F1}s";
        }
    }
    private void OnEnable() {
        ScoreManager.OnStreakChanged += HandleStreakChanged;
        Controller.OnPlayerDriftStart += HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd += HandlePlayerDriftEnd;
        GameOverTrigger.OnPlayerOutOfBounds += HandleGameOver;
    }
    private void HandlePlayerDriftStart()
    {
        isDrifting = true;
        StreakTimerText.text = "";
    }
    private void HandlePlayerDriftEnd()
    {
        isDrifting = false;
    }
    private void HandleStreakChanged(int newStreak)
    {
        MultiplierText.text = $"x{newStreak}";
    }
    private void HandleGameOver()
    {
        FinalScoreText.text = $"Final Score: {scoreManager.score}";
        ActiveDriftText.text = "GAME OVER (Press R to Restart)";
        MultiplierText.text = "";
        StreakTimerText.text = "";
        gameOverPanel.SetActive(true);
        gamePlayPanel.SetActive(false);
        gameStartPanel.SetActive(false);
    }
    private void OnDisable() {
        ScoreManager.OnStreakChanged -= HandleStreakChanged;
        Controller.OnPlayerDriftStart -= HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd -= HandlePlayerDriftEnd;
        GameOverTrigger.OnPlayerOutOfBounds -= HandleGameOver;
    }
}
