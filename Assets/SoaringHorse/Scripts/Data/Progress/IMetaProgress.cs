using System;

public interface IMetaProgress
{
    int BestScore { get; }
    int TotalHorseshoes { get; }
    int TotalRevives { get; }
    float BestRunTime { get; }
    string[] UnlockedSkins { get; }

    event Action Changed;

    void SetBestScore(int bestScore);
    void SetTotalHorseshoes(int totalHorseshoes);
    void SetTotalRevives(int revives);
    void SetBestRunTime(float bestRunTime);
    void SetUnlockedSkins(string[] unlockedSkins);
}