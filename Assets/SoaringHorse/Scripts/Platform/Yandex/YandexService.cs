using System;
using System.Collections;
using UnityEngine;
using Zenject;

public sealed class YandexService : IYandexService, IInitializable, IDisposable
{
    private readonly YandexPlatform _platform;
    private readonly IPauseService _pause;
    private readonly MonoBehaviour _runner;

    private bool _inited;
    private bool _readySent;

    private bool _rvInProgress;

    private float _nextInterstitialTime = 0f;
    private const float InterstitialCooldownSec = 90f;

    private const bool MockRewardedWhenUnavailable = true;
    private const float MockDelaySeconds = 0.25f;

    public bool HasPlayer => _platform != null && _platform.HasPlayer;
    public bool IsSdkReady => _platform != null && _platform.IsSdkReady;

    public YandexService(YandexPlatform platform, IPauseService pause, [Inject(Id = "CoroutineRunner")] MonoBehaviour runner)
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
        if (_rvInProgress) return;

        bool adsUnavailable =
#if UNITY_WEBGL && !UNITY_EDITOR
            _platform == null || !_platform.IsSdkReady;
#else
            true;
#endif

        if (adsUnavailable)
        {
            if (!MockRewardedWhenUnavailable) return;
            _runner.StartCoroutine(MockRewarded(onReward));
            return;
        }

        _rvInProgress = true;

        bool rewarded = false;
        void OnRewarded() => rewarded = true;

        void OnClosed()
        {
            _platform.RvRewarded -= OnRewarded;
            _platform.RvClosed -= OnClosed;
            _rvInProgress = false;

            if (rewarded) onReward?.Invoke();
        }

        _platform.RvRewarded += OnRewarded;
        _platform.RvClosed += OnClosed;

        _platform.ShowRewarded();
    }

    private IEnumerator MockRewarded(Action onReward)
    {
        _rvInProgress = true;
        yield return new WaitForSecondsRealtime(MockDelaySeconds);
        onReward?.Invoke();
        _rvInProgress = false;
    }

    private void OnPauseRequested() => _pause.RequestPause();
    private void OnResumeRequested() => _pause.RequestResume();

    private void OnAdOpened() => _pause.RequestPause();
    private void OnAdClosed(bool _) => _pause.RequestResume();

    private void OnRvOpened() => _pause.RequestPause();
    private void OnRvClosed() => _pause.RequestResume();
}
