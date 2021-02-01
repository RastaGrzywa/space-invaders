using System;

[Serializable]
public class PlayerScore
{
    public uint score;
    public string date;

    public void SetScore(uint score)
    {
        this.score = score;
        DateTime scoreDate = DateTime.Now;
        date = scoreDate.ToString("dd-MM-yyyy");
    }
}
