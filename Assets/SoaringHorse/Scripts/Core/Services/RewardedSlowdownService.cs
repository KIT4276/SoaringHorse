public sealed class RewardedSlowdownService : IRewardedSlowdownService
{
    private readonly IYandexService _yandex;
    private readonly EnvironmentMove _environmentMove;

    private bool _inProgress;

    public RewardedSlowdownService(IYandexService yandex, EnvironmentMove environmentMove)
    {
        _yandex = yandex;
        _environmentMove = environmentMove;
    }

    public void TryReduceSpeed()
    {
        if (_inProgress)
            return;

        _inProgress = true;

        _yandex.ShowRewarded(OnRewarded);
    }

    private void OnRewarded()
    {
        _environmentMove.ReduceSpeedByPercent(0.3f);
        _inProgress = false;
    }
}