//using UnityEngine;

//public sealed class RewardedService : IRewardedService
//{
//    private readonly IYandexService _yandex;
//    private readonly EnvironmentMove _environmentMove;
//    private readonly LiveSystem _liveSystem;
//    private readonly IPauseService _pause;
//    private bool _inProgress;

//    public RewardedService(IYandexService yandex, EnvironmentMove environmentMove, LiveSystem liveSystem,
//        IPauseService pauseService)
//    {
//        _yandex = yandex;
//        _environmentMove = environmentMove;
//        _liveSystem = liveSystem;
//        _pause = pauseService;
//    }


//    public void TryGiveLifes()
//    {
//        if (_inProgress)
//            return;

//        _inProgress = true;

//        _yandex.ShowRewarded(OnLifeRewarded);
//    }

//    public void TryReduceSpeed()
//    {
//        if (_inProgress)
//            return;

//        _inProgress = true;

//        _yandex.ShowRewarded(OnSpeedRewarded);
//    }

//    private void OnLifeRewarded()
//    {
//        _liveSystem.Revive(1);
//        _inProgress = false;
//        Debug.Log("OnLifeRewarded");
//        //_pause.RequestResume();
//    }

//    private void OnSpeedRewarded()
//    {
//        _environmentMove.ReduceSpeedByPercent(0.3f);
//        _inProgress = false;
//        Debug.Log("OnSpeedRewarded");
//        // _pause.RequestResume();
//    }
//}

using System;

public sealed class RewardedService : IRewardedService
{
    private readonly IYandexService _yandex;
    private readonly EnvironmentMove _environmentMove;
    private readonly LiveSystem _liveSystem;

    private bool _inProgress;

    public event Action RewardGranted;

    public RewardedService(
        IYandexService yandex,
        EnvironmentMove environmentMove,
        LiveSystem liveSystem)
    {
        _yandex = yandex;
        _environmentMove = environmentMove;
        _liveSystem = liveSystem;
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
        _environmentMove.ReduceSpeedByPercent(0.3f);
        _inProgress = false;
        RewardGranted?.Invoke();
    }
}