public sealed class BootstrapState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly IYandexService _yandex;

    public BootstrapState(IGameStateMachine sm, IYandexService yandex)
    {
        _sm = sm;
        _yandex = yandex;
    }

    public void Enter()
    {
        _yandex.Init();
        _sm.Enter<LoadSaveState>();
    }

    public void Exit() { }
}
