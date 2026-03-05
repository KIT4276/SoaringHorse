public sealed class LoadSaveState : IGameState, ITickableState
{
    private readonly IGameStateMachine _sm;
    private readonly ISaveService _save;
    private readonly IPlayerProgress _progress;

    public LoadSaveState(IGameStateMachine sm, ISaveService save, IPlayerProgress progress)
    {
        _sm = sm;
        _save = save;
        _progress = progress;
    }

    public void Enter() => _save.LoadOrCreate();

    public void Tick()
    {
        if (!_save.IsLoaded) return;

        _progress.ApplyFromSave(_save.Data);
        _sm.Enter<LoadSceneState>();
    }

    public void Exit() { }
}
