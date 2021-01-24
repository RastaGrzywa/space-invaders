using System;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreScreen : UIScreen
{
    [SerializeField] private GameObject scoreList;
    [SerializeField] private Score uiScorePrefab;
    private void OnEnable()
    {
        List<PlayerScore> scores = DataManager.LoadScores();

        for (var i = 0; i < scores.Count; i++)
        {
            var playerScore = scores[i];
            Score score = Instantiate(uiScorePrefab);
            score.SetupScore(i + 1, playerScore.score, playerScore.date);
            score.transform.SetParent(scoreList.transform, false);    
        }
    }

    private void OnDisable()
    {
        foreach (Transform scoreListObject in scoreList.transform)
        {
            Destroy(scoreListObject.gameObject);
        }
    }
}
