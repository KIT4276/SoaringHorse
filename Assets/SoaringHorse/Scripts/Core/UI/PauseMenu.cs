using UnityEngine;
using Zenject;

public class PauseMenu 
{
    private StartMenuController _startMenuController;
    private IRewardedSlowdownService _rewardedSlowdownService;

    [Inject]
    public void Construct(
       StartMenuController startMenuController,
       IRewardedSlowdownService rewardedSlowdownService)
    {
        _startMenuController = startMenuController;
        _rewardedSlowdownService = rewardedSlowdownService;
    }

    public void StartAgain() => 
        _startMenuController.StartNewGame();

    public void ShowRewarded()
    {
        _rewardedSlowdownService.TryReduceSpeed();
    }

    public void Exit() => 
        Application.Quit();
}
