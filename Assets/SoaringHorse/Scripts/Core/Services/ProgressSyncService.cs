using JetBrains.Annotations;

public class ProgressSyncService
{
    private readonly IRunProgress _progress;

    public ProgressSyncService(IRunProgress progress) =>
        _progress = progress;

    public float ReadScore() =>
        _progress.Score;

    public void SyncScore(float value) =>
        _progress.AddScore(value);

    public int ReadLifes() =>
        _progress.Lifes;

    public void SetLifes(int currentLives) =>
        _progress.SetLifes(currentLives);

    public float ReadSpeed()
        => _progress.Speed;

    public float ReadRunTime()
        => _progress.RunTime;

    public void SetSpeed(float currentSpeed)
        => _progress.SetSpeed(currentSpeed);

    public void SetRunTime(float currentRunTime)
        => _progress.SetRunTime(currentRunTime);
}
