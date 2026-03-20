using System;
using UnityEngine;
using Zenject;

public sealed class YandexService : IYandexService, IInitializable, IDisposable
{
    public event Action SdkReady;

    private readonly YandexPlatform _platform;
    private readonly IPauseService _pause;
    private readonly MonoBehaviour _runner;

    private bool _inited;
    private bool _readySent;
    private bool _sdkReadyRaised;

    private bool _rvInProgress;

    private float _nextInterstitialTime = 0f;
    private const float InterstitialCooldownSec = 90f;

    public bool HasPlayer => _platform != null && _platform.HasPlayer;
    public bool IsSdkReady => _platform != null && _platform.IsSdkReady;

    public YandexService(
        YandexPlatform platform,
        IPauseService pause,
        [Inject(Id = "CoroutineRunner")] MonoBehaviour runner)
    {
        _platform = platform;
        _pause = pause;
        _runner = runner;
    }

    public void Initialize()
    {
        if (_platform == null) return;

        _platform.PauseRequested += OnPauseRequested;
        _platform.ResumeRequested += OnResumeRequested;

        _platform.AdOpened += OnAdOpened;
        _platform.AdClosed += OnAdClosed;

        _platform.RvOpened += OnRvOpened;
        _platform.RvClosed += OnRvClosed;

        _platform.SdkReady += OnPlatformSdkReady;

        if (_platform.IsSdkReady)
            RaiseSdkReadyOnce();
    }

    public void Dispose()
    {
        if (_platform == null) return;

        _platform.PauseRequested -= OnPauseRequested;
        _platform.ResumeRequested -= OnResumeRequested;

        _platform.AdOpened -= OnAdOpened;
        _platform.AdClosed -= OnAdClosed;

        _platform.RvOpened -= OnRvOpened;
        _platform.RvClosed -= OnRvClosed;

        _platform.SdkReady -= OnPlatformSdkReady;
    }

    public void Init()
    {
        if (_inited) return;
        _inited = true;
        _platform?.Init();
    }

    public void ReadyOnce()
    {
        if (_readySent) return;
        _readySent = true;
        _platform?.Ready();
    }

    public void TryShowInterstitial_UpgradeBought()
    {
        if (_platform == null) return;
        if (!IsSdkReady) return;

        if (Time.unscaledTime < _nextInterstitialTime) return;
        _nextInterstitialTime = Time.unscaledTime + InterstitialCooldownSec;

        _platform.ShowInterstitial();
    }

    public void ShowRewarded(Action onReward)
    {
#if UNITY_EDITOR
        onReward?.Invoke();
#endif
        if (_rvInProgress)
            return;

        if (_platform == null || !_platform.IsSdkReady)
            return;

        _rvInProgress = true;
        bool rewarded = false;

        void OnRewarded()
        {
            rewarded = true;
        }

        void OnClosed()
        {
            _platform.RvRewarded -= OnRewarded;
            _platform.RvClosed -= OnClosed;
            _rvInProgress = false;

            if (rewarded)
                onReward?.Invoke();
        }

        _platform.RvRewarded += OnRewarded;
        _platform.RvClosed += OnClosed;

        _platform.ShowRewarded();
    }

    private void OnPlatformSdkReady() => RaiseSdkReadyOnce();

    private void RaiseSdkReadyOnce()
    {
        if (_sdkReadyRaised) return;
        _sdkReadyRaised = true;
        SdkReady?.Invoke();
    }

    private void OnPauseRequested() => _pause.RequestPause();
    private void OnResumeRequested() => _pause.RequestResume();

    private void OnAdOpened() => _pause.RequestPause();
    private void OnAdClosed(bool _) => _pause.RequestResume();

    private void OnRvOpened() => _pause.RequestPause();
    private void OnRvClosed() { }
}
