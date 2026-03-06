public sealed class LoadSceneState : IGameState, ITickableState
{
    private readonly IGameStateMachine _sm;
    private readonly ISceneLoader _loader;

    private readonly string _sceneName;

    public LoadSceneState(IGameStateMachine sm, ISceneLoader loader, GameConfig config)
    {
        _sm = sm;
        _loader = loader;
        _sceneName = config.GameSceneName; 
    }

    public void Enter() => _loader.Load(_sceneName);

    public void Tick()
    {
        if (_loader.IsDone)
            _sm.Enter<GameStartState>();
    }

    public void Exit() { }
}
