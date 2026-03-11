using Zenject;

public class InputlayInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<InputManager>()
            .AsSingle()
            .NonLazy();
    }
}
