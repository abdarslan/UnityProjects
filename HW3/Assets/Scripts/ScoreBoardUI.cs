using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Required for TextMeshPro

public class ScoreboardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI historyText;

    // OnEnable automatically runs every time the GameObject this script is attached to becomes visible.
    // This makes it perfect for a Game Over panel.
    private void OnEnable()
    {
        UpdateScoreboardDisplay();
    }

    public void UpdateScoreboardDisplay()
    {
        // Safety check to ensure the SaveManager exists
        if (SaveManager.Instance == null)
        {
            Debug.LogWarning("SaveManager instance not found!");
            return;
        }

        // Grab the saved data from the Singleton
        ScoreData data = SaveManager.Instance.currentScoreData;

        // 1. Display the Best Score
        if (bestScoreText != null)
        {
            bestScoreText.text = "Best Score: " + data.bestScore.ToString();
        }

        // 2. Display the History List
        if (historyText != null)
        {
            // Start with a header
            string formattedHistory = "Recent Attempts:\n";

            // Loop through the history list backwards so the most recent attempt is at the top
            for (int i = data.previousAttempts.Count - 1; i >= 0; i--)
            {
                formattedHistory += "- " + data.previousAttempts[i].ToString() + "\n";
            }

            // Apply the formatted string to the UI text
            historyText.text = formattedHistory;
        }
    }
}