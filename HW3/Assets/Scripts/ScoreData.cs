using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    public int bestScore = 0;
    public List<int> previousAttempts = new List<int>();
}