public class ProgressSyncService
{
    private readonly IPlayerProgress _progress;

    public ProgressSyncService(IPlayerProgress progress) => 
        _progress = progress;

    public void SyncExperience(float value) => 
        _progress.AddExp(value);

    public float ReadExperience()
        => _progress.Exp;

    public float ReadScore() =>
        _progress.Score;

    public void SyncScore(float value) => 
        _progress.AddScore(value);

    public int ResdLifes() =>
        _progress.Lifes;

    public void SetLifes(int currentLives) => 
        _progress.SetLifes(currentLives);
}
