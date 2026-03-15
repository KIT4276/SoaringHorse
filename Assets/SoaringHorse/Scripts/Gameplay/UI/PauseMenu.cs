using UnityEngine;
using Zenject;

public class PauseMenu 
{
    private StartMenuController _startMenuController;
    private IRewardedService _rewardedService;

    [Inject]
    public void Construct(
       StartMenuController startMenuController,
       IRewardedService rewardedSlowdownService)
    {
        _startMenuController = startMenuController;
        _rewardedService = rewardedSlowdownService;
    }

    public void StartAgain() => 
        _startMenuController.StartNewGame();

    public void ShowRewardedForReduseSpeed() => 
        _rewardedService.TryReduceSpeed();

    public void ShowRewardedForLifes() =>
        _rewardedService.TryGiveLifes();

    //public void Exit() => 
    //    Application.Quit();
}
