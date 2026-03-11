public sealed class GameStartState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly GameSessionStarter _sessionStarter;

    public GameStartState(IGameStateMachine sm, GameSessionStarter gameSessionStarter)
    {
        _sm = sm;
        _sessionStarter = gameSessionStarter;
    }

    public void Enter()
    {
        _sessionStarter.StartFromLoadedProgress();
        _sm.Enter<GameplayState>();
    }

    public void Exit() { }
}
