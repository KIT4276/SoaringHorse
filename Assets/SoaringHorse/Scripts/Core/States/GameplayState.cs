using UnityEngine;

public sealed class GameplayState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly IPauseService _pause;

    public GameplayState(IGameStateMachine sm, IPauseService pause)
    {
        _sm = sm;
        _pause = pause;
    }

    public void Enter()
    {
        Time.timeScale = 1f;
        _pause.PauseRequested += OnPauseRequested;
    }

    public void Exit() => 
        _pause.PauseRequested -= OnPauseRequested;

    private void OnPauseRequested() => 
        _sm.Pause();
}
