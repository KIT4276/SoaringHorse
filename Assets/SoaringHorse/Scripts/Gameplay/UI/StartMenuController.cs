public class StartMenuController
{
    private readonly ISaveService _save;
    private readonly IRunProgress _progress;
    private readonly IGameStateMachine _sm;
    private readonly LiveSystem _liveSystem;
    private readonly IPauseService _pauseService;

    public bool HasSave() => _save.TryLoadLocal(out _);

    public StartMenuController(ISaveService save, IRunProgress progress, IGameStateMachine sm, 
        LiveSystem liveSystem, IPauseService pauseService)
    {
        _save = save;
        _progress = progress;
        _sm = sm;
        _liveSystem = liveSystem;
        _pauseService = pauseService;
    }


    public void LoadGame()
    {
        _save.Loaded -= OnLoaded;
        _save.Loaded += OnLoaded;
        _save.LoadOrCreate(true);
    }

    public void StartNewGame()
    {
        _pauseService.ResetState();
        _save.ResetAllProgress(true);
        _progress.ApplyFromSave(_save.RunData);

        _liveSystem.ResetForNewRun(_progress.Lifes);
        _sm.Enter<LoadSceneState>();
    }

    private void OnLoaded()
    {
        _save.Loaded -= OnLoaded;
        _progress.ApplyFromSave(_save.RunData);
        _sm.Enter<LoadSceneState>();
    }
}
