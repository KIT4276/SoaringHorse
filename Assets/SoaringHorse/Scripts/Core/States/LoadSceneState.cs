public sealed class LoadSceneState : IGameState, ITickableState
{
    private readonly IGameStateMachine _sm;
    private readonly ISceneLoader _loader;

    // замени на свой источник (GameConfig / константа)
    private readonly string _sceneName;

    public LoadSceneState(IGameStateMachine sm, ISceneLoader loader)
    {
        _sm = sm;
        _loader = loader;
        _sceneName = "Game"; // TODO: взять из конфига
    }

    public void Enter() => _loader.Load(_sceneName);

    public void Tick()
    {
        if (_loader.IsDone)
            _sm.Enter<GameStartState>();
    }

    public void Exit() { }
}
