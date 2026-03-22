using UnityEngine;

public sealed class GameSessionStarter
{
    private readonly ISaveService _save;
    private readonly IRunProgress _progress;
    private readonly LiveSystem _liveSystem;
    private readonly ScoreSystem _scoreSystem;
    private readonly SpeedSystem _speedSystem;
    private readonly HorseshoeSystem _horseshoeSystem;
    private readonly RunTimeSystem _runTimeSystem;
    private readonly RevivesSystem _revivesSystem;

    public GameSessionStarter(
        ISaveService save,
        IRunProgress progress,
        LiveSystem liveSystem,        
        ScoreSystem scoreSystem,
        SpeedSystem speedSystem,
        HorseshoeSystem horseshoeSystem,
        RunTimeSystem runTimeSystem,
        RevivesSystem revivesSystem)
    {
        _save = save;
        _progress = progress;
        _liveSystem = liveSystem;
        _scoreSystem = scoreSystem;
        _speedSystem = speedSystem;
        _horseshoeSystem = horseshoeSystem;
        _runTimeSystem = runTimeSystem;
        _revivesSystem = revivesSystem;
    }


    public void StartFromLoadedProgress()
    {
        _save.RunData.ApplyTo(_progress);

        _liveSystem.LoadFromProgress();

        _scoreSystem.Initialize();
        _speedSystem.Initialize();
        _runTimeSystem.Initialize();
        _horseshoeSystem.ResetForNewRun();
        _runTimeSystem.ResetForNewRun();
        _revivesSystem.ResetForNewRun();

        Time.timeScale = 1f;
    }
}
