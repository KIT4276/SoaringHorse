using Zenject;

public class GameplaySystemsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<LiveSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<ScoreSystem>()
            .AsSingle();
    }
}