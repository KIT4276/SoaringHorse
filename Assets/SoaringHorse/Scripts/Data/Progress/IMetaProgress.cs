using System;

public interface IMetaProgress
{
    int BestScore { get; }
    int TotalHorseshoes { get; }
    int BestRevives { get; }
    float BestRunTime { get; }

    event Action Changed;

    void ApplyFromSave(MetaSaveData data);

    void SetBestScore(int bestScore);
    void SetTotalHorseshoes(int totalHorseshoes);
    void SetBestRevives(int revives);
    void SetBestRunTime(float bestRunTime);
}
