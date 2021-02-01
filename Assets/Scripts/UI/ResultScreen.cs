using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class ResultScreen: UIScreen
{
    [SerializeField] private Text wavesAmountText;
    [SerializeField] private Text scoreAmountText;

    public void UpdateResultScreenUI(int wavesAmount, uint playerScore)
    {
        wavesAmountText.text = wavesAmount.ToString();
        scoreAmountText.text = playerScore.ToString();
    }
}
