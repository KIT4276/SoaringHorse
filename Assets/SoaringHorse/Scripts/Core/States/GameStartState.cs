using UnityEngine;

public sealed class GameStartState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly ISaveService _save;
    private readonly IPlayerProgress _progress;
    private readonly IYandexService _yandex;

    public GameStartState(IGameStateMachine sm, ISaveService save, IPlayerProgress progress, IYandexService yandex)
    {
        _sm = sm;
        _save = save;
        _progress = progress;
        _yandex = yandex;
    }

    public void Enter()
    {
        _yandex.ReadyOnce();
        _save.Data.ApplyTo(_progress);

        Time.timeScale = 1f;
        _sm.Enter<GameplayState>();
    }

    public void Exit() { }
}
