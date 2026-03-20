using System;
using UnityEngine;

public sealed class YandexPlatform : MonoBehaviour
{
    public bool IsSdkReady { get; private set; }
    public bool HasPlayer { get; private set; }

    public event Action SdkReady;
    public event Action<bool> PlayerReady;

    public event Action PauseRequested;
    public event Action ResumeRequested;

    public event Action AdOpened;
    public event Action<bool> AdClosed;

    public event Action RvOpened;
    public event Action RvRewarded;
    public event Action RvClosed;

    public event Action<string> CloudDataReceived;

    public void Init()
    {
       // Debug.Log($"[YandexPlatform] Init on GO: {gameObject.name}");
        YandexSDKBridge.Init(gameObject.name);
    }

    public void Ready() => YandexSDKBridge.Ready();

    public void ShowInterstitial() => YandexSDKBridge.ShowInterstitial();
    public void ShowRewarded() => YandexSDKBridge.ShowRewarded();

    public void CloudLoad() => YandexSDKBridge.CloudLoad();
    public void CloudSave(string json) => YandexSDKBridge.CloudSave(json);

    // JS callbacks
    public void OnYsdkInitOk(string _)
    {
        if (IsSdkReady) return;

        IsSdkReady = true;
        Debug.Log("[YandexPlatform] SDK READY");
        SdkReady?.Invoke();
    }

    public void OnYsdkInitError(string msg)
    {
        IsSdkReady = false;
        Debug.LogWarning($"[YandexPlatform] SDK ERROR: {msg}");
    }

    public void OnPlayerReady(string hasPlayer)
    {
        HasPlayer = hasPlayer == "1";
        PlayerReady?.Invoke(HasPlayer);
    }

    public void OnGameApiPause(string _) => PauseRequested?.Invoke();
    public void OnGameApiResume(string _) => ResumeRequested?.Invoke();

    public void OnAdOpen(string _) => AdOpened?.Invoke();
    public void OnAdClose(string wasShown) => AdClosed?.Invoke(wasShown == "1");
    public void OnAdError(string err) => AdClosed?.Invoke(false);

    public void OnRvOpen(string _) => RvOpened?.Invoke();
    public void OnRvReward(string _) => RvRewarded?.Invoke();
    public void OnRvClose(string _) => RvClosed?.Invoke();
    public void OnRvError(string _) => RvClosed?.Invoke();

    public void OnCloudData(string json) => CloudDataReceived?.Invoke(json);
    public void OnCloudSaveOk(string _) { }
    public void OnCloudSaveError(string err) => Debug.LogWarning(err);
}
