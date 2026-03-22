public interface IMetaProgressSyncService
{
    int ReadBestScore();
    int ReadTotalHorseshoes();
    int ReadBestRevives();
    float ReadBestRunTime();

    void SetBestScore(int bestScore);
    void SetTotalHorseshoes(int totalHorseshoes);
    void SetBestRevives(int bestRevives);
    void SetBestRunTime(float bestRunTime);

    void AddHorseshoes(int delta);
    void TrySetBestScore(int candidateScore);
    void TrySetBestRunTime(float candidateRunTime);
    void TrySetBestRevives(int candidateRevives);
}
