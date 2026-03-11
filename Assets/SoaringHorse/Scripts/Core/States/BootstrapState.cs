using UnityEngine;

public sealed class BootstrapState : IGameState
{
    private readonly ApplicationStarter _applicationStarter;

    public BootstrapState(ApplicationStarter applicationStarter) => 
        _applicationStarter = applicationStarter;

    public void Enter()
    {
        Debug.Log("[BootstrapState] Enter");
        
        _applicationStarter.InitYandex();
        _applicationStarter.InitStartUI();
    }

    public void Exit() { }
}
