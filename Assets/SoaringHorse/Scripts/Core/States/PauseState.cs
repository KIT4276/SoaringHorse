using UnityEngine;

public sealed class PauseState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly IPauseService _pause;

    public PauseState(IGameStateMachine sm, IPauseService pause)
    {
        _sm = sm;
        _pause = pause;
    }

    public void Enter()
    {
        Time.timeScale = 0f;
        _pause.ResumeRequested += OnResumeRequested;
        // TODO: показать pause UI
    }

    public void Exit()
    {
        _pause.ResumeRequested -= OnResumeRequested;
        Time.timeScale = 1f;
        // TODO: скрыть pause UI
    }

    private void OnResumeRequested() => _sm.Resume();
}