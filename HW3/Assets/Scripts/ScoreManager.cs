using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    // it keeps a streak system. When a drift ends timer starts from 2 seconds. If another drift starts streak is not lost. 2 steps available to 3x streak. Score chunk accumulates and when drift ends its added to total score. Streak changes the momentary score multiplier
    private static ScoreManager instance;
    // Start is called before the first frame update
    public int score = 0;
    public int streak = 1;
    public float streakTimer = 0f;
    public float streakDuration = 2f;
    public float scoreMultiplier = 1f;
    public float scoreChunk = 0f;
    public float streak1Threshold = 100000f;
    public float streak2Threshold = 300000f;
    public bool isDrifting = false;
    public static event Action<int> OnStreakChanged;
    Controller controller;
    void Start()
    {
        instance = this;
        controller = FindObjectOfType<Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        // if the streak timer is greater than 0, we want to decrease it by the time that has passed since the last frame
        if (isDrifting)
        {
            // if the player is currently drifting, we want to check if the score chunk has reached the threshold for the next streak level
            if (scoreChunk >= streak2Threshold && streak < 3)
            {
                streak = 3;
                OnStreakChanged?.Invoke(streak);
            }
            else if (scoreChunk >= streak1Threshold && streak < 2)
            {
                streak = 2;
                OnStreakChanged?.Invoke(streak);
            }
            scoreChunk += controller.driftIntensity * scoreMultiplier* Time.deltaTime * streak; // we want to multiply the score chunk by the current streak level, so that the player gets more points for maintaining a longer streak
        }else {
            if (streakTimer > 0)
            {
                streakTimer -= Time.deltaTime;
                // if the streak timer reaches 0, we want to reset the streak count and score multiplier
            }
            if (streakTimer <= 0)
            {
                if (streak > 1)
                {
                    streak = 1;
                    OnStreakChanged?.Invoke(streak);
                }
                score += Mathf.RoundToInt(scoreChunk); // when the streak ends, we want to add the accumulated score chunk to the total score and reset the score chunk
                scoreChunk = 0f;
            }
        }
    }
    private void OnEnable() {
        Controller.OnPlayerDriftStart += HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd += HandlePlayerDriftEnd;
    }
    private void HandlePlayerDriftStart()
    {
        isDrifting = true;
    }
    private void HandlePlayerDriftEnd()
    {
        // when a drift ends, we want to add the accumulated score chunk to the total score and reset the score chunk
        streakTimer = streakDuration; // reset the streak timer when a drift ends, so that the player has a chance to start another drift and keep the streak going
        isDrifting = false;
    }
    private void OnDisable() {
        Controller.OnPlayerDriftStart -= HandlePlayerDriftStart;
        Controller.OnPlayerDriftEnd -= HandlePlayerDriftEnd;
    }
}
