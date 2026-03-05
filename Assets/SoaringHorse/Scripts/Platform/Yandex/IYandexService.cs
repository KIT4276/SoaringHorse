using System;

public interface IYandexService
{
    void Init();
    void ReadyOnce();

    void TryShowInterstitial_UpgradeBought();
    void ShowRewarded(Action onReward);

    bool HasPlayer { get; }
    bool IsSdkReady { get; }
}
