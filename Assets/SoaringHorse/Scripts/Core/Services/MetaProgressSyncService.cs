public sealed class MetaProgressSyncService : IMetaProgressSyncService
{
    private readonly IMetaProgress _progress;

    public MetaProgressSyncService(IMetaProgress progress) =>
        _progress = progress;

    public int ReadBestScore() =>
        _progress.BestScore;

    public int ReadTotalHorseshoes() =>
        _progress.TotalHorseshoes;

    public int ReadBestRevives() =>
        _progress.BestRevives;

    public float ReadBestRunTime() =>
        _progress.BestRunTime;

    public void SetBestScore(int bestScore) =>
        _progress.SetBestScore(bestScore);

    public void SetTotalHorseshoes(int totalHorseshoes) =>
        _progress.SetTotalHorseshoes(totalHorseshoes);

    public void SetBestRevives(int bestRevives) =>
        _progress.SetBestRevives(bestRevives);

    public void SetBestRunTime(float bestRunTime) =>
        _progress.SetBestRunTime(bestRunTime);

    public void AddHorseshoes(int delta)
    {
        if (delta == 0)
            return;

        _progress.SetTotalHorseshoes(_progress.TotalHorseshoes + delta);
    }

    public void TrySetBestScore(int candidateScore)
    {
        if (candidateScore <= _progress.BestScore)
            return;

        _progress.SetBestScore(candidateScore);
    }

    public void TrySetBestRunTime(float candidateRunTime)
    {
        if (candidateRunTime <= 0f)
            return;

        if (_progress.BestRunTime > 0f && candidateRunTime <= _progress.BestRunTime)
            return;

        _progress.SetBestRunTime(candidateRunTime);
    }

    public void TrySetBestRevives(int candidateRevives)
    {
        if (candidateRevives < 0)
            return;

        bool hasRecordedMetaResults =
            _progress.BestScore > 0 ||
            _progress.TotalHorseshoes > 0 ||
            _progress.BestRunTime > 0f ||
            _progress.BestRevives > 0;

        if (hasRecordedMetaResults && candidateRevives >= _progress.BestRevives)
            return;

        _progress.SetBestRevives(candidateRevives);
    }
}
