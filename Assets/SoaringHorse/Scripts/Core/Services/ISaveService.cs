using System;

public interface ISaveService
{
    RunSaveData RunData { get; }
    bool IsLoaded { get; }
    event Action Loaded;

    void LoadOrCreate(bool tryLoad = true);

    void SaveLocalNow();
    void RequestCloudSave();

    void CommitFrom(IRunProgress progress, bool requestCloud = true);

    void ResetAllProgress(bool clearCloud);
    bool TryLoadLocal(out RunSaveData data);
}
