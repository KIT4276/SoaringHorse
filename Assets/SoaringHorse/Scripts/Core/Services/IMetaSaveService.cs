using System;

public interface IMetaSaveService
{
    MetaSaveData MetaData { get; }
    bool IsLoaded { get; }
    event Action Loaded;

    void LoadOrCreate();
    void SaveLocalNow();
    void CommitFrom(IMetaProgress progress);
    void ResetAllProgress();
    bool TryLoadLocal(out MetaSaveData data);
}
