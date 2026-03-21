using System;

public sealed class RewardedService : IRewardedService
{
    private readonly IYandexService _yandex;
    private readonly LiveSystem _liveSystem;
    private readonly SpeedSystem _speedSystem;
    private readonly RevivesSystem _revivesSystem;
    private bool _inProgress;

    public event Action RewardGranted;

    public RewardedService(
        IYandexService yandex,
        LiveSystem liveSystem,
        SpeedSystem speedSystem,
        RevivesSystem revivesSystem)
    {
        _yandex = yandex;
        _liveSystem = liveSystem;
        _speedSystem = speedSystem;
        _revivesSystem = revivesSystem;
    }

    public void TryGiveLifes()
    {
        if (_inProgress)
            return;

        _inProgress = true;
        _yandex.ShowRewarded(OnLifeRewarded);
    }

    public void TryReduceSpeed()
    {
        if (_inProgress)
            return;

        _inProgress = true;
        _yandex.ShowRewarded(OnSpeedRewarded);
    }

    private void OnLifeRewarded()
    {
        _liveSystem.Revive(1);
        _revivesSystem.RegisterRevive();
        _inProgress = false;
        RewardGranted?.Invoke();
    }

    private void OnSpeedRewarded()
    {
        _speedSystem.ReduceByPercent(0.3f);
        _inProgress = false;
        RewardGranted?.Invoke();
    }
}
