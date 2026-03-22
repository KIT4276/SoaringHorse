public sealed class BootstrapState : IGameState
{
    private readonly ApplicationStarter _applicationStarter;
    private readonly IYandexService _yandex;

    public BootstrapState(ApplicationStarter applicationStarter, IYandexService yandex)
    {
        _applicationStarter = applicationStarter;
        _yandex = yandex;
    }

    public void Enter()
    {
        _yandex.SdkReady += OnSdkReady;

        _applicationStarter.InitYandex();
        _applicationStarter.InitStartUI();

        if (_yandex.IsSdkReady)
            _yandex.ReadyOnce();
    }

    public void Exit() => 
        _yandex.SdkReady -= OnSdkReady;

    private void OnSdkReady() => 
        _yandex.ReadyOnce();
}
