
[System.Serializable]
public class PlayerScore
{
    public string Name;
    public int Score;

    public PlayerScore(string name, int score)
    {
        this.Name = name;
        this.Score = score;
    }
}
