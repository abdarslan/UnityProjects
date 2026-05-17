using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    // A global reference so other scripts can access the data easily
    public static SaveManager Instance;
    
    public ScoreData currentScoreData;
    private string saveFilePath;

    private void Awake()
    {
        // Simple Singleton pattern so only one SaveManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keeps it alive between scene reloads
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // persistentDataPath is a safe folder Unity guarantees will survive game updates and shutdowns.
        // It works on Windows, Mac, Android, and iOS automatically.
        saveFilePath = Application.persistentDataPath + "/savedScores.json";
        
        LoadScores();
    }

    public void LoadScores()
    {
        if (File.Exists(saveFilePath))
        {
            // Read the text from the file
            string loadJson = File.ReadAllText(saveFilePath);
            
            // Convert the text back into our ScoreData object
            currentScoreData = JsonUtility.FromJson<ScoreData>(loadJson);
        }
        else
        {
            // If no file exists (first time playing), create fresh data
            currentScoreData = new ScoreData();
        }
    }

    public void SaveScores()
    {
        // Convert the ScoreData object into a JSON text string
        string saveJson = JsonUtility.ToJson(currentScoreData, true);
        
        // Write it to the hard drive
        File.WriteAllText(saveFilePath, saveJson);
    }

    // A handy method your GameManager can call at the end of a run
    public void AddNewScore(int newScore)
    {
        // 1. Add to the history list
        currentScoreData.previousAttempts.Add(newScore);

        // Optional Pro-Tip: Keep the list from growing infinitely huge (e.g., max 10 attempts)
        if (currentScoreData.previousAttempts.Count > 10)
        {
            currentScoreData.previousAttempts.RemoveAt(0); // Removes the oldest score
        }

        // 2. Check for a new High Score
        if (newScore > currentScoreData.bestScore)
        {
            currentScoreData.bestScore = newScore;
        }

        // 3. Save to the hard drive immediately
        SaveScores();
    }
}