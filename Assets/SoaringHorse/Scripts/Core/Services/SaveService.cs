using System;
using UnityEngine;
using Zenject;

public sealed class SaveService : ISaveService, ITickable, IDisposable
{
    private const string LocalKeyPrefix = "save_v";
    private static string LocalKey => $"{LocalKeyPrefix}{SaveData.CurrentVersion}";

    private readonly YandexPlatform _platform;

    public SaveData Data { get; private set; } = new SaveData();
    public bool IsLoaded { get; private set; }
    public event Action Loaded;

    private SaveData _localCandidate;
    private bool _waitingCloud;

    private float _nextCloudAllowedTime = 0f;
    private float _cloudDebounceUntil = 0f;
    private bool _cloudDirty = false;

    private const float CloudMinIntervalSec = 20f;
    private const float CloudDebounceSec = 5f;

    private float _cloudLoadDeadline;
    private const float CloudLoadTimeoutSec = 3f;

    public SaveService(YandexPlatform platform)
    {
        _platform = platform;
    }

    public void Dispose()
    {
        if (_platform == null) return;
        _platform.CloudDataReceived -= OnCloudData;
        _platform.PlayerReady -= OnPlayerReady;
        _platform.SdkReady -= OnSdkReady;
    }

    public void LoadOrCreate()
    {
        IsLoaded = false;
        _waitingCloud = false;

        TryLoadLocal(out _localCandidate);

        if (_platform == null)
        {
            ApplyLoaded(_localCandidate ?? NewDefault());
            FinishLoad();
            return;
        }

        _platform.CloudDataReceived -= OnCloudData;
        _platform.CloudDataReceived += OnCloudData;

        _platform.PlayerReady -= OnPlayerReady;
        _platform.PlayerReady += OnPlayerReady;

        _platform.SdkReady -= OnSdkReady;
        _platform.SdkReady += OnSdkReady;

        TryStartCloudLoadOrFinish();
    }

    private void OnSdkReady()
    {
        TryStartCloudLoadOrFinish();
    }

    private void OnPlayerReady(bool hasPlayer)
    {
        TryStartCloudLoadOrFinish();
    }

    private void TryStartCloudLoadOrFinish()
    {

        if (_waitingCloud) return;
        if (!_platform.IsSdkReady)
        {
            ApplyLoaded(_localCandidate ?? NewDefault());
            FinishLoad();
            return;
        }

        if (!_platform.HasPlayer)
        {
            ApplyLoaded(_localCandidate ?? NewDefault());
            FinishLoad();
            return;
        }

        _waitingCloud = true;
        _cloudLoadDeadline = Time.unscaledTime + CloudLoadTimeoutSec;
        _platform.CloudLoad();
    }

    private void OnCloudData(string json)
    {
        if (!_waitingCloud) return;
        _waitingCloud = false;

        var cloud = TryParse(json);

        SaveData chosen;
        if (cloud == null && _localCandidate == null) chosen = NewDefault();
        else if (cloud == null) chosen = _localCandidate;
        else if (_localCandidate == null) chosen = cloud;
        else chosen = PickNewest(_localCandidate, cloud);

        ApplyLoaded(chosen);
        SaveLocalNow();
        FinishLoad();
    }

    public void SaveLocalNow()
    {
        if (Data == null) Data = NewDefault();
        Data.TouchNow();
        Data.Fixup();

        var json = JsonUtility.ToJson(Data);
        PlayerPrefs.SetString(LocalKey, json);
        PlayerPrefs.Save();
    }

    public void CommitFrom(IPlayerProgress progress, bool requestCloud = true)
    {
        if (Data == null) Data = NewDefault();
        Data.CaptureFrom(progress);
        SaveLocalNow();
        if (requestCloud) RequestCloudSave();
    }

    public void RequestCloudSave()
    {
        _cloudDirty = true;
        _cloudDebounceUntil = Time.unscaledTime + CloudDebounceSec;
    }

    public void Tick()
    {
        if (_waitingCloud && Time.unscaledTime >= _cloudLoadDeadline)
        {
            _waitingCloud = false;
            ApplyLoaded(_localCandidate ?? NewDefault());
            FinishLoad();
        }

        if (!_cloudDirty) return;
        if (_platform == null) return;
        if (!_platform.IsSdkReady || !_platform.HasPlayer) return;
        if (Time.unscaledTime < _cloudDebounceUntil) return;
        if (Time.unscaledTime < _nextCloudAllowedTime) return;

        _cloudDirty = false;
        _nextCloudAllowedTime = Time.unscaledTime + CloudMinIntervalSec;

        if (Data == null) Data = NewDefault();
        Data.TouchNow();
        Data.Fixup();

        var json = JsonUtility.ToJson(Data);
        _platform.CloudSave(json);
    }

    public void ResetAllProgress(bool clearCloud)
    {
        Data = NewDefault();
        SaveLocalNow();

        if (clearCloud && _platform != null && _platform.IsSdkReady && _platform.HasPlayer)
        {
            // безопаснее записать дефолтный сейв, чем "{}"
            _platform.CloudSave(JsonUtility.ToJson(Data));
        }
    }

    public bool TryLoadLocal(out SaveData data)
    {
        data = null;
        if (!PlayerPrefs.HasKey(LocalKey)) return false;

        var json = PlayerPrefs.GetString(LocalKey, "");
        if (string.IsNullOrEmpty(json)) return false;

        data = TryParse(json);
        return data != null;
    }

    private static SaveData TryParse(string json)
    {
        if (string.IsNullOrEmpty(json)) return null;

        try
        {
            var d = JsonUtility.FromJson<SaveData>(json);
            if (d == null) return null;
            d.Fixup();
            return d;
        }
        catch
        {
            return null;
        }
    }

    private static SaveData NewDefault()
    {
        var d = new SaveData();
        d.TouchNow();
        d.Fixup();
        return d;
    }

    private void ApplyLoaded(SaveData data)
    {
        Data = data ?? NewDefault();
        Data.Fixup();
    }

    private static SaveData PickNewest(SaveData a, SaveData b) =>
        (a.lastWriteUnixUtc >= b.lastWriteUnixUtc) ? a : b;

    private void FinishLoad()
    {
        IsLoaded = true;
        Loaded?.Invoke();
    }
}