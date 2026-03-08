using System;

public interface ISaveService
{
    SaveData Data { get; }
    bool IsLoaded { get; }
    event Action Loaded;

    void LoadOrCreate(bool tryLoad = true);

    void SaveLocalNow();
    void RequestCloudSave();

    void CommitFrom(IPlayerProgress progress, bool requestCloud = true);

    void ResetAllProgress(bool clearCloud);
    bool TryLoadLocal(out SaveData data);
}
