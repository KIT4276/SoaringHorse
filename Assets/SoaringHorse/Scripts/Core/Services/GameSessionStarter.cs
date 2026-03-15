using UnityEngine;

public sealed class GameSessionStarter
{
    private readonly ISaveService _save;
    private readonly IRunProgress _progress;
    private readonly IYandexService _yandex;
    private readonly LiveSystem _liveSystem;
    private readonly ScoreSystem _scoreSystem;
    private readonly SpeedSystem _speedSystem;

    public GameSessionStarter(
        ISaveService save,
        IRunProgress progress,
        IYandexService yandex,
        LiveSystem liveSystem,        
        ScoreSystem scoreSystem,
        SpeedSystem speedSystem)
    {
        _save = save;
        _progress = progress;
        _yandex = yandex;
        _liveSystem = liveSystem;
        _scoreSystem = scoreSystem;
        _speedSystem = speedSystem;
    }


    public void StartFromLoadedProgress()
    {
        _yandex.ReadyOnce();

        _save.RunData.ApplyTo(_progress);

        _liveSystem.LoadFromProgress();

        _scoreSystem.Initialize();
        _speedSystem.Initialize(); 

        Time.timeScale = 1f;
    }
}
