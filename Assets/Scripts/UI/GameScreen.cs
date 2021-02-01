
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GameScreen: UIScreen
{
    [SerializeField] private Text livesAmountText;
    [SerializeField] private Text waveAmountText;
    [SerializeField] private Text scoreAmountText;

    [Inject] private GameSettings _gameSettings;
    public void UpdatePlayerHealthUI(byte healthAmount)
    {
        string livesAmount = "";
        for (int i = 0; i < healthAmount; i++)
        {
            livesAmount += _gameSettings.playerHealthSymbol;
        }

        livesAmountText.text = livesAmount;
    }
    public void UpdateWaveUI(ushort currentWave)
    {
        waveAmountText.text = currentWave.ToString();
    }
    
    public void UpdatePlayerScoreUI(uint playerScore)
    {
        scoreAmountText.text = playerScore.ToString();
    }
}
