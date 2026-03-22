using Zenject;

public class RunTimeSystem : ITickable
{
    private readonly RunProgressSyncService _progressSyncService;

    public float CurrentRunTime { get; private set; }

    public RunTimeSystem(RunProgressSyncService progressSyncService)
    {
        _progressSyncService = progressSyncService;
    }

    public void Initialize()
    {
        CurrentRunTime = _progressSyncService.ReadRunTime();
    }

    public void ResetForNewRun()
    {
        CurrentRunTime = 0f;
        _progressSyncService.SetRunTime(CurrentRunTime);
    }

    public void Tick()
    {
        CurrentRunTime += UnityEngine.Time.deltaTime;
        _progressSyncService.SetRunTime(CurrentRunTime);
    }
}
