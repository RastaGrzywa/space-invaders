
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

public class DataManager
{
    public static void SaveScore(int score)
    {
        PlayerScore newPlayerScore = new PlayerScore();
        newPlayerScore.SetScore(score);
        
        List<PlayerScore> scores = LoadScores();

        if (scores == null)
        {
            scores = new List<PlayerScore>();
            scores.Add(newPlayerScore);
            SaveScoresToSystem(scores);
            return;
        }
        
        int insertScoreIndex = -1;
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerScore playerScore = scores[i];
            if (playerScore.score < score)
            {
                insertScoreIndex = i;
                break;
            }
        }

        if (insertScoreIndex != -1)
        {
            scores.Insert(insertScoreIndex, newPlayerScore);
        }
        else if (scores.Count < 10)
        {
            scores.Add(newPlayerScore);
        }
        if (scores.Count > 10)
        {
            scores.RemoveAt(scores.Count - 1);    
        }

        SaveScoresToSystem(scores);
    }

    private static void SaveScoresToSystem(List<PlayerScore> scores)
    {
        
        string scoresString = JsonConvert.SerializeObject(scores, Formatting.None);
        PlayerPrefs.SetString("Scores", scoresString);
        PlayerPrefs.Save();
    }
    
    public static List<PlayerScore> LoadScores()
    {
        return JsonConvert.DeserializeObject<List<PlayerScore>>(PlayerPrefs.GetString("Scores"));
    }
}
