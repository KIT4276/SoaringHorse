using Zenject;

public class GameplaySystemsInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.BindInterfacesAndSelfTo<LiveSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<ScoreSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<SpeedSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<RunTimeSystem>()
            .AsSingle();
        Container.Bind<HorseshoeSystem>()
            .AsSingle();
        Container.Bind<RevivesSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<RunMetaSettlementService>()
            .AsSingle()
            .NonLazy();
    }
}
