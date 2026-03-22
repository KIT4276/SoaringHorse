using Zenject;

public class CoreServicesInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        Container.Bind<IPauseService>().To<PauseService>().AsSingle();

        Container.Bind<IRunProgress>().To<RunProgress>().AsSingle();
        Container.Bind<IMetaProgress>().To<MetaProgress>().AsSingle();

        Container.BindInterfacesAndSelfTo<LocalizationService>().AsSingle();

        Container.BindInterfacesAndSelfTo<YandexService>().AsSingle().NonLazy();

        Container.BindInterfacesAndSelfTo<RunSaveService>().AsSingle().NonLazy();
        Container.BindInterfacesAndSelfTo<MetaSaveService>().AsSingle().NonLazy();

        Container.Bind<RunProgressSyncService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MetaProgressSyncService>().AsSingle();

        Container.BindInterfacesTo<ProgressAutosave>().AsSingle();

        Container.BindInterfacesTo<SceneLoader>().AsSingle();

        Container.Bind<ApplicationStarter>().AsSingle();

        Container.Bind<GameSessionStarter>().AsSingle();

    }
}
    
