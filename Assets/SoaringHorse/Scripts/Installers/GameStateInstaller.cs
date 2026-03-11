using Zenject;

public class GameStateInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<BootstrapState>().AsSingle();
        Container.Bind<LoadSceneState>().AsSingle();
        Container.Bind<GameStartState>().AsSingle();
        Container.Bind<GameplayState>().AsSingle();
        Container.Bind<PauseState>().AsSingle();

        Container.Bind<IGameStateFactory>()
            .To<GameStateFactory>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<GameStateMachine>()
            .AsSingle()
            .NonLazy();
        //Container.Bind<IGameStateMachine>()
        //    .To<GameStateMachine>()
        //    .FromResolve();
    }
}