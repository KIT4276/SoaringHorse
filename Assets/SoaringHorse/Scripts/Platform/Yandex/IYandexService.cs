using System;

public interface IYandexService
{
    event Action SdkReady;

    void Init();
    void ReadyOnce();

    void TryShowInterstitial_UpgradeBought();
    void ShowRewarded(Action onReward);

    bool HasPlayer { get; }
    bool IsSdkReady { get; }
}
