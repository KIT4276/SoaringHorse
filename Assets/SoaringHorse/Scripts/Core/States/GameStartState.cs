using UnityEngine;

public sealed class GameStartState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly ISaveService _save;
    private readonly IPlayerProgress _progress;
    private readonly IYandexService _yandex;
    private readonly LiveSystem _liveSystem;
    private ExperienceSystem _experienceSystem;
    private readonly ScoreSystem _scoreSystem;

    public GameStartState(IGameStateMachine sm, ISaveService save, IPlayerProgress progress, IYandexService yandex, 
        LiveSystem liveSystem, ExperienceSystem experienceSystem, ScoreSystem scoreSystem)
    {
        _sm = sm;
        _save = save;
        _progress = progress;
        _yandex = yandex;
        _liveSystem = liveSystem;
        _experienceSystem = experienceSystem;
        _scoreSystem = scoreSystem;
    }

    public void Enter()
    {
        _yandex.ReadyOnce();
        _save.Data.ApplyTo(_progress);
        _liveSystem.Initialize();
        _experienceSystem.Initialize();
        _scoreSystem.Initialize();

        Time.timeScale = 1f;
        _sm.Enter<GameplayState>();
    }

    public void Exit() { }
}
