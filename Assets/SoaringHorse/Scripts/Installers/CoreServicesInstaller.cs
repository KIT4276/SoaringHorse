using Zenject;

public class CoreServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPauseService>().To<PauseService>().AsSingle();

        Container.Bind<IPlayerProgress>().To<PlayerProgress>().AsSingle();

        Container.BindInterfacesAndSelfTo<YandexService>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<SaveService>().AsSingle().NonLazy();

        Container.BindInterfacesTo<ProgressAutosave>().AsSingle();

        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();

        Container.Bind<GameSessionStarter>().AsSingle();
    }
}
    