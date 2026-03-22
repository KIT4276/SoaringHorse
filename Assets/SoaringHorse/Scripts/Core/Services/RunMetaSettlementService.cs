using System;
using UnityEngine;
using Zenject;

public sealed class RunMetaSettlementService : IInitializable, IDisposable
{
    private readonly LiveSystem _liveSystem;
    private readonly ScoreSystem _scoreSystem;
    private readonly RunTimeSystem _runTimeSystem;
    private readonly HorseshoeSystem _horseshoeSystem;
    private readonly RevivesSystem _revivesSystem;
    private readonly IMetaProgress _metaProgress;
    private readonly IMetaProgressSyncService _metaSync;
    private readonly IMetaSaveService _metaSave;

    public RunMetaSettlementService(
        LiveSystem liveSystem,
        ScoreSystem scoreSystem,
        RunTimeSystem runTimeSystem,
        HorseshoeSystem horseshoeSystem,
        RevivesSystem revivesSystem,
        IMetaProgress metaProgress,
        IMetaProgressSyncService metaSync,
        IMetaSaveService metaSave)
    {
        _liveSystem = liveSystem;
        _scoreSystem = scoreSystem;
        _runTimeSystem = runTimeSystem;
        _horseshoeSystem = horseshoeSystem;
        _revivesSystem = revivesSystem;
        _metaProgress = metaProgress;
        _metaSync = metaSync;
        _metaSave = metaSave;
    }

    public void Initialize()
    {
        _liveSystem.Death += OnDeath;
    }

    public void Dispose()
    {
        _liveSystem.Death -= OnDeath;
    }

    private void OnDeath()
    {
        int finalScore = Mathf.FloorToInt(_scoreSystem.Score);

        _metaSync.TrySetBestScore(finalScore);
        _metaSync.TrySetBestRunTime(_runTimeSystem.CurrentRunTime);
        _metaSync.AddHorseshoes(_horseshoeSystem.CurrentRunCount);
        _metaSync.TrySetBestRevives(_revivesSystem.CurrentRunCount);

        _metaSave.CommitFrom(_metaProgress);
    }
}
