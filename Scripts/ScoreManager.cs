using UnityEngine;

//solid I AND D 
public interface IScoreStorage
{
    int LoadHighScore();
    void SaveHighScore(int score);
}


public class PlayerPrefsScoreStorage : IScoreStorage
{
    const string Key = "asteroids_high_score"; // the "name" of the saved value

    public int LoadHighScore() => PlayerPrefs.GetInt(Key, 0); // 0 if nothing was saved yet

    public void SaveHighScore(int score)
    {
        PlayerPrefs.SetInt(Key, score);
        PlayerPrefs.Save(); // write to disk now
    }
}


public class ScoreManager
{
    readonly IScoreStorage storage; // given from outside = DEPENDENCY INJECTION

    public int Score { get; private set; }
    public int HighScore { get; private set; }

    // Announced every time the score changes: (score, highScore). The HUD listens to it.
    public event System.Action<int, int> Changed;

    public ScoreManager(IScoreStorage storage)
    {
        this.storage = storage;
        HighScore = storage.LoadHighScore(); // load the saved best score at start
    }

    public void Add(int points)
    {
        Score += points;
        if (Score > HighScore) HighScore = Score; // new record
        Changed?.Invoke(Score, HighScore);
    }

    public void Reset()
    {
        Score = 0;
        Changed?.Invoke(Score, HighScore);
    }

    // Writes the best score to the disk. Called at Game Over and when leaving to the menu.
    public void SaveIfBest()
    {
        storage.SaveHighScore(HighScore);
    }
}
