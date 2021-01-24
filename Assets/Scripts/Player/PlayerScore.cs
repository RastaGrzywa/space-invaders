using System;

[Serializable]
public class PlayerScore
{
    public int score;
    public string date;

    public void SetScore(int score)
    {
        this.score = score;
        DateTime scoreDate = DateTime.Now;
        date = scoreDate.ToString("dd-MM-yyyy");
    }
}
