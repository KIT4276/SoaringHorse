using System;
using UnityEngine;
using Zenject;

public sealed class MetaSaveService : IMetaSaveService, IInitializable
{
    private const string LocalKeyPrefix = "meta_save_v";
    private static string LocalKey => $"{LocalKeyPrefix}{MetaSaveData.CurrentVersion}";

    private readonly IMetaProgress _progress;

    public MetaSaveData MetaData { get; private set; } = new MetaSaveData();
    public bool IsLoaded { get; private set; }
    public event Action Loaded;

    public MetaSaveService(IMetaProgress progress)
    {
        _progress = progress;
    }

    public void Initialize() =>
        LoadOrCreate();

    public void LoadOrCreate()
    {
        IsLoaded = false;

        if (!TryLoadLocal(out var local))
            local = NewDefault();

        ApplyLoaded(local);
        MetaData.ApplyTo(_progress);
        FinishLoad();
    }

    public void SaveLocalNow()
    {
        if (MetaData == null) MetaData = NewDefault();
        MetaData.TouchNow();
        MetaData.Fixup();

        var json = JsonUtility.ToJson(MetaData);
        PlayerPrefs.SetString(LocalKey, json);
        PlayerPrefs.Save();
    }

    public void CommitFrom(IMetaProgress progress)
    {
        if (MetaData == null) MetaData = NewDefault();
        MetaData.CaptureFrom(progress);
        SaveLocalNow();
    }

    public void ResetAllProgress()
    {
        MetaData = NewDefault();
        MetaData.ApplyTo(_progress);
        SaveLocalNow();
    }

    public bool TryLoadLocal(out MetaSaveData data)
    {
        data = null;
        if (!PlayerPrefs.HasKey(LocalKey)) return false;

        var json = PlayerPrefs.GetString(LocalKey, "");
        if (string.IsNullOrEmpty(json)) return false;

        data = TryParse(json);
        return data != null;
    }

    private static MetaSaveData TryParse(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;

        try
        {
            var data = JsonUtility.FromJson<MetaSaveData>(json);
            if (data == null) return null;
            data.Fixup();
            return data;
        }
        catch
        {
            return null;
        }
    }

    private static MetaSaveData NewDefault()
    {
        var data = new MetaSaveData();
        data.TouchNow();
        data.Fixup();
        return data;
    }

    private void ApplyLoaded(MetaSaveData data)
    {
        MetaData = data ?? NewDefault();
        MetaData.Fixup();
    }

    private void FinishLoad()
    {
        IsLoaded = true;
        Loaded?.Invoke();
    }
}
