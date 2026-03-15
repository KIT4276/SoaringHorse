using System;

public sealed class RewardedService : IRewardedService
{
    private readonly IYandexService _yandex;
   // private readonly EnvironmentMove _environmentMove;
    private readonly LiveSystem _liveSystem;
    private readonly SpeedSystem _speedSystem;
    private bool _inProgress;

    public event Action RewardGranted;

    public RewardedService(
        IYandexService yandex,
        //EnvironmentMove environmentMove,
        LiveSystem liveSystem,
        SpeedSystem speedSystem)
    {
        _yandex = yandex;
       // _environmentMove = environmentMove;
        _liveSystem = liveSystem;
        _speedSystem = speedSystem;
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