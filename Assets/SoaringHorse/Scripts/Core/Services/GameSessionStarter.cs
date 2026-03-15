using UnityEngine;

public sealed class GameSessionStarter
{
    private readonly ISaveService _save;
    private readonly IPlayerProgress _progress;
    private readonly IYandexService _yandex;
    private readonly LiveSystem _liveSystem;
    private readonly ScoreSystem _scoreSystem;

    public GameSessionStarter(
        ISaveService save,
        IPlayerProgress progress,
        IYandexService yandex,
        LiveSystem liveSystem,        
        ScoreSystem scoreSystem)
    {
        _save = save;
        _progress = progress;
        _yandex = yandex;
        _liveSystem = liveSystem;
        _scoreSystem = scoreSystem;
    }


    public void StartFromLoadedProgress()
    {
        _yandex.ReadyOnce();

        _save.Data.ApplyTo(_progress);

        _liveSystem.LoadFromProgress();

        _scoreSystem.Initialize();

        Time.timeScale = 1f;
    }
}
