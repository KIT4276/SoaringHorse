using UnityEngine;
using Zenject;

public class PauseMenu 
{
    private StartMenuController _startMenuController;
    private IRewardedService _rewardedSlowdownService;

    [Inject]
    public void Construct(
       StartMenuController startMenuController,
       IRewardedService rewardedSlowdownService)
    {
        _startMenuController = startMenuController;
        _rewardedSlowdownService = rewardedSlowdownService;
    }

    public void StartAgain() => 
        _startMenuController.StartNewGame();

    public void ShowRewardedForReduseSpeed() => 
        _rewardedSlowdownService.TryReduceSpeed();

    public void ShowRewardedForLifes() =>
        _rewardedSlowdownService.TryGiveLifes();

    public void Exit() => 
        Application.Quit();
}
