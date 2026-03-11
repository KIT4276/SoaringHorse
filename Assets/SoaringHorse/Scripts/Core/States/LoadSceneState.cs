public sealed class LoadSceneState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly ISceneLoader _loader;

    private readonly string _sceneName;

    public LoadSceneState(IGameStateMachine sm, ISceneLoader loader, SceneConfig config)
    {
        _sm = sm;
        _loader = loader;
        _sceneName = config.GameSceneName;
    }

    public void Enter()
    {
        _loader.Done += LoadIsDone;
        _loader.Load(_sceneName);
    }
    public void Exit() => 
        _loader.Done -= LoadIsDone;


    private void LoadIsDone() => 
        _sm.Enter<GameStartState>();
}
