using System;
using UnityEngine;
using Zenject;

public class ProjectGameplayInstaller : MonoInstaller
{
    [Header("Config")]
    [SerializeField] private GameConfig _gameConfig;

    [Header("Platform (prefabs)")]
    [SerializeField] private YandexPlatform _platformPrefab;
    [SerializeField] private CoroutineRunner _coroutineRunnerPrefab;
    [SerializeField] private StartMenu _startMenuPrefab;

    //[SerializeField] private GameUI _uiPrefab;

    public override void InstallBindings()
    {
        BindConfig();
        BindPlatformObjects();
        BindCoreServices();
        BindStartMenu();
        BindSystems();
        BindStateMachine();
        BindInput();

    }

    private void BindStartMenu()
    {
        Container.Bind<StartMenuController>()
            .AsSingle();

        Container.Bind<StartMenu>()
            .FromComponentInNewPrefab(_startMenuPrefab)
            .AsSingle()
            .NonLazy();
    }

    //    BindUI();

    //private void BindUI()
    //{
    //    Container.BindInterfacesAndSelfTo<GameUI>()
    //        .FromComponentInNewPrefab(_uiPrefab)
    //        .AsSingle();
    //}

    private void BindSystems()
    {
        Container.Bind<LiveSystem>()
            .AsSingle();
        Container.BindInterfacesAndSelfTo<ExperienceSystem>()
            .AsSingle();
        Container.Bind<ScoreSystem>()
            .AsSingle();
    }

    private void BindConfig()
    {
        Container.Bind<GameConfig>()
            .FromInstance(_gameConfig)
            .AsSingle();
    }

    private void BindPlatformObjects()
    {
        Container.Bind<YandexPlatform>()
        .FromComponentInNewPrefab(_platformPrefab)
        .AsSingle()
        .OnInstantiated((InjectContext ctx, YandexPlatform p) =>
        {
            p.gameObject.name = "YandexPlatform";
        })
        .NonLazy();

        Container.Bind<CoroutineRunner>()
            .FromComponentInNewPrefab(_coroutineRunnerPrefab)
            .AsSingle()
            .OnInstantiated((InjectContext ctx, CoroutineRunner r) =>
            {
                r.gameObject.name = "CoroutineRunner";
            })
            .NonLazy();

        Container.Bind<MonoBehaviour>()
            .WithId("CoroutineRunner")
            .To<CoroutineRunner>()
            .FromResolve();
    }

    private void BindCoreServices()
    {
        Container.Bind<IPauseService>().To<PauseService>().AsSingle();

        // модель прогресса (замена GameLoop)
        Container.Bind<IPlayerProgress>().To<PlayerProgress>().AsSingle();

        // сервис платформы (Init/Ready, пауза от рекламы, rewarded/interstitial)
        Container.BindInterfacesAndSelfTo<YandexService>().AsSingle().NonLazy();

        // сейвы (local + cloud, debounce/interval, async load)
        Container.BindInterfacesAndSelfTo<SaveService>().AsSingle().NonLazy();

        // автосейв на изменениях прогресса (если используешь)
        Container.BindInterfacesTo<ProgressAutosave>().AsSingle();

        // загрузка сцен для LoadSceneState
        Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
    }

    private void BindStateMachine()
    {
        Container.Bind<BootstrapState>().AsSingle();
        //Container.Bind<LoadSaveState>().AsSingle();
        Container.Bind<LoadSceneState>().AsSingle();
        Container.Bind<GameStartState>().AsSingle();
        Container.Bind<GameplayState>().AsSingle();
        Container.Bind<PauseState>().AsSingle();

        Container.BindInterfacesAndSelfTo<GameStateMachine>()
            .AsSingle()
            .NonLazy();
    }

    private void BindInput()
    {
        Container.BindInterfacesAndSelfTo<InputManager>()
            .AsSingle()
            .NonLazy();
    }
}
