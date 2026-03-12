using System;

public interface IRewardedService
{
    event Action RewardGranted;

    void TryGiveLifes();
    void TryReduceSpeed();
}
