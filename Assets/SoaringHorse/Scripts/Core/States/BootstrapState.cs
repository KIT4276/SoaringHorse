using UnityEngine;

public sealed class BootstrapState : IGameState
{
    private readonly IGameStateMachine _sm;
    private readonly IYandexService _yandex;
    private StartMenu _startMenu;

    public BootstrapState(IGameStateMachine sm, IYandexService yandex, StartMenu startMenu)
    {
        _sm = sm;
        _yandex = yandex;
        _startMenu = startMenu;
    }

    public void Enter()
    {
        _yandex.Init();
        _startMenu.Init();
    }

    public void Exit() { }
}
