public class StartMenuController
{
    private readonly ISaveService _save;
    private readonly IPlayerProgress _progress;
    private readonly IGameStateMachine _sm;

    public bool HasSave() => _save.TryLoadLocal(out _);

    public StartMenuController(ISaveService save, IPlayerProgress progress, IGameStateMachine sm)
    {
        _save = save;
        _progress = progress;
        _sm = sm;
    }


    public void LoadGame()
    {
        _save.Loaded -= OnLoaded;
        _save.Loaded += OnLoaded;
        _save.LoadOrCreate(true);
    }

    public void StartNewGame()
    {
        _save.ResetAllProgress(true);
        _progress.ApplyFromSave(_save.Data);
        _sm.Enter<LoadSceneState>();
    }

    private void OnLoaded()
    {
        _save.Loaded -= OnLoaded;
        _progress.ApplyFromSave(_save.Data);
        _sm.Enter<LoadSceneState>();
    }
}
